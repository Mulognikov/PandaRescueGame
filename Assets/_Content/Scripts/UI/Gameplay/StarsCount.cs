using DG.Tweening;
using UnityEngine;
using Zenject;


public class StarsCount : MonoBehaviour
{
    [SerializeField] private GameObject _star1;
    [SerializeField] private GameObject _star2;
    [SerializeField] private GameObject _star3;

    private GameScore _gameScore;

    [Inject]
    private void Construct(GameScore gameScore)
    {
        _gameScore = gameScore;
    }

    private void Update()
    {
        if (_star3.activeInHierarchy && _gameScore.Stars < 3) DisableStar(_star3);
        if (_star2.activeInHierarchy && _gameScore.Stars < 2) DisableStar(_star2);;
        if (_star1.activeInHierarchy && _gameScore.Stars < 1) DisableStar(_star1);;
    }

    private void DisableStar(GameObject star)
    {
        star.transform.DOScale(0.5f, 0.15f).SetEase(Ease.InBack).OnComplete(() =>
        {
            star.SetActive(false);
        });
    }
}
