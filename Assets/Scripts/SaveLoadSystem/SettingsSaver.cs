using Assets.Scripts.SaveLoadSystem;
using Assets.Scripts.SaveLoadSystem.Models;
using System;
using UnityEngine;

public class SettingsSaver : MonoBehaviour
{
    [SerializeField] private SettingsValues settings;

    private DateTime _lastSave;
    private void Start()
    {
        _lastSave = DateTime.UtcNow;
        settings.OnMusicMutedChanged += SaveSettings;
        settings.OnVoicesMutedChanged += SaveSettings;
    }

    public void SaveSettings()
    {
        SettingsData settingsData = new SettingsData
        {
            MusicMuted = !settings.IsMusicUnMuted,
            VoicesMuted = !settings.IsVoicesUnMuted
        };
        LocalStorage.SaveSettings(settingsData);
        Debug.Log("Settings Saved");
    }

    private void Update()
    {
        if (_lastSave.AddSeconds(13) < DateTime.UtcNow)
        {
            SaveSettings();
            _lastSave = DateTime.UtcNow;
        }
    }

    private void OnApplicationQuit()
    {
        SaveSettings();
    }

    private void OnDestroy()
    {
        settings.OnMusicMutedChanged -= SaveSettings;
        settings.OnVoicesMutedChanged -= SaveSettings;
    }

}
