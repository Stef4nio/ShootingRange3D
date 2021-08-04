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
    /// <summary>
    /// Gives you a TargetModel by its id
    /// </summary>
    /// <param name="targetId">Id of a TargetModel needed</param>
    /// <returns>TargetModel with id = targetId </returns>
    public TargetModel GetTargetModelById(int targetId)
    {
        return _targets
            .SelectMany(x => x)
            .FirstOrDefault(target => target.Id == targetId);
    }

    /// <summary>
    /// Get the total amount of targets
    /// </summary>
    /// <returns>Total amount of targets</returns>
    public int GetTotalTargetsAmount()
    {
        return _targets
            .SelectMany(x => x)
            .Count();
    }

    public IObservable<int> TargetDestroyed => _targetDestroyed;

    //Creates all target models and subscribes to the restart event
    public GameModel()
    {
        CreateTargets();
        DependencyContainer.Get<IGameCore>().RestartInitiated
            .Subscribe(_ =>
            {
                ResetAllTargets();
            });
    }

    /// <summary>
    /// Resets all targets to their initial state
    /// </summary>
    private void ResetAllTargets()
    {
        _targets
            .SelectMany(x => x)
            .ToList()
            .ForEach(target => target.Reset());
    }
    
    /// <summary>
    /// Destroys the desired target by its id
    /// </summary>
    /// <param name="targetId">Id of the target to be destroyed</param>
    public void DestroyTarget(int targetId)
    {
        var foundTarget = _targets
            .SelectMany(x => x).FirstOrDefault(target => target.Id == targetId);
        foundTarget?.Destroy();
        _targetDestroyed?.OnNext(targetId);
    }

    /// <summary>
    /// Get amount of targets that are still on the scene
    /// </summary>
    /// <returns>Amount of not destroyed targets</returns>
    public int GetTargetsLeft()
    {
        return GetAllNotDestroyedTargets().Count;
    }

    /// <summary>
    /// Gives you a collection of targets that are still on the scene
    /// </summary>
    /// <returns>List of targets that aren't destroyed</returns>
    public List<TargetModel> GetAllNotDestroyedTargets()
    {
        return _targets
            .SelectMany(target => target)
            .Where(target => !target.IsDestroyed.Value)
            .ToList();
    }

    /// <summary>
    /// Creates all the targets, and fills the Targets list with them
    /// </summary>
    private void CreateTargets()
    {
        for (int i = 0; i < Config.TARGETS_AMOUNT / 2; i++)
        {
            _targets.Add(new List<TargetModel>());
            for (int j = 0; j < Config.TARGETS_AMOUNT; j++)
            {
                _targets[i].Add(TargetModelFactory.CreateTargetModel()); 
            }
        }
    }

    /// <summary>
    /// Checks if given targets are neighbors to each other
    /// </summary>
    /// <param name="targetId1">Id of the first target to check</param>
    /// <param name="targetId2">Id of the second target to check</param>
    /// <returns>True if targets are near each other, false otherwise</returns>
    public bool CheckIfNeighbours(int targetId1, int targetId2)
    {
        int idDifference = Mathf.Abs(targetId1 - targetId2);
        return idDifference == 0 || idDifference == 1 || idDifference == Config.TARGETS_AMOUNT ||
               Math.Abs(idDifference - Config.TARGETS_AMOUNT) == 1;
    }

    /// <summary>
    /// Get target's position if Target list
    /// </summary>
    /// <param name="target">A TargetModel which position you want to know</param>
    /// <returns>A cortege of X and Y coords of a target in the Targets list</returns>
    public (int, int) GetTargetIndices(TargetModel target)
    {
        for (int i = 0; i < Targets.Count; i++)
        {
            for (int j = 0; j < Targets[i].Count; j++)
            {
                if (target == Targets[i][j])
                {
                    return (i, j);
                }
            }
        }

        return (-1, -1);
    }
}
