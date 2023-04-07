using Economy;
using System;
using TMPro;
using UnityEngine;

namespace Country
{
    public class RegionTuneView : MonoBehaviour
    {
        public event Action RegionTuneEnabling;
        public event Action RegionTuneDisabling;

        [SerializeField] private GameObject _regionTune;
        [SerializeField] private TMP_Text _tuneCountryBallCost;
        [SerializeField] private TMP_Text _tuneMoneyCost;
        [SerializeField] private TMP_Text _currentCountryBallCount;
        [SerializeField] private TMP_Text _currentMoney;

        private Region _region;

        public void Render(TuneLevel tuneLevel, Region region, bool callFromRegionClick)
        {
            if (callFromRegionClick) RegionTuneEnabling?.Invoke();

            _region = region;

            _regionTune.SetActive(true);

            if (tuneLevel.CurrentCountryBallLevel == tuneLevel.MaxLevel)
            {
                _tuneCountryBallCost.text = "MaxLevel";
                _currentCountryBallCount.text = tuneLevel.GetCountryBallTuneCount().ToString();
            }
            else if (tuneLevel.CurrentCountryBallLevel != tuneLevel.MaxLevel)
            {
                _currentCountryBallCount.text = tuneLevel.GetCountryBallTuneCount().ToString();
                tuneLevel.UpCountryBallLevel();
                _tuneCountryBallCost.text = tuneLevel.GetCountryBallTuneCost().ToString();
                tuneLevel.DownCountryBallLevel();
            }

            if (tuneLevel.CurrentMoneyLevel == tuneLevel.MaxLevel)
            {
                _tuneMoneyCost.text = "MaxLevel";
                _currentMoney.text = tuneLevel.GetMoneyTune().ToString();
            }
            else if (tuneLevel.CurrentMoneyLevel != tuneLevel.MaxLevel)
            {
                _currentMoney.text = tuneLevel.GetMoneyTune().ToString();
                tuneLevel.UpMoneyLevel();
                _tuneMoneyCost.text = tuneLevel.GetMoneyTuneCost().ToString();
                tuneLevel.DownMoneyLevel();
            }
        }

        public void OnClose()
        {
            RegionTuneDisabling?.Invoke();
            _regionTune.SetActive(false);
        }

        public void OnCountryBallTuneClick()
        {
            _region.TuneCountryBallCount();
        }

        public void OnMoneyTuneClick()
        {
            _region.TuneMoney();
        }
    }
}