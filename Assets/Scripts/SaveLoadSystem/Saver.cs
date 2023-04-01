using Assets;
using Country;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.AssetDatabaseExperimental.AssetDatabaseCounters;

public class Saver : MonoBehaviour
{
    private DateTime _lastSave;
    [SerializeField] private GameObject _world;
    [SerializeField] private YandexSDK _sdk;
    void Start()
    {
        if(_sdk.user.id == null)
            _sdk.Authenticate();
        else
        {
            _lastSave= DateTime.Now;
        }
    }

    void Update()
    {
        if(_lastSave.AddSeconds(4) < DateTime.Now && _sdk.user.id != null)
        {
            Save();
            _lastSave= DateTime.Now;
        }   
    }

    private void Save()
    {
        ProgressAsset SaveProfile = MakeSaveAsset();

        string profilejson = JsonUtility.ToJson(SaveProfile);
        _sdk.SaveUserData(profilejson);
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
            var region = _world.transform.GetChild(regionIndex);
            saveProfile.Regions.Add(region.GetComponent<Region>().Export());
        }
    }
}
