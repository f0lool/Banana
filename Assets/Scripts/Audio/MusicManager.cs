using UnityEngine;

public class MusicManager : MonoBehaviour
{
    private static MusicManager Instance;
    private static AudioSource _audioSource;
    private static MusicLibrary _musicLibrary;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            _audioSource = GetComponent<AudioSource>();
            _musicLibrary = GetComponent<MusicLibrary>();
            DontDestroyOnLoad(gameObject);
        } else
        {
            Destroy(gameObject);
        }
    }

    public static void PlayBackgroundMusicMenu()
    {
        _audioSource.clip = _musicLibrary.GetMenuMusic();

        _audioSource.Play();
    }

    public static void PlayBackgroundMusicGame()
    {
        _audioSource.clip = _musicLibrary.GetRandomClip();

        _audioSource.Play();
    }

    public void PauseBackgroundMusic() 
    {
        _audioSource.Pause();
    }

    public static void SetVolume(float volume)
    {
        _audioSource.volume = volume;
    }

    public static float GetVolume()
    {
        return _audioSource.volume;
    }
}
