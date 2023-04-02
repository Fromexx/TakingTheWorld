using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    [SerializeField] private GameObject _world;

    private void Awake()
    {
        Load();
    }

    private void Load()
    {
        ImportCountriesTo(progressAsset);
    }

    private void ImportCountriesTo(ProgressAsset progressAsset)
    {
        for (int countryIndex = 0; countryIndex < _world.transform.childCount; countryIndex++)
        {
            var countryTransform = _world.transform.GetChild(countryIndex);
            countryTransform.TryGetComponent(out Country country);
            country.Import(progressAsset.Countries[countryIndex]);
            ImportRegionsTo(progressAsset, countryTransform);
        }
    }

    private void ImportRegionsTo(ProgressAsset progressAsset, Transform country)
    {
        for (int regionIndex = 0; regionIndex < country.transform.childCount; regionIndex++)
        {
            var regionTransform = _world.transform.GetChild(regionIndex);
            TryGetComponent(out Region region);
            region.Import(progressAsset.Regions[regionIndex]);
        }
    }
}
