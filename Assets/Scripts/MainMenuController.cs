using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    [SerializeField] private Button _startButton;
    [SerializeField] private Button _exitButton;
    
    private Subject<Unit> _startButtonPressed = new Subject<Unit>();

    public IObservable<Unit> StartButtonPressed => _startButtonPressed;
    // Start is called before the first frame update
    void Start()
    {
        _startButton.OnClickAsObservable()
            .TakeUntilDestroy(this)
            .Subscribe(_ =>
            {
                _startButtonPressed.OnNext(Unit.Default);
                gameObject.SetActive(false);
            });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
