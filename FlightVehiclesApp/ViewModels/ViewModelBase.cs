using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace FlightVehiclesApp
{
    public class ViewModelBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private string _assemblyPath;
        public string AssemblyPath
        {
            get => _assemblyPath;
            set { _assemblyPath = value; OnPropertyChanged(); }
        }

        private ObservableCollection<Type> _aircraftTypes = new ObservableCollection<Type>();
        public ObservableCollection<Type> AircraftTypes
        {
            get => _aircraftTypes;
            set { _aircraftTypes = value; OnPropertyChanged(); }
        }

        private Type _selectedAircraftType;
        public Type SelectedAircraftType
        {
            get => _selectedAircraftType;
            set
            {
                _selectedAircraftType = value;
                OnPropertyChanged();
                LoadMethods();
            }
        }

        private ObservableCollection<MethodInfoWrapper> _methods = new ObservableCollection<MethodInfoWrapper>();
        public ObservableCollection<MethodInfoWrapper> Methods
        {
            get => _methods;
            set { _methods = value; OnPropertyChanged(); }
        }

        private MethodInfoWrapper _selectedMethod;
        public MethodInfoWrapper SelectedMethod
        {
            get => _selectedMethod;
            set { _selectedMethod = value; OnPropertyChanged(); }
        }

        private string _output;
        public string Output
        {
            get => _output;
            set { _output = value; OnPropertyChanged(); }
        }

        private string _statusMessage = "Ready";
        public string StatusMessage
        {
            get => _statusMessage;
            set { _statusMessage = value; OnPropertyChanged(); }
        }

        public ICommand LoadAssemblyCommand { get; }
        public ICommand ExecuteMethodCommand { get; }

        public ViewModelBase()
        {
            LoadAssemblyCommand = new RelayCommand(LoadAssembly);
            ExecuteMethodCommand = new RelayCommand(ExecuteMethod);
        }

        private void LoadAssembly()
        {
            try
            {
                if (!File.Exists(AssemblyPath))
                {
                    StatusMessage = "File not found";
                    return;
                }

                var assembly = Assembly.LoadFrom(AssemblyPath);
                var baseType = typeof(AircraftBase);
                var types = assembly.GetTypes().Where(t => t.IsClass && !t.IsAbstract && baseType.IsAssignableFrom(t));

                AircraftTypes.Clear();
                foreach (var type in types)
                {
                    AircraftTypes.Add(type);
                }

                StatusMessage = $"Loaded {AircraftTypes.Count} aircraft types";
            }
            catch (Exception ex)
            {
                StatusMessage = $"Error loading assembly: {ex.Message}";
            }
        }

        private void LoadMethods()
        {
            Methods.Clear();
            if (SelectedAircraftType == null) return;

            var methods = SelectedAircraftType.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly)
                .Where(m => !m.IsSpecialName); 

            foreach (var method in methods)
            {
                Methods.Add(new MethodInfoWrapper(method));
            }

            StatusMessage = $"Loaded {Methods.Count} methods for {SelectedAircraftType.Name}";
        }

        private void ExecuteMethod()
        {
            if (SelectedMethod == null) return;

            try
            {
                var instance = Activator.CreateInstance(SelectedAircraftType);
                var parameters = SelectedMethod.Parameters.Select(p => Convert.ChangeType(p.Value, p.ParameterType)).ToArray();

                if (instance is AircraftBase aircraft)
                {
                    aircraft.FlightEvent += (sender, message) => 
                    {
                        Output += message + Environment.NewLine;
                    };
                }

                var result = SelectedMethod.MethodInfo.Invoke(instance, parameters);
                if (result != null)
                {
                    Output += $"Result: {result}{Environment.NewLine}";
                }

                StatusMessage = "Method executed successfully";
            }
            catch (Exception ex)
            {
                Output += $"Error: {ex.Message}{Environment.NewLine}";
                StatusMessage = "Error executing method";
            }
        }

        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }

    internal class AircraftBase
    {
        public Action<object, object> FlightEvent { get; internal set; }
    }

    public class MethodInfoWrapper
    {
        public MethodInfo MethodInfo { get; }
        public string Name => MethodInfo.Name;
        public ObservableCollection<ParameterWrapper> Parameters { get; } = new ObservableCollection<ParameterWrapper>();

        public MethodInfoWrapper(MethodInfo methodInfo)
        {
            MethodInfo = methodInfo;
            foreach (var param in methodInfo.GetParameters())
            {
                Parameters.Add(new ParameterWrapper(param));
            }
        }
    }

    public class ParameterWrapper
    {
        public ParameterInfo ParameterInfo { get; }
        public string Name => ParameterInfo.Name;
        public Type ParameterType => ParameterInfo.ParameterType;
        public string Value { get; set; }

        public ParameterWrapper(ParameterInfo parameterInfo)
        {
            ParameterInfo = parameterInfo;
            Value = GetDefaultValue(parameterInfo.ParameterType);
        }

        private string GetDefaultValue(Type type)
        {
            if (type == typeof(string)) return "value";
            if (type == typeof(int)) return "0";
            if (type == typeof(double)) return "0.0";
            if (type == typeof(bool)) return "false";
            return "null";
        }
    }

    public class RelayCommand : ICommand
    {
        private readonly Action _execute;
        private readonly Func<bool> _canExecute;

        public event EventHandler CanExecuteChanged;

        public RelayCommand(Action execute, Func<bool> canExecute = null)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        public bool CanExecute(object parameter) => _canExecute == null || _canExecute();

        public void Execute(object parameter) => _execute();

        public void RaiseCanExecuteChanged() => CanExecuteChanged?.Invoke(this, EventArgs.Empty);
    }
}