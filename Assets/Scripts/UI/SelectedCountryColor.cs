using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class SelectedCountryColor : MonoBehaviour
    {
        [SerializeField] private Image _image;

        public void SetSelectedCountryColor(Color color) => _image.color = color;

        public void EnableGameObject() => gameObject.SetActive(true);
    }
}