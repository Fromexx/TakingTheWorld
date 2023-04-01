using System;
using UnityEngine;

namespace DefaultNamespace
{
    public class ScreenResolution : MonoBehaviour
    {
        private void Awake()
        {
            Screen.SetResolution(1080, 1920, true);
        }
    }
}