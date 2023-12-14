using System;
using System.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

[RequireComponent(typeof(CanvasGroup))]
public class WinPanel : MonoBehaviour
{
    [SerializeField] private Button _retryButton;
    [SerializeField] private Button _nextButton;
    [SerializeField] private Button _backToMenuButton;

    [Space]
    [SerializeField] private Image[] _stars;

    [Space]
    [SerializeField] private ParticleSystem _threeStarsParticles;
    [SerializeField] private Material _threeStarsParticlesMaterial;
    
    private LoadLevel _loadLevel;
    private UISettings _uiSettings;
    private SelectLevelUI _selectLevelUI;
    private GameScore _gameScore;
    private SoundController _soundController;
    
    private CanvasGroup _canvasGroup;

    [Inject]
    private void Construct(LoadLevel loadLevel, UISettings uiSettings, SelectLevelUI selectLevelUI, GameScore gameScore, SoundController soundController)
    {
        _loadLevel = loadLevel;
        _uiSettings = uiSettings;
        _selectLevelUI = selectLevelUI;
        _gameScore = gameScore;
        _soundController = soundController;
    }
    
    private void Awake()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
        _canvasGroup.alpha = 0;
        _canvasGroup.blocksRaycasts = false;
        SetupButtons();
    }

    private void SetupButtons()
    {
        _retryButton.onClick.AddListener(Retry);
        _nextButton.onClick.AddListener(LoadNext);
        _backToMenuButton.onClick.AddListener(ShowLevels);
        
        if (_loadLevel.IsLastLevel()) _nextButton.gameObject.SetActive(false);
    }

    public void Show()
    {
        for (int i = 0; i < _stars.Length; i++)
        {
            _stars[i].transform.localScale = Vector3.zero;
        }

		_threeStarsParticles.transform.parent = null;
        _threeStarsParticles.transform.position = Vector3.zero;
        _threeStarsParticles.transform.localScale = Vector3.one;
		_threeStarsParticlesMaterial.SetFloat("_Alpha", 1);
        _canvasGroup.blocksRaycasts = true;
        _canvasGroup.DOFade(1, _uiSettings.UiFadeTime).SetEase(Ease.OutCubic).OnComplete(ShowStars);
    }

    private async void ShowStars()
    {
        for (int i = 0; i < _gameScore.Stars; i++)
        {
            _stars[i].transform.DOScale(1f, 0.26f).SetEase(Ease.OutBack);

            if (i == 2)
            {
                _threeStarsParticles.Play(true);
            }
            
            await Task.Delay(65);
			_soundController.StarSound(i);
			await Task.Delay(195);
			
		}
    }

    private void LoadNext()
    {
        Hide(_loadLevel.LoadNext);
    }
    
    private void Retry()
    {
        Hide(_loadLevel.RetryLevel);
    }

    private void ShowLevels()
    {
        Hide(_selectLevelUI.Show);
    }

    private void Hide(Action callback)
    {
        float currentAlpha = 1;
        DOTween.To(() => currentAlpha, x => _threeStarsParticlesMaterial.SetFloat("_Alpha", x), 0, _uiSettings.UiFadeTime);

        _canvasGroup.DOFade(0, _uiSettings.UiFadeTime).SetEase(Ease.OutCubic).OnComplete(() =>
        {
            _canvasGroup.blocksRaycasts = false;
            callback?.Invoke();
        });
    }
}
