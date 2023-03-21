using System;
using UnityEngine;

namespace Singleton
{
    public class GeneralAsset : MonoBehaviour
    {
        public static GeneralAsset Instance => _instance;

        private static GeneralAsset _instance;

        [field: SerializeField] public Camera Camera;

        private void Awake()
        {
            if (_instance is null) _instance = this;
        }
    }
}