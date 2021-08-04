using System;
using UniRx;
using UnityEngine;

public class ScoreCounterModel: IRestartableScoreCounterModel, IScoreCounterModel
{
    private ReactiveProperty<int> _score = new ReactiveProperty<int>(0);
    public IReadOnlyReactiveProperty<int> Score => _score;
    
    private ReactiveProperty<GameState> _currentGameState = new ReactiveProperty<GameState>(GameState.PreGame);
    public IReadOnlyReactiveProperty<GameState> CurrentGameState => _currentGameState;
    
    private TimeSpan _gameDuration = TimeSpan.Zero;
    public TimeSpan GameDuration => _gameDuration;

    private IRestartableGameCore _gameCore;

    private DateTime _gameStartTime;

    private LoseCause _loseCause;

    public LoseCause LoseCause => _loseCause;

    public ScoreCounterModel()
    {
        IControllerGameModel gameModel = DependencyContainer.Get<IControllerGameModel>();
        _gameCore = DependencyContainer.Get<IRestartableGameCore>();
        DependencyContainer.Get<MainMenuController>().StartButtonPressed.Subscribe(_ =>
        {
            StartGame();
        });
        DependencyContainer.Get<IGameModel>().TargetDestroyed.Subscribe(targetId =>
        {
            if (gameModel.GetTargetModelById(targetId).IsHighlighted.Value)
            {
                _score.Value++;
            }
            if (_score.Value == Config.TARGETS_GOAL_AMOUNT)
            {
                FinishGame(true);
            }

            if (gameModel.GetTargetsLeft() == 0 || gameModel.GetTargetsLeft() < Config.TARGETS_GOAL_AMOUNT - _score.Value)
            {
                LoseGame(LoseCause.NotEnoughTargetsLeft);
            }
        });
        DependencyContainer.Get<IGameCore>().RestartInitiated
            .Subscribe(_ =>
            {
                _score.Value = 0;
                StartGame();
            });
        DependencyContainer.Get<IGameCore>().GameLost
            .Subscribe(_ =>
            {
                LoseGame(LoseCause.OutOfTime);
            });
    }

    /// <summary>
    /// Finishes the game with specified outcome
    /// </summary>
    /// <param name="isWon">Has player won or lost</param>
    private void FinishGame(bool isWon)
    {
        _gameDuration = DateTime.Now - _gameStartTime;
        _currentGameState.Value = isWon ? GameState.GameWon : GameState.GameLost;
    }

    /// <summary>
    /// Lose the game and specify the cause of that
    /// </summary>
    /// <param name="cause">Reason of player losing the game</param>
    private void LoseGame(LoseCause cause)
    {
        _loseCause = cause;
        FinishGame(false);
    }
    
    /// <summary>
    /// Starts the game, and player's timer
    /// </summary>
    private void StartGame()
    {
        _gameStartTime = DateTime.Now;
        _currentGameState.Value = GameState.Playing;
    }
    
    /// <summary>
    /// Notifies gameCore about the need of restarting
    /// </summary>
    public void InitiateRestart()
    {
        _gameCore.InitiateRestart();
    }
}