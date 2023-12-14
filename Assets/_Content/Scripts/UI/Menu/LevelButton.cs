using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class LevelButton : MonoBehaviour
{
    [SerializeField] private TMP_Text _text;
    [SerializeField] private GameObject _lockImage;
    [SerializeField] private GameObject[] _disableIfLocked;
    [SerializeField] private Image[] _stars;

    private Button _button;

    private void Awake()
    {
        _button = GetComponent<Button>();
    }

    private void Start()
    {
        //Setup();
    }

    // private void Setup()
    // {
    //     Button button = GetComponent<Button>();
    //     bool levelUnlocked = LevelUnlocker.GetLevelUnlockStatus(_level);
    //     int levelStars = LevelUnlocker.GetLevelStars(_level);
    //     
    //     button.onClick.AddListener(LoadLevel);
    //
    //     if (levelUnlocked)
    //     {
    //         for (int i = 0; i < _stars.Length; i++)
    //         {
    //             _stars[i].enabled = i <= levelStars;
    //         }
    //     }
    //     else
    //     {
    //         button.interactable = false;
    //         _lockImage.enabled = true;
    //         foreach (var star in _stars)
    //         {
    //             star.enabled = false;
    //         }
    //     }
    // }

    public void SetButtonActive(int level, int stars)
    {
        _button.interactable = true;
        _lockImage.SetActive(false);
        _text.text = level.ToString();
        
        foreach (var go in _disableIfLocked)
        {
            go.SetActive(true);
        }

        for (int i = 0; i < _stars.Length; i++)
        {
            _stars[i].enabled = i < stars;
        }
    }

    public void SetButtonDisabled()
    {
        _button.interactable = false;
        _lockImage.SetActive(true);
        foreach (var go in _disableIfLocked)
        {
            go.SetActive(false);
        }
    }
}
