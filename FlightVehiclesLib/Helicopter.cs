using System;

namespace FlightVehiclesLib
{
    public class Helicopter : AircraftBase
    {
        public override bool TakeOff()
        {
            for (int i = 0; i <= 100; i += 20)
            {
                Altitude = i;
                System.Threading.Thread.Sleep(100);
            }
            
            FlightEvent?.Invoke(this, "Helicopter took off successfully");
            return true;
        }

        public override void Land()
        {
            for (int i = 100; i >= 0; i -= 20)
            {
                Altitude = i;
                System.Threading.Thread.Sleep(100);
            }
            
            FlightEvent?.Invoke(this, "Helicopter landed successfully");
        }
    }
}