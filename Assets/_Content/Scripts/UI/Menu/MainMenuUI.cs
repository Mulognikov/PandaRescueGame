using System;
using System.Collections;
using UnityEngine;
using Zenject;

public class MainMenuUI : MonoBehaviour
{
    private SelectLevelUI _selectLevelUI;
    private LoadLevel _loadLevel;
    private UISettings _uiSettings;

    [Inject]
    public void Construct(SelectLevelUI selectLevelUI, LoadLevel loadLevel, UISettings uiSettings)
    {
        _selectLevelUI = selectLevelUI;
        _loadLevel = loadLevel;
        _uiSettings = uiSettings;
    }
    
    public void Awake()
    {
        _selectLevelUI.Show();
    }

    private void OnEnable()
    {
        _loadLevel.RequestedLoadLevelEvent += OnLevelLoadRequest;
    }

    private void OnDisable()
    {
        _loadLevel.RequestedLoadLevelEvent -= OnLevelLoadRequest;
    }

    private void OnLevelLoadRequest()
    {
        //_selectLevelUI.Hide();
        StartCoroutine(WaitForHide());
    }

    private IEnumerator WaitForHide()
    {
        yield return new WaitForSeconds(_uiSettings.UiFadeTime);
        _loadLevel.LoadRequested();
    }
}
