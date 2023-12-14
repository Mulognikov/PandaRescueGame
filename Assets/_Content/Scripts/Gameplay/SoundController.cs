using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Serialization;
using Zenject;
using Random = UnityEngine.Random;

public class SoundController : MonoBehaviour
{
    [SerializeField] private AudioSource _survivorHit;
    [SerializeField] private AudioClip[] _survivorHitSounds;

    [Space]
    [SerializeField] private AudioSource _bomb;
    [SerializeField] private AudioClip[] _explosionSounds;

    [Space]
    [SerializeField] private AudioSource _beeHit;
    [SerializeField] private AudioClip[] _beeHitSounds;
    
    [Space]
    [SerializeField] private AudioSource _beeFly;

    [Space]
    [SerializeField] private AudioSource _saw;
    [SerializeField] private AudioClip _sawStart;
    [SerializeField] private AudioClip _sawLoop;
    [SerializeField] private AudioClip _sawStop;

    [Space]
    [SerializeField] private AudioSource _starWin;
    [SerializeField] private AudioClip[] _starSounds;

    [Space]
    [SerializeField] private AudioSource _lavaSplash;
    [SerializeField] private AudioClip[] _lavaSplashSounds;

    private GameStateModel _gameState;
    private AudioMixer _mixer;
    private UISettings _uiSettings;
    private GameplayUI _gameplayUI;

    private float _survivorHitSoundCooldown = 0.1f;
    private float _lastSurvivorHitSoundTime;

    private const int _maxBeeSounds = 5;
    private int _currentBeePlaying = 0;
    private AudioSource[] _beeFlyPool = new AudioSource[_maxBeeSounds];


    private const string _mixerSoundFXVolumeKey = "SoundFX";

    [Inject]
    private void Construct(GameStateModel gameState, AudioMixer mixer, UISettings uiSettings, GameplayUI gameplayUI)
    {
        _gameState = gameState;
        _mixer = mixer;
        _uiSettings = uiSettings;
        _gameplayUI = gameplayUI;
    }

    private void Awake()
    {
        for (int i = 0; i < _beeFlyPool.Length; i++)
        {
            _beeFlyPool[i] = Instantiate(_beeFly, _beeFly.transform);
        }
    }

    private void OnEnable()
    {
        _gameState.GameStateChangedEvent += OnGameStateChanged;
        Bee.BeeSpawnEvent += OnBeeSpawn;
        Bee.BeeDestroyEvent += OnBeeDestroy;
    }
    
    private void OnDisable()
    {
        _gameState.GameStateChangedEvent -= OnGameStateChanged;
        Bee.BeeSpawnEvent -= OnBeeSpawn;
        Bee.BeeDestroyEvent -= OnBeeDestroy;
    }

    private void Start()
    {
        _lastSurvivorHitSoundTime = Time.time;
    }

    public void SurvivorHitSound()
    {
        if (Time.time - _lastSurvivorHitSoundTime < _survivorHitSoundCooldown) return;

        _survivorHit.PlayOneShot(_survivorHitSounds[Random.Range(0, _survivorHitSounds.Length)]);
        _survivorHit.pitch = Random.Range(0.9f, 1.1f);
        _lastSurvivorHitSoundTime = Time.time;
    }
    
    public void BombSound()
    {
        _bomb.pitch = Random.Range(0.9f, 1.1f);
        _bomb.PlayOneShot(_explosionSounds[Random.Range(0, _explosionSounds.Length)]);
    }
    
    public void BeeHitSound()
    {
        _beeHit.pitch = Random.Range(0.75f, 1.25f);
        _beeHit.PlayOneShot(_beeHitSounds[Random.Range(0, _beeHitSounds.Length)]);
    }

    public void SawStart()
    {
        StartCoroutine(SawStartCoroutine());
    }
    
    
    public void SawStop()
    {
        _saw.clip = _sawStop;
        _saw.loop = false;
        _saw.Play();
    }

    public void LavaSplashSound()
    {
        _lavaSplash.pitch = Random.Range(0.9f, 1.1f);
        _lavaSplash.PlayOneShot(_lavaSplashSounds[Random.Range(0, _lavaSplashSounds.Length)]);
    }

    public void StarSound(int star)
    {
        _starWin.PlayOneShot(_starSounds[star]);
    }

    private void OnGameStateChanged()
    {
        if (_gameState.CurrentGameState == GameStateEnum.Draw)
        {
            StartCoroutine(UnmuteSound());
        }

        if (_gameState.CurrentGameState == GameStateEnum.Win)
        {
            StartCoroutine(MuteSound());
        }
        
        if (_gameState.CurrentGameState == GameStateEnum.Lose)
        {
            StartCoroutine(MuteSound());
        }
    }

    private IEnumerator MuteSound()
    {
        if (!_gameplayUI.ShowMenu)
        {
            yield return new WaitForSeconds(_uiSettings.WaitMusicEndGame);
        }
        
        _mixer.DOSetFloat(_mixerSoundFXVolumeKey, -80f, _uiSettings.MusicFadeTime).SetEase(Ease.InExpo);
    }

    private IEnumerator UnmuteSound()                                       
    {
        yield return new WaitForSeconds(_uiSettings.WaitMusicStartGame);
        _mixer.DOSetFloat(_mixerSoundFXVolumeKey, 0f, _uiSettings.MusicFadeTime).SetEase(Ease.OutExpo);
    }

    private void OnBeeSpawn()
    {
        if (_currentBeePlaying >= _maxBeeSounds)
        {
            _currentBeePlaying++;
            return;
        }
        
        float clipLength = _beeFly.clip.length;
        _beeFlyPool[_currentBeePlaying].pitch = UnityEngine.Random.Range(0.9f, 1.1f);
        _beeFlyPool[_currentBeePlaying].panStereo = UnityEngine.Random.Range(-0.25f, 0.25f);
        _beeFlyPool[_currentBeePlaying].time = UnityEngine.Random.Range(0, clipLength);
        _beeFlyPool[_currentBeePlaying].Play();
        _currentBeePlaying++;
    }

    private void OnBeeDestroy()
    {
        if (_currentBeePlaying <= 0)
        {
            return;
        }
        
        if (_currentBeePlaying >= _maxBeeSounds)
        {
            _currentBeePlaying--;
            return;
        }
        
        _currentBeePlaying--;
        _beeFlyPool[_currentBeePlaying].Stop();
    }

    private IEnumerator SawStartCoroutine()
    {
        _saw.clip = _sawStart;
        _saw.loop = false;
        _saw.Play();
        
        yield return new WaitForSeconds(_sawStart.length);
        
        _saw.clip = _sawLoop;
        _saw.loop = true;
        _saw.Play();
    }
}
