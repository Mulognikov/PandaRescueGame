using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using Zenject;

public class AudioSettings : MonoBehaviour
{
    [SerializeField] private Button _musicButton;
    [SerializeField] private Button _audioButton;

    [SerializeField] private Image _musicOnImage;
    [SerializeField] private Image _musicOffImage;
    [SerializeField] private Image _audioOnImage;
    [SerializeField] private Image _audioOffImage;

    private AudioMixer _mixer;

    private bool _musicState = true;
    private bool _audioState = true;

    private const string _musicKeyPlayerPrefs = "MusicKey";
    private const string _audioKeyPlayerPrefs = "AudioKey";
    private const string _musicKeyMixer = "Music";
    private const string _audioKeyMixer = "SoundGlobal";

    [Inject]
    private void Construct(AudioMixer mixer)
    {
        _mixer = mixer;
    }
    
    private void Start()
    {
        _musicState = System.Convert.ToBoolean(PlayerPrefs.GetInt(_musicKeyPlayerPrefs, 1));
        _audioState = System.Convert.ToBoolean(PlayerPrefs.GetInt(_audioKeyPlayerPrefs, 1));

        _musicButton.onClick.AddListener(SwitchMusic);
        _audioButton.onClick.AddListener(SwitchAudio);
        
        UpdateMusicAudio();
    }

    private void SwitchMusic()
    {
        _musicState = !_musicState;
        PlayerPrefs.SetInt(_musicKeyPlayerPrefs, System.Convert.ToInt32(_musicState));
        UpdateMusicAudio();
    }
    
    private void SwitchAudio()
    {
        _audioState = !_audioState;
        PlayerPrefs.SetInt(_audioKeyPlayerPrefs, System.Convert.ToInt32(_audioState));
        UpdateMusicAudio();
    }

    private void UpdateMusicAudio()
    {
        _mixer.SetFloat(_musicKeyMixer, System.Convert.ToInt32(!_musicState) * -80f);
        _mixer.SetFloat(_audioKeyMixer, System.Convert.ToInt32(!_audioState) * -80f);
        
        _musicOnImage.enabled = _musicState;
        _musicOffImage.enabled = !_musicState;
        
        _audioOnImage.enabled = _audioState;
        _audioOffImage.enabled = !_audioState;
    }
}
