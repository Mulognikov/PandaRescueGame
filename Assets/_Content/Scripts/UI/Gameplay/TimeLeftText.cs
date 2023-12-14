using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class TimeLeftText : MonoBehaviour
{
    [SerializeField] private TMP_Text _timeText;
    [SerializeField] private Image _fillImage;
    
    private GameStateModel _gameState;
    private GameSettings _gameSettings;

    [Inject]
    public void Construct(GameStateModel gameState, GameSettings gameSettings)
    {
        _gameState = gameState;
        _gameSettings = gameSettings;
    }

    private void Update()
    {
        _timeText.text = ((int)Mathf.Ceil(_gameState.TimeLeft)).ToString();
        _fillImage.fillAmount = _gameState.TimeLeft / _gameSettings.GameDuration;
    }
}
