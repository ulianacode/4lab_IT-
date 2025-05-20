namespace letatelniyapparat.Models
{
    public class Airplane : Aircraft
    {
        public double RunwayLength { get; set; }

        public Airplane(double runwayLength, double initialAltitude)
            : base(initialAltitude)
        {
            RunwayLength = runwayLength;
        }

        public override bool TakeOff()
        {
            if (RunwayLength >= 1000)
            {
                Altitude = 10000;
                RaiseTakeOffEvent("Самолет успешно взлетел.");
                return true;
            }
            else
            {
                RaiseTakeOffEvent("Самолет не может взлететь: недостаточная длина взлетной полосы.");
                return false;
            }
        }

        public override void Land()
        {
            Altitude = 0;
            RaiseLandingEvent("Самолет успешно приземлился.");
        }
    }

}