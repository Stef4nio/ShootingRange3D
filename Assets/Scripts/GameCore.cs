using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class GameCore : MonoBehaviour, IGameCore, IRestartableGameCore
{
    [SerializeField] private TargetSpawner _targetSpawner = null;
    [SerializeField] private MainMenuController _mainMenuController = null;

    private Subject<Unit> _restartInitiated = new Subject<Unit>();
    private Subject<Unit> _gameLost = new Subject<Unit>();

    public IObservable<Unit> RestartInitiated => _restartInitiated;
    public IObservable<Unit> GameLost => _gameLost;

    private void Awake()
    {
        DependencyContainer.Set(_mainMenuController);
        
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
        DependencyContainer.Set(new PlayersResultsManager());
        _targetSpawner.TargetsSpawned
            .Subscribe(_ =>
        {
            DependencyContainer.Get<HighlightController>().StartHighlighting();
        });

        IDisposable loseTimer = null;
        
        DependencyContainer.Get<IScoreCounterModel>().CurrentGameState
            .TakeUntilDestroy(this)
            .Subscribe(state =>
            {
                if(state == GameState.Playing)
                {
                    loseTimer = Observable
                        .Timer(TimeSpan.FromSeconds(Config.TARGETS_GOAL_AMOUNT * Config.TARGET_HIGHLIGHT_DURATION))
                        .Subscribe(_ => { _gameLost.OnNext(Unit.Default); });
                }
                else
                {
                    loseTimer?.Dispose();
                }
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
