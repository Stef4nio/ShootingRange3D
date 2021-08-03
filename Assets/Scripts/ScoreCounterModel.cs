using UniRx;
using UnityEngine;

public class ScoreCounterModel: IRestartableScoreCounterModel, IScoreCounterModel
{
    private ReactiveProperty<int> _score = new ReactiveProperty<int>(0);
    public IReadOnlyReactiveProperty<int> Score => _score;
    
    private ReactiveProperty<GameState> _currentGameState = new ReactiveProperty<GameState>(GameState.Playing);
    public IReadOnlyReactiveProperty<GameState> CurrentGameState => _currentGameState;

    private IRestartableGameCore _gameCore;

    public ScoreCounterModel()
    {
        IControllerGameModel gameModel = DependencyContainer.Get<IControllerGameModel>();
        _gameCore = DependencyContainer.Get<IRestartableGameCore>();
        DependencyContainer.Get<IGameModel>().TargetDestroyed.Subscribe(targetId =>
        {
            if (gameModel.GetTargetModelById(targetId).IsHighlighted.Value)
            {
                _score.Value++;
            }
            if (_score.Value == Config.TARGETS_GOAL_AMOUNT)
            {
                _currentGameState.Value = GameState.GameWon;
            }
        });
        DependencyContainer.Get<IGameCore>().RestartInitiated
            .Subscribe(_ =>
            {
                _score.Value = 0;
                _currentGameState.Value = GameState.Playing;
            });
        DependencyContainer.Get<IGameCore>().GameLost
            .Subscribe(_ =>
            {
                _currentGameState.Value = GameState.GameLost;
            });
    }

    public void InitiateRestart()
    {
        _gameCore.InitiateRestart();
    }
}