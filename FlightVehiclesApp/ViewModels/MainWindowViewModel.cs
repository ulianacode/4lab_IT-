using FlightVehiclesApp;
using letatelniyapparat.Models;
using ReactiveUI;
using System;
using System.Reactive;

namespace letatelniyapparat.ViewModels
{
    public class MainWindowViewModel : ReactiveObject
    {
        private string _statusMessage = "Готов к взлету.";
        private double _runwayLength;
        private double _altitude;
        private Aircraft _aircraft = new Airplane(1000, 1000);
        private int _selectedAircraftType;

        public string StatusMessage
        {
            get => _statusMessage;
            set => this.RaiseAndSetIfChanged(ref _statusMessage, value);
        }

        public double RunwayLength
        {
            get => _runwayLength;
            set => this.RaiseAndSetIfChanged(ref _runwayLength, value);
        }

        public double Altitude
        {
            get => _altitude;
            set => this.RaiseAndSetIfChanged(ref _altitude, value);
        }

        public int SelectedAircraftType
        {
            get => _selectedAircraftType;
            set
            {
                this.RaiseAndSetIfChanged(ref _selectedAircraftType, value);
                UpdateAircraft();
            }
        }

        public bool IsAirplaneSelected => SelectedAircraftType == 0;
        public bool IsHelicopterSelected => SelectedAircraftType == 1;

        public ReactiveCommand<Unit, Unit> TakeOffCommand { get; }

        public ReactiveCommand<Unit, Unit> LandCommand { get; }

        public MainWindowViewModel()
        {
            TakeOffCommand = ReactiveCommand.Create(TakeOff);
            LandCommand = ReactiveCommand.Create(Land);

            UpdateAircraft();
        }

        private void UpdateAircraft()
        {
            _aircraft = SelectedAircraftType == 0
                ? new Airplane(RunwayLength, Altitude)
                : new Helicopter(Altitude);

            _aircraft.OnTakeOff += (sender, message) => StatusMessage = message;
            _aircraft.OnLanding += (sender, message) => StatusMessage = message;
        }

        private void TakeOff()
        {
            if (_aircraft.TakeOff())
            {
                StatusMessage = _aircraft is Airplane
                    ? "Самолет успешно взлетел."
                    : "Вертолет успешно взлетел.";
            }
            else
            {
                StatusMessage = "Взлет не удался.";
            }
        }

        private void Land()
        {
            _aircraft.Land();
            StatusMessage = _aircraft is Airplane
                ? "Самолет успешно приземлился."
                : "Вертолет успешно приземлился.";
        }
    }



}