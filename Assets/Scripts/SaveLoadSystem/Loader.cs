using Assets;
using Assets.Scripts.SaveLoadSystem;
using Country;
using UnityEngine;

public class Loader : MonoBehaviour
{
    [SerializeField] private GameObject _world;

    private void Awake()
    {
        Load();
    }

    private void Load()
    {
        ProgressAsset progressAsset = LocalStorage.GetProgress();
        ImportCountriesFrom(progressAsset);
    }

    private void ImportCountriesFrom(ProgressAsset progressAsset)
    {
        for (int countryIndex = 0; countryIndex < _world.transform.childCount; countryIndex++)
        {
            var countryTransform = _world.transform.GetChild(countryIndex);
            countryTransform.TryGetComponent(out Country.Country country);
            country.Import(progressAsset.Countries[countryIndex]);
            ImportRegionsFrom(progressAsset, countryTransform);
        }
    }

    private void ImportRegionsFrom(ProgressAsset progressAsset, Transform country)
    {
        for (int regionIndex = 0; regionIndex < country.transform.childCount; regionIndex++)
        {
            var regionTransform = _world.transform.GetChild(regionIndex);
            TryGetComponent(out Region region);
            region.Import(progressAsset.Regions[regionIndex]);
        }
    }
}
