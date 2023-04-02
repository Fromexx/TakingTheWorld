using UnityEngine;

public class SettingsValues : MonoBehaviour
{
    public bool IsMusicUnMuted { get; set; } = true;
    private bool LastMusicUnMuted = true;
    public delegate void MusicMutedChanged();
    public event MusicMutedChanged OnMusicMutedChanged;

    public bool IsVoicesUnMuted { get; set; } = true;
    private bool LastVoicesUnMuted = true;
    public delegate void VoicesMutedChanged();
    public event VoicesMutedChanged OnVoicesMutedChanged;

    private void Update()
    {
        if (LastMusicUnMuted != IsMusicUnMuted)
            OnMusicMutedChanged?.Invoke();
        LastMusicUnMuted = IsMusicUnMuted;

        if (LastVoicesUnMuted != IsVoicesUnMuted)
            OnVoicesMutedChanged?.Invoke();
        LastVoicesUnMuted = IsVoicesUnMuted;

    }
}
