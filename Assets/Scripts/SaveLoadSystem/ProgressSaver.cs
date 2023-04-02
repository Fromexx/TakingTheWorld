using Assets;
using Assets.Scripts.SaveLoadSystem;
using Country;
using System;
using UnityEngine;

public class ProgressSaver : MonoBehaviour
{
    private DateTime _lastSave;
    [SerializeField] private GameObject _world;
    void Start()
    {
        _lastSave = DateTime.Now;
    }

    void Update()
    {
        if (_lastSave.AddSeconds(10) < DateTime.Now)
        {
            Save();
            Debug.Log("Game Saved");
            _lastSave = DateTime.Now;
        }
    }

    private void Save()
    {
        ProgressAsset SaveProfile = MakeSaveAsset();
        LocalStorage.SaveProgress(SaveProfile);
    }

    private ProgressAsset MakeSaveAsset()
    {
        ProgressAsset saveProfile = new ProgressAsset();
        ExportCountriesTo(saveProfile);
        return saveProfile;
    }

    private void ExportCountriesTo(ProgressAsset saveProfile)
    {
        for (int countryIndex = 0; countryIndex < _world.transform.childCount; countryIndex++)
        {
            var country = _world.transform.GetChild(countryIndex);
            saveProfile.Countries.Add(country.GetComponent<Country.Country>().Export());
            ExportRegionsTo(saveProfile, country);
        }
    }
    private void ExportRegionsTo(ProgressAsset saveProfile, Transform country)
    {
        for (int regionIndex = 0; regionIndex < country.transform.childCount; regionIndex++)
        {
            var region = country.transform.GetChild(regionIndex);
            saveProfile.Regions.Add(region.GetComponent<Region>().Export());
        }
    }
    private void OnApplicationQuit()
    {
        Save();
        Debug.Log("Game saved while closing");
    }
}
