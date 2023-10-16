using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class DrawLeftSlider : MonoBehaviour
{
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
        _slider.maxValue = _gameSettings.MaxDrawPoints;
    }

    private void Update()
    {
        _slider.value = _line.DrawLeftPoints;
    }
}
