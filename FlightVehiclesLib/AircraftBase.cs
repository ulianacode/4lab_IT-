using System;

namespace FlightVehiclesLib
{
    public abstract class AircraftBase
    {
        public event EventHandler<string> FlightEvent;

        private double _altitude;
        public double Altitude
        {
            get => _altitude;
            protected set
            {
                _altitude = value;
                FlightEvent?.Invoke(this, $"Altitude changed to {_altitude} meters");
            }
        }

        protected AircraftBase()
        {
            Altitude = 0;
        }

        public abstract bool TakeOff();
        public abstract void Land();
    }
}