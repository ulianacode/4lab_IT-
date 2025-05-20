using System;

namespace FlightVehiclesLib
{
    public class Airplane : AircraftBase
    {
        public double RequiredRunwayLength { get; set; }

        public override bool TakeOff()
        {
            if (RequiredRunwayLength < 1000)
            {
                FlightEvent?.Invoke(this, "Takeoff failed: runway too short");
                return false;
            }

            for (int i = 0; i <= 100; i += 10)
            {
                Altitude = i;
                System.Threading.Thread.Sleep(100);
            }
            
            FlightEvent?.Invoke(this, "Airplane took off successfully");
            return true;
        }

        public override void Land()
        {
            for (int i = 100; i >= 0; i -= 10)
            {
                Altitude = i;
                System.Threading.Thread.Sleep(100);
            }
            
            FlightEvent?.Invoke(this, "Airplane landed successfully");
        }
    }
}