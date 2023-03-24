using UnityEngine;

namespace Economy
{
    public class Economy : MonoBehaviour
    {
        [SerializeField] private int _startMoneyCount;

        private static int _currentMoneyCount;
        private static EconomyView _economyView;

        private void Awake()
        {
            _currentMoneyCount = _startMoneyCount;
            
            TryGetComponent(out _economyView);
            _economyView.Render(_currentMoneyCount);
        }

        public static void IncreaseMoney(int money)
        {
            _currentMoneyCount += money;
            _economyView.Render(_currentMoneyCount);
        }

        public static bool DecreaseMoney(int money)
        {
            if (money > _currentMoneyCount) return false;
            
            _currentMoneyCount -= money;
            _economyView.Render(_currentMoneyCount);

            return true;
        }
    }
}