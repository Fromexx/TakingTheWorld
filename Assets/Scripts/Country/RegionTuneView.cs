using System;
using Economy;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Country
{
    public class RegionTuneView : MonoBehaviour
    {
        [SerializeField] private GameObject _regionTune;
        [SerializeField] private TMP_Text _tuneCountryBallCost;
        [SerializeField] private TMP_Text _tuneMoneyCost;
        [SerializeField] private TMP_Text _currentCountryBallCount;
        [SerializeField] private TMP_Text _currentMoney;
        
        private Region _region;

        public void Render(TuneLevel tuneLevel, Region region)
        {
            _region = region;
            
            _regionTune.SetActive(true);

            if (tuneLevel.CurrentCountryBallLevel == tuneLevel.MaxLevel)
            {
                _tuneCountryBallCost.text = "-1";
                _currentCountryBallCount.text = tuneLevel.GetCountryBallTuneCount().ToString();
            }
            else if (tuneLevel.CurrentCountryBallLevel != tuneLevel.MaxLevel)
            {
                _currentCountryBallCount.text = tuneLevel.GetCountryBallTuneCount().ToString();
                _tuneCountryBallCost.text = tuneLevel.GetCountryBallTuneCost().ToString();
            }

            if (tuneLevel.CurrentMoneyLevel == tuneLevel.MaxLevel)
            {
                _tuneMoneyCost.text = "-1";
                _currentMoney.text = tuneLevel.GetMoneyTune().ToString();
            }
            else if (tuneLevel.CurrentMoneyLevel != tuneLevel.MaxLevel)
            {
                _currentMoney.text = tuneLevel.GetMoneyTune().ToString();
                _tuneMoneyCost.text = tuneLevel.GetMoneyTuneCost().ToString();
            }
        }

        public void OnClose()
        {
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