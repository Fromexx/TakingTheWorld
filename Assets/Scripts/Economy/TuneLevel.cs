using System;

namespace Economy
{
    [Serializable]
    public class TuneLevel
    {
        public int CurrentCountryBallLevel { get; private set; }
        public int CurrentMoneyLevel { get; private set; }
        public int MaxLevel => 6;

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
                2 => 500,
                3 => 550,
                4 => 600,
                5 => 700,
                6 => 750,
                _ => -1
            };
        }

        public int GetMoneyTuneCost()
        {
            return CurrentMoneyLevel switch
            {
                2 => 500,
                3 => 550,
                4 => 600,
                5 => 700,
                6 => 750,
                _ => -1
            };
        }

        public int GetCountryBallTuneCount()
        {
            return CurrentCountryBallLevel switch
            {
                1 => 10,
                2 => 15,
                3 => 20,
                4 => 25,
                5 => 30,
                6 => 35,
                _ => -1
            };
        }

        public float GetMoneyTune()
        {
            return CurrentMoneyLevel switch
            {
                1 => 1f,
                2 => 1.05f,
                3 => 1.1f,
                4 => 1.15f,
                5 => 1.2f,
                6 => 1.25f,
                _ => -1
            };
        }
    }
}