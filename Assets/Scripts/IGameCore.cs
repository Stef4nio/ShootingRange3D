using System;
using UniRx;

public interface IGameCore
{
    IObservable<Unit> RestartInitiated { get; }
    IObservable<Unit> GameLost { get; }
}
