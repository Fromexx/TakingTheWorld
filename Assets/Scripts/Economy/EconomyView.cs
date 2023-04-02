using System;
using TMPro;
using UnityEngine;

namespace Economy
{
    public class EconomyView : MonoBehaviour
    {
        [SerializeField] private TMP_Text _text;

        public void Render(float money)
        {
            _text.text = Math.Round(money).ToString();
        }
    }
}