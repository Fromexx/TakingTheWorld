using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsValues : MonoBehaviour
{
    public bool IsMusicUnMuted { get; set; } = true;
    private bool LastMusicUnMuted = true;
    public delegate void MusicMutedChanged(bool IsMuted);
    public event MusicMutedChanged OnMusicMutedChanged;
    public bool IsVibrationMuted { get; set; } = true;
    private bool LastVibrationUnMuted = true;
    public delegate void VibrationMutedChanged(bool IsMuted);
    public event VibrationMutedChanged OnVibrationMutedChanged;
    public bool IsVoicesUnMuted { get; set; } = true;
    private bool LastVoicesUnMuted = true;
    public delegate void VoicesMutedChanged(bool IsMuted);
    public event VoicesMutedChanged OnVoicesMutedChanged;

    private void Update()
    {
        if (LastMusicUnMuted != IsMusicUnMuted && OnMusicMutedChanged != null)
            OnMusicMutedChanged(!IsMusicUnMuted);
        LastMusicUnMuted = IsMusicUnMuted;

        if (LastVibrationUnMuted != IsVibrationMuted && OnVibrationMutedChanged != null)
            OnVibrationMutedChanged(!IsVibrationMuted);
        LastVibrationUnMuted= IsVibrationMuted;

        if (LastVoicesUnMuted != IsVoicesUnMuted && OnVoicesMutedChanged != null)
            OnVoicesMutedChanged(!IsVoicesUnMuted);
        LastVoicesUnMuted= IsVoicesUnMuted;
        
    }
}
