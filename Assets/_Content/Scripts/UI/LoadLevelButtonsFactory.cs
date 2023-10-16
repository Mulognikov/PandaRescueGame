using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

public class LoadLevelButtonsFactory : MonoBehaviour
{
    private LoadLevelButton _loadLevelButton;
    private DiContainer _diContainer;

    [Inject]
    private void Construct(LoadLevelButton loadLevelButton, DiContainer diContainer)
    {
        _loadLevelButton = loadLevelButton;
        _diContainer = diContainer;
    }

    private void Awake()
    {
        Create();
    }

    private void Create()
    {
        for (int i = 1; i < SceneManager.sceneCountInBuildSettings; i++)
        {
            _diContainer.InstantiatePrefab(_loadLevelButton, transform).GetComponent<LoadLevelButton>().SetLevel(i);
        }
    }
}
