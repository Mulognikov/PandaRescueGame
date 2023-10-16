using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Zenject;

[RequireComponent(typeof(CanvasGroup))]
public class WinPanel : MonoBehaviour
{
    [SerializeField] private Button _nextButton;
    [SerializeField] private Button _backToMenuButton;
    
    private LoadScene _loadScene;
    private GameState _gameState;
    private GameSettings _gameSettings;
    private CanvasGroup _canvasGroup;

    [Inject]
    private void Construct(LoadScene loadScene, GameState gameState, GameSettings gameSettings)
    {
        _loadScene = loadScene;
        _gameState = gameState;
        _gameSettings = gameSettings;
    }
    
    private void Awake()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
        _gameState.GameStateChangedEvent += OnGameStateChange;
        SetupButtons();
    }

    private void SetupButtons()
    {
        int nextLevel = SceneManager.GetActiveScene().buildIndex + 1;
        Debug.Log("button " + nextLevel);
        _nextButton.onClick.AddListener(() =>
        {
            Debug.Log("Invoke " + nextLevel);
            _loadScene.Load(nextLevel);
        });
        _backToMenuButton.onClick.AddListener(() => _loadScene.Load(0));
        
        if (nextLevel == SceneManager.sceneCountInBuildSettings)
        {
            _nextButton.gameObject.SetActive(false);
        }
    }

    private void OnGameStateChange()
    {
        if (_gameState.CurrentGameState == GameState.GameStateEnum.Win)
        {
            StartCoroutine(ShowPanel());
        }
    }

    private IEnumerator ShowPanel()
    {
        yield return new WaitForSeconds(_gameSettings.GameEndDuration);

        _canvasGroup.DOFade(1, 0.5f).SetEase(Ease.OutCubic);
        _canvasGroup.blocksRaycasts = true;
    }
    
}
