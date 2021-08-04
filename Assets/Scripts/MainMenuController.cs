using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    [SerializeField] private Button _startButton = null;
    [SerializeField] private Button _exitButton = null;
    [SerializeField] private InputField _playerNameInputField = null;
    
    private Subject<Unit> _startButtonPressed = new Subject<Unit>();

    public IObservable<Unit> StartButtonPressed => _startButtonPressed;
    // Subscribes to all the inner UI events
    void Start()
    {
        gameObject.SetActive(true);
        _exitButton.OnClickAsObservable()
            .TakeUntilDestroy(this)
            .Subscribe(_ =>
            {
                Application.Quit();
            });
        _playerNameInputField.OnValueChangedAsObservable()
            .TakeUntilDestroy(this)
            .Subscribe(value =>
            {
                _startButton.interactable = value != "";
            });
        _startButton.OnClickAsObservable()
            .TakeUntilDestroy(this)
            .Subscribe(_ =>
            {
                _startButtonPressed.OnNext(Unit.Default);
                gameObject.SetActive(false);
            });
    }

    /// <summary>
    /// Returns you player's name
    /// </summary>
    /// <returns>Player's name</returns>
    public string GetPlayerName()
    {
        return _playerNameInputField.text;
    }
    
    // Update is called once per frame
    void Update()
    {
        
    }
}
