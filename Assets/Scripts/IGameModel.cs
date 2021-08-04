using System;
using System.Collections.Generic;
using UniRx;

public interface IGameModel
{
    List<List<TargetModel>> Targets { get; }
    IObservable<int> TargetDestroyed { get; }
    (int, int) GetTargetIndices(TargetModel target);
}