using Assets;
using Assets.Scripts.Country.Region;
using Assets.Scripts.SaveLoadSystem;
using Country;
using System.Linq;
using UnityEngine;

public class ProgressLoader : MonoBehaviour
{
    [SerializeField] private GameObject _world;

    private void Awake()
    {
        Debug.Log("Trying to load a game");
        Load();
    }

    private void Load()
    {
        ProgressAsset progressAsset = LocalStorage.GetProgress();
        if (progressAsset == null)
            return;
        ImportCountriesFrom(progressAsset);
        foreach (var country in _world.GetComponentsInChildren<Assets.Scripts.Country.Country>()) country.AddOwnRegions();
        Debug.Log("Game loaded");
    }

    private void ImportCountriesFrom(ProgressAsset progressAsset)
    {
        for (int countryIndex = 0; countryIndex < _world.transform.childCount; countryIndex++)
        {
            var countryTransform = _world.transform.GetChild(countryIndex);
            countryTransform.TryGetComponent(out Assets.Scripts.Country.Country country);
            if (progressAsset.Countries.Count == 0) return;
            country.Import(progressAsset.Countries.First(c => c.Id == country.Id));
            ImportRegionsFrom(progressAsset, countryTransform);
        }
    }

    private void ImportRegionsFrom(ProgressAsset progressAsset, Transform country)
    {
        for (int regionIndex = 0; regionIndex < country.transform.childCount; regionIndex++)
        {
            var regionTransform = country.transform.GetChild(regionIndex);
            var region = regionTransform.GetComponent<Region>();
            var regionId = region.Id;
            region.Import(progressAsset.Regions.First(reg => reg.Id == regionId));
        }
    }
}
