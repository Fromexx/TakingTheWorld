using Country;
using UnityEngine;
using UnityEngine.UI;

namespace Assets
{
    public class GeneralAsset : MonoBehaviour
    {
        public static GeneralAsset Instance => _instance;

        private static GeneralAsset _instance;

        [field: SerializeField] public Camera Camera { get; private set; }
        [field: SerializeField] public float MainCountryBallIncreaseFactor { get; private set; }
        [field: SerializeField] public int MaxFactorCount { get; private set; }
        [field: SerializeField] public float TimeBetweenCountryBallSpawn { get; private set; }
        [field: SerializeField] public float TimeBetweenIncreaseCountryBall { get; private set; }
        [field: SerializeField] public Country.Country PlayerCountry { get; private set; }
        [field: SerializeField] public RegionTuneView RegionTuneView { get; private set; }

        private void Awake() => _instance ??= this;
    }
}