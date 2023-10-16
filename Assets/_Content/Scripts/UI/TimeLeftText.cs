using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Zenject;

[RequireComponent(typeof(TMP_Text))]
public class TimeLeftText : MonoBehaviour
{
    private GameState _gameState;
    private TMP_Text _timeText;

    [Inject]
    public void Construct(GameState gameState)
    {
        _gameState = gameState;
    }
    
    private void Awake()
    {
        _timeText = GetComponent<TMP_Text>();
    }

    private void Update()
    {
        _timeText.text = ((int)Mathf.Ceil(_gameState.TimeLeft)).ToString();
    }
}
