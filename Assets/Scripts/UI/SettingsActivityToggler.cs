using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsActivityToggler : MonoBehaviour
{
    public delegate void GameStopChanged(bool IsStopped);
    public event GameStopChanged OnGameStopChanged;
    public void ToggleActive()
    {
        gameObject.SetActive(!gameObject.activeSelf);
        if (OnGameStopChanged != null)
            OnGameStopChanged(gameObject.activeSelf);
    }
}
