using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Zenject;

public class DrawLeftSlider : MonoBehaviour
{
    [SerializeField] private RectTransform _twoStarsMarkers;
    [SerializeField] private RectTransform _threeStarsMarkers;
    [SerializeField] private RectTransform fillArea;
    
    private Slider _slider;
    private Line _line;
    private GameSettings _gameSettings;

    [Inject]
    public void Construct(Line line, GameSettings gameSettings)
    {
        _line = line;
        _gameSettings = gameSettings;
    }
    
    private void Awake()
    {
        _slider = GetComponent<Slider>();
        _slider.maxValue = _gameSettings.MaxLineLength;
        Vector2 pos3Stars = new (fillArea.rect.width * (_gameSettings.MaxLineLength - _gameSettings.ThreeStarsLength) / _gameSettings.MaxLineLength, 0);
        Vector2 pos2Stars = new (fillArea.rect.width * (_gameSettings.MaxLineLength - _gameSettings.TwoStarsLenght) / _gameSettings.MaxLineLength, 0);
        _threeStarsMarkers.anchoredPosition = pos3Stars;
        _twoStarsMarkers.anchoredPosition = pos2Stars;
    }

    private void Update()
    {
        _slider.value = _gameSettings.MaxLineLength - _line.CurrentLineLength;
    }
}
