using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Zenject;

[RequireComponent(typeof(CanvasGroup))]
public class SelectLevelUI : MonoBehaviour
{
    [SerializeField] private LevelButton _levelButton;
    [SerializeField] private GridLayoutGroup _buttonsParent;

    private UISettings _uiSettings;
    private LoadLevel _loadLevel;
    
    private CanvasGroup _canvasGroup;
    private Dictionary<int, LevelButton> _buttonLevelDictionary = new();

    [Inject]
    private void Construct(UISettings uiSettings, LoadLevel loadLevel)
    {
        _uiSettings = uiSettings;
        _loadLevel = loadLevel;
    }

    private void Awake()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
        Create();
        SetupGrid();
    }

    private void Start()
    {
        if (SceneManager.GetActiveScene().buildIndex == 0) Show();
    }

    private void Update()
    {
        UpdateButtons();
    }

    private void Create()
    {
        for (int i = 1; i < SceneManager.sceneCountInBuildSettings; i++)
        {
            int level = i;
            LevelButton button = Instantiate(_levelButton, _buttonsParent.transform);
            button.GetComponent<Button>().onClick.AddListener(() => LoadLevel(level));
            _buttonLevelDictionary.Add(level, button);
        }
    }

    private void UpdateButtons()
    {
        foreach (int level in _buttonLevelDictionary.Keys)
        {
            bool levelUnlocked = LevelUnlocker.GetLevelUnlockStatus(level);
            int levelStars = LevelUnlocker.GetLevelStars(level);

            if (levelUnlocked)  _buttonLevelDictionary[level].SetButtonActive(level, levelStars);
            else                _buttonLevelDictionary[level].SetButtonDisabled();
        }
    }

    public void Show()
    {
        gameObject.SetActive(true);
        UpdateButtons();
        _canvasGroup.DOFade(1f, _uiSettings.UiFadeTime).OnComplete(() =>
        {
            _canvasGroup.interactable = true;
        });
    }

    private void LoadLevel(int level)
    {
        _canvasGroup.interactable = false;
        _canvasGroup.DOFade(0f, _uiSettings.UiFadeTime).OnComplete(() =>
        {
            _loadLevel.Load(level);
            gameObject.SetActive(false);
        });
        
    }

    private void SetupGrid()
    {
        float ratio = Screen.height / (float)Screen.width;
        int rows = Mathf.FloorToInt(ratio * 2);
        _buttonsParent.constraintCount = rows;
    }
}
