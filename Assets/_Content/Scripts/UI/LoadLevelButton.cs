using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

[RequireComponent(typeof(Button))]
public class LoadLevelButton : MonoBehaviour
{
    [SerializeField] private TMP_Text text;
    private int _level;
    private LoadScene _loadScene;

    [Inject]
    private void Construct(LoadScene loadScene)
    {
        _loadScene = loadScene;
    }

    private void Awake()
    {
        GetComponent<Button>().onClick.AddListener(LoadLevel);
    }
    
    public void SetLevel(int level)
    {
        _level = level;
        text.text = "LeveL " + _level;
    }

    private void LoadLevel()
    {
        _loadScene.Load(_level);
    }
}
