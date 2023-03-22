using UnityEngine;

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
        [field: SerializeField] public int MaxCountryBallCount { get; private set; }
        [field: SerializeField] public float TimeBetweenIncreaseCountryBall { get; private set; }

        private void Awake() => _instance ??= this;
    }
}