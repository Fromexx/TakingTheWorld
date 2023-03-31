using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoldierVoicesPlayer : MonoBehaviour
{
    [SerializeField] private SettingsValues settings;
    [SerializeField] private List<AudioSource> voicePool;
    [SerializeField] private float Volume;
    private int currentFreeVoice = 0;

    private void Awake()
    {
        foreach(AudioSource source in voicePool)
        {
            source.volume = Volume;
            source.playOnAwake = false;
            source.loop = false;
        }

    }

    public void Play()
    {
        if (settings.IsVoicesUnMuted == false)
            return;
        voicePool[currentFreeVoice].Play();
        currentFreeVoice = (currentFreeVoice + 1) % voicePool.Count;
    }
}
