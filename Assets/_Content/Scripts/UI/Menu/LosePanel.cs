using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

[RequireComponent(typeof(CanvasGroup))]
public class LosePanel : MonoBehaviour
{
    [SerializeField] private Button _retryButton;
    [SerializeField] private Button _backToMenuButton;
    
    private LoadLevel _loadLevel;
    private UISettings _uiSettings;
    private SelectLevelUI _selectLevelUI;
    private CanvasGroup _canvasGroup;

    [Inject]
    private void Construct(LoadLevel loadLevel, UISettings uiSettings, SelectLevelUI selectLevelUI)
    {
        _loadLevel = loadLevel;
        _uiSettings = uiSettings;
        _selectLevelUI = selectLevelUI;
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
        _backToMenuButton.onClick.AddListener(ShowLevels);
    }

    public void Show()
    {
        _canvasGroup.blocksRaycasts = true;
        _canvasGroup.DOFade(1, _uiSettings.UiFadeTime).SetEase(Ease.OutCubic);
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
        _canvasGroup.DOFade(0, _uiSettings.UiFadeTime).SetEase(Ease.OutCubic).OnComplete(() =>
        {
            _canvasGroup.blocksRaycasts = false;
            callback?.Invoke();
        });
    }
}
