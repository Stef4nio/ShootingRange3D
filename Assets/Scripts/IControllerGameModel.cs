using System;
using System.Collections.Generic;
using UniRx;

public interface IControllerGameModel
{
    int GetTotalTargetsAmount();
    void DestroyTarget(int targetId);
    IObservable<int> TargetDestroyed { get; }

    TargetModel GetTargetModelById(int targetId);

    bool CheckIfNeighbours(int targetId1, int targetId2);

    List<TargetModel> GetAllNotDestroyedTargets();

}
