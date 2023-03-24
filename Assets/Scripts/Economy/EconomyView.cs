using TMPro;
using UnityEngine;

namespace Economy
{
    public class EconomyView : MonoBehaviour
    {
        [SerializeField] private TMP_Text _text;
        
        public void Render(int money)
        {
            _text.text = money.ToString();
        }
    }
}