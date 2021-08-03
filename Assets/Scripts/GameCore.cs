using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class GameCore : MonoBehaviour, IGameCore, IRestartableGameCore
{
    [SerializeField] private TargetSpawner _targetSpawner = null;

    private Subject<Unit> _restartInitiated = new Subject<Unit>();
    private Subject<Unit> _gameLost = new Subject<Unit>();

    public IObservable<Unit> RestartInitiated => _restartInitiated;
    public IObservable<Unit> GameLost => _gameLost;

    private void Awake()
    {
        DependencyContainer.Set((IGameCore) this);
        DependencyContainer.Set((IRestartableGameCore) this);
        
        GameModel model = new GameModel();
        DependencyContainer.Set((IGameModel) model);
        DependencyContainer.Set((IControllerGameModel)model);
        DependencyContainer.Set(new GameController());
        ScoreCounterModel scoreCounterModel = new ScoreCounterModel();
        DependencyContainer.Set((IScoreCounterModel)scoreCounterModel);
        DependencyContainer.Set((IRestartableScoreCounterModel)scoreCounterModel);
        DependencyContainer.Set(new HighlightController());
        _targetSpawner.TargetsSpawned
            .Subscribe(_ =>
        {
            DependencyContainer.Get<HighlightController>().StartHighlighting();
        });
        IDisposable loseTimer = Observable
            .Timer(TimeSpan.FromSeconds(Config.TARGETS_GOAL_AMOUNT * Config.TARGET_HIGHLIGHT_DURATION))
            .Subscribe(_ =>
            {
                _gameLost.OnNext(Unit.Default);
            });
    }

    public void InitiateRestart()
    {
        _restartInitiated.OnNext(Unit.Default);
    }

    // Start is called before the first frame update

    void Start()
    {
        
    }

    // Update is called once per frame

    void Update()
    {
        
    }
}
