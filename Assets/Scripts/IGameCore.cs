using System;
using UniRx;

public interface IGameCore
{
    IObservable<Unit> RestartInitiated { get; }
}
