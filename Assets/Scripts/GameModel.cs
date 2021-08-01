using System;
using System.Collections.Generic;
using System.Linq;

public class GameModel:IGameModel, IControllerGameModel
{
    private List<List<TargetModel>> _targets = new List<List<TargetModel>>();
    
    public List<List<TargetModel>> Targets => _targets;

    public event EventHandler TargetDestroyed;
    
    public GameModel()
    {
        CreateTargets();
    }

    public void DestroyTarget(int targetId)
    {
        var foundTarget = _targets.SelectMany(x => x).Where(target => target.Id == targetId).FirstOrDefault();
        foundTarget?.Destroy();
        TargetDestroyed?.Invoke(this,null);
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
}
