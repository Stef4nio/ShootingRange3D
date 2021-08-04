using System;
using UniRx;

public interface IScoreCounterModel
{
    IReadOnlyReactiveProperty<int> Score { get; }
    IReadOnlyReactiveProperty<GameState> CurrentGameState { get; }
    TimeSpan GameDuration { get; }
    LoseCause LoseCause { get; }

}
