using System;

namespace letatelniyapparat.Models
{
    public abstract class Aircraft
    {
        public event EventHandler<string>? OnTakeOff;
        public event EventHandler<string>? OnLanding;

        public double Altitude { get; protected set; }

        public Aircraft(double altitude)
        {
            Altitude = altitude;
        }

        public abstract bool TakeOff();
        public abstract void Land();

        protected void RaiseTakeOffEvent(string message)
        {
            OnTakeOff?.Invoke(this, message);
        }

        protected void RaiseLandingEvent(string message)
        {
            OnLanding?.Invoke(this, message);
        }
    }
}
