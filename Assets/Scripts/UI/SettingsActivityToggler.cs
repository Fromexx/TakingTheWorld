using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsActivityToggler : MonoBehaviour
{
    public void ToggleActive()
    {
        gameObject.SetActive(!gameObject.active);
    }
}
