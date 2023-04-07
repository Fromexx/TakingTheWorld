using Assets;
using UnityEngine;

public class SettingsActivityToggler : MonoBehaviour
{
    public delegate void GameStopChanged(bool IsStopped);
    public event GameStopChanged OnGameStopChanged;
    public void ToggleActive(bool active)
    {
        gameObject.SetActive(active);
        GeneralAsset.Instance.IsSettingsWindowOpen = active;
        OnGameStopChanged?.Invoke(active);
    }
}
