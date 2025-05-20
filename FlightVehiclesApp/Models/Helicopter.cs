namespace letatelniyapparat.Models
{
    public class Helicopter : Aircraft
    {
        public Helicopter(double altitude)
            : base(altitude)
        {
        }

        public override bool TakeOff()
        {
            if (Altitude > 0)
            {
                RaiseTakeOffEvent("Вертолет успешно взлетел.");
                return true;
            }
            else
            {
                RaiseTakeOffEvent("Вертолет не может взлететь: высота не задана.");
                return false;
            }
        }

        public override void Land()
        {
            Altitude = 0;
            RaiseLandingEvent("Вертолет успешно приземлился.");
        }
    }

}