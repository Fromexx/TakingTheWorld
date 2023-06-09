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
        OnMutedChanged();
        audioSorce.Play();
        audioSorce.loop = true;
        settings.OnMusicMutedChanged += OnMutedChanged;
    }

    private void OnMutedChanged()
    {
        if (settings.IsMusicUnMuted == false) audioSorce.Stop();
        else audioSorce.Play();
    }

    private void OnDestroy()
    {
        settings.OnMusicMutedChanged -= OnMutedChanged;
    }
}
