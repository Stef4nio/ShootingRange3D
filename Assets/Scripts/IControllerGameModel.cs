using System;
using System.Collections.Generic;
using UniRx;

public interface IControllerGameModel: IGameModel
{
    int GetTotalTargetsAmount();
    void DestroyTarget(int targetId);

    TargetModel GetTargetModelById(int targetId);

    bool CheckIfNeighbours(int targetId1, int targetId2);

    List<TargetModel> GetAllNotDestroyedTargets();

    int GetTargetsLeft();

}
