using System.Collections;
using UnityEngine;
using Zenject;

public class UIController : MonoBehaviour
{
    private GameStateModel _gameState;
    private GameplayUI _gameplayUI;
    private SelectLevelUI _selectLevelUI;
    private WinPanel _winPanel;
    private LosePanel _losePanel;
    private CameraAndBackgroundMove _backgroundMove;
    private UISettings _uiSettings;

    [Inject]
    public void Construct(GameStateModel gameState, GameplayUI gameplayUI, SelectLevelUI selectLevelUI, 
        WinPanel winPanel, LosePanel losePanel, CameraAndBackgroundMove backgroundMove, UISettings uiSettings)
    {
        _gameState = gameState;
        _gameplayUI = gameplayUI;
        _selectLevelUI = selectLevelUI;
        _winPanel = winPanel;
        _losePanel = losePanel;
        _backgroundMove = backgroundMove;
        _uiSettings = uiSettings;
    }


    public void OnEnable()
    {
        _gameState.GameStateChangedEvent += OnGameStateChanged;
    }

    public void OnDisable()
    {
        _gameState.GameStateChangedEvent -= OnGameStateChanged;
    }

    private void OnGameStateChanged()
    {
        if (_gameState.CurrentGameState == GameStateEnum.Draw)
        {
            StartCoroutine(OnDrawState());
        }
        
        if (_gameState.CurrentGameState == GameStateEnum.Win)
        {
            StartCoroutine(OnWinState());
        }

        if (_gameState.CurrentGameState == GameStateEnum.Lose)
        {
            StartCoroutine(OnLoseState());
        }
    }

    private IEnumerator OnDrawState()
    {
        _selectLevelUI.gameObject.SetActive(false); // needed for testing in editor
        _backgroundMove.Normal();
        yield return new WaitForSeconds(_uiSettings.WaitUiAfterGameStartTime);
        _gameplayUI.Show();
    }

    private IEnumerator OnWinState()
    {
        yield return new WaitForSeconds(_uiSettings.WaitCameraAfterGameEndTime);
        _backgroundMove.Fly();
        _gameplayUI.Hide();
        yield return new WaitForSeconds(_uiSettings.WaitUiAfterGameEndTime);
        _winPanel.Show();
    }
    
    private IEnumerator OnLoseState()
    {
        yield return null;
        
        if (!_gameplayUI.ShowMenu)
        {
            yield return new WaitForSeconds(_uiSettings.WaitCameraAfterGameEndTime);
        }
        
        _backgroundMove.Fly();
        _gameplayUI.Hide();
        yield return new WaitForSeconds(_uiSettings.WaitUiAfterGameEndTime);
        
        if (_gameplayUI.ShowMenu)
        {
            _selectLevelUI.Show();
        }
        else
        {
            _losePanel.Show();
        }
    }
}
