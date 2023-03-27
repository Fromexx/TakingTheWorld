using Economy;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Country
{
    public class RegionTuneView : MonoBehaviour
    {
        [SerializeField] private GameObject _regionTune;
        [SerializeField] private TMP_Text _tuneCountryBall;
        [SerializeField] private TMP_Text _tuneMoney;
        [SerializeField] private string _startTuneCountryBallText;
        [SerializeField] private string _startTuneMoneyText;

        private Button _countryBallButton;
        private Button _moneyButton;
        private Region _region;
        private bool _listenersAlreadyAdded;
        
        public void Render(TuneLevel tuneLevel, Region region)
        {
            _region = region;
            
            _regionTune.SetActive(true);

            if (tuneLevel.CurrentCountryBallLevel == tuneLevel.MaxLevel) _tuneCountryBall.text = _startTuneCountryBallText + tuneLevel.GetCountryBallTuneCount() + "\nMax Level";
            else if (tuneLevel.CurrentCountryBallLevel != tuneLevel.MaxLevel) _tuneCountryBall.text = _startTuneCountryBallText + tuneLevel.GetCountryBallTuneCount();

            if (tuneLevel.CurrentMoneyLevel == tuneLevel.MaxLevel) _tuneMoney.text = _startTuneMoneyText + tuneLevel.GetMoneyTune() + "%" + "\nMax Level";
            else if (tuneLevel.CurrentMoneyLevel != tuneLevel.MaxLevel) _tuneMoney.text = _startTuneMoneyText + tuneLevel.GetMoneyTune() + "%";

            if (_listenersAlreadyAdded) return;
            
            AddButtonsListeners();
            _listenersAlreadyAdded = true;
        }

        public void OnClose()
        {
            RemoveButtonsListeners();
            _regionTune.SetActive(false);
            _listenersAlreadyAdded = false;
        }

        private void AddButtonsListeners()
        {
            _tuneCountryBall.transform.parent.TryGetComponent(out _countryBallButton);
            _tuneMoney.transform.parent.TryGetComponent(out _moneyButton);

            _countryBallButton.onClick.AddListener(_region.TuneCountryBallCount);
            _moneyButton.onClick.AddListener(_region.TuneMoney);
        }

        private void RemoveButtonsListeners()
        {
            _countryBallButton.onClick.RemoveAllListeners();
        }
    }
}