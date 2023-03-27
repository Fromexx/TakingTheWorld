namespace Economy
{
    public class TuneLevel
    {
        public int CurrentCountryBallLevel { get; private set; }
        public int CurrentMoneyLevel { get; private set; }
        public int MaxLevel => 5;

        public TuneLevel(int countryBallTuneLevel, int moneyTuneLevel)
        {
            CurrentCountryBallLevel = countryBallTuneLevel;
            CurrentMoneyLevel = moneyTuneLevel;
        }

        public void UpCountryBallLevel() => CurrentCountryBallLevel++;
        
        public void UpMoneyLevel() => CurrentMoneyLevel++;

        public void DownCountryBallLevel() => CurrentCountryBallLevel--;

        public void DownMoneyLevel() => CurrentMoneyLevel--;
        
        public int GetCountryBallTuneCost()
        {
            return CurrentCountryBallLevel switch
            {
                1 => 100,
                2 => 200,
                3 => 300,
                4 => 400,
                5 => 500,
                _ => -1
            };
        }
        
        public int GetMoneyTuneCost()
        {
            return CurrentMoneyLevel switch
            {
                1 => 100,
                2 => 200,
                3 => 300,
                4 => 400,
                5 => 500,
                _ => -1
            };
        }
        
        public int GetCountryBallTuneCount()
        {
            return CurrentCountryBallLevel switch
            {
                1 => 15,
                2 => 20,
                3 => 25,
                4 => 30,
                5 => 35,
                _ => -1
            };
        }
        
        public float GetMoneyTune()
        {
            return CurrentMoneyLevel switch
            {
                1 => 1.1f,
                2 => 1.2f,
                3 => 1.3f,
                4 => 1.4f,
                5 => 1.5f,
                _ => -1
            };
        }
    }
}