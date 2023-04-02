using UnityEngine;

public class SettingsActivityToggler : MonoBehaviour
{
    public delegate void GameStopChanged(bool IsStopped);
    public event GameStopChanged OnGameStopChanged;
    public void ToggleActive()
    {
        gameObject.SetActive(!gameObject.activeSelf);
        OnGameStopChanged?.Invoke(gameObject.activeSelf);
    }
}
