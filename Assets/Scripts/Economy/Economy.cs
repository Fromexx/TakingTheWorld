using UnityEngine;

namespace Economy
{
    public class Economy : MonoBehaviour
    {
        [SerializeField] private float _startMoneyCount;

        private static float _currentMoneyCount;
        private static EconomyView _economyView;

        private void Awake()
        {
            _currentMoneyCount = _startMoneyCount;
            
            TryGetComponent(out _economyView);
            _economyView.Render(_currentMoneyCount);
        }

        public static void IncreaseMoney(float money)
        {
            _currentMoneyCount += money;
            _economyView.Render(_currentMoneyCount);
        }

        public static bool DecreaseMoney(float money)
        {
            if (money > _currentMoneyCount) return false;
            
            _currentMoneyCount -= money;
            _economyView.Render(_currentMoneyCount);

            return true;
        }
    }
}