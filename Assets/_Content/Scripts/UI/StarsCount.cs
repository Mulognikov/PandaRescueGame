using DG.Tweening;
using UnityEngine;
using Zenject;

public class StarsCount : MonoBehaviour
{
    [SerializeField] private GameObject _star1;
    [SerializeField] private GameObject _star2;
    [SerializeField] private GameObject _star3;

    private Line _line;
    private GameSettings _gameSettings;

    [Inject]
    private void Construct(Line line, GameSettings gameSettings)
    {
        _line = line;
        _gameSettings = gameSettings;
    }

    private void Update()
    {
        if (_line.CurrentLineLength > _gameSettings.ThreeStarsLength)   DisableStar(_star3);
        if (_line.CurrentLineLength > _gameSettings.TwoStarsLenght)     DisableStar(_star2);;
    }

    private void DisableStar(GameObject star)
    {
        star.transform.DOScale(0.5f, 0.15f).SetEase(Ease.InBack).OnComplete(() =>
        {
            star.SetActive(false);
        });
    }
}
