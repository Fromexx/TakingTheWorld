using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class BackgroundMusic : MonoBehaviour
{
    [SerializeField] private float musicVolume;
    [SerializeField] private SettingsValues settings;
    private AudioSource audioSorce;

    public void Start()
    {
        audioSorce = GetComponent<AudioSource>();
        OnMutedChanged(!settings.IsMusicUnMuted);
        audioSorce.Play();
        audioSorce.loop = true;
        settings.OnMusicMutedChanged += OnMutedChanged;
    }

    private void OnMutedChanged(bool isMuted)
    {
        if (isMuted) audioSorce.Stop();
        else audioSorce.Play();
    }

    private void OnDestroy()
    {
        settings.OnMusicMutedChanged -= OnMutedChanged;
    }
}
