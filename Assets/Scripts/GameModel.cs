using System;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;

public class GameModel:IGameModel, IControllerGameModel
{
    private List<List<TargetModel>> _targets = new List<List<TargetModel>>();

    public List<List<TargetModel>> Targets => _targets;

    private Subject<int> _targetDestroyed = new Subject<int>();
    public TargetModel GetTargetModelById(int targetId)
    {
        return _targets
            .SelectMany(x => x)
            .Where(target => target.Id == targetId)
            .FirstOrDefault();
    }

    public int GetTotalTargetsAmount()
    {
        return _targets
            .SelectMany(x => x)
            .Count();
    }

    public IObservable<int> TargetDestroyed => _targetDestroyed;

    public GameModel()
    {
        CreateTargets();
        DependencyContainer.Get<IGameCore>().RestartInitiated
            .Subscribe(_ =>
            {
                ResetAllTargets();
            });
    }

    private void ResetAllTargets()
    {
        _targets
            .SelectMany(x => x)
            .ToList()
            .ForEach(target => target.Reset());
    }
    
    public void DestroyTarget(int targetId)
    {
        var foundTarget = _targets
            .SelectMany(x => x)
            .Where(target => target.Id == targetId).FirstOrDefault();
        foundTarget?.Destroy();
        _targetDestroyed?.OnNext(targetId);
    }

    public List<TargetModel> GetAllNotDestroyedTargets()
    {
        return _targets
            .SelectMany(target => target)
            .Where(target => !target.IsDestroyed.Value)
            .ToList();
    }

    private void CreateTargets()
    {
        for (int i = 0; i < Config.TARGETS_AMOUNT / 2; i++)
        {
            _targets.Add(new List<TargetModel>());
            for (int j = 0; j < Config.TARGETS_AMOUNT; j++)
            {
                _targets[i].Add(TargetModelFactory.createTargetModel()); 
            }
        }
    }

    public bool CheckIfNeighbours(int targetId1, int targetId2)
    {
        int idDifference = Mathf.Abs(targetId1 - targetId2);
        return idDifference == 0 || idDifference == 1 || idDifference == Config.TARGETS_AMOUNT ||
               Math.Abs(idDifference - Config.TARGETS_AMOUNT) == 1;
    }
}
