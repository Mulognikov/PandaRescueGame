using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

[RequireComponent(typeof(CanvasGroup))]
public class GameplayUI : MonoBehaviour
{
    public event Action MenuButtonClickEvent;
    public bool ShowMenu => _showMenu;

    [SerializeField] private Button _menuButton;
    
    private UISettings _uiSettings;
    private CanvasGroup _canvasGroup;

    private bool _showMenu = false;
    
    [Inject]
    private void Construct(UISettings uiSettings)
    {
        _uiSettings = uiSettings;
    }

    private void Awake()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
        _canvasGroup.alpha = 0;
        _menuButton.onClick.AddListener(() =>
        {
            _showMenu = true;
            MenuButtonClickEvent?.Invoke();
        });
    }

    public void Show()
    {
        _canvasGroup.DOFade(1f, _uiSettings.UiFadeTime);
        _showMenu = false;
    }

    public void Hide()
    {
        _canvasGroup.DOFade(0f, _uiSettings.UiFadeTime);
    }
}
