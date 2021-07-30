using System;

public class GameModel:IGameModel
{
    TargetModel[,] _targets = new TargetModel[Config.TARGETS_AMOUNT / 2, Config.TARGETS_AMOUNT];

    public TargetModel[,] Targets => _targets;

    public event EventHandler _targetsChanged;

    public GameModel()
    {
        CreateTargets();
    }

    public void CreateTargets()
    {
        for (int i = 0; i < _targets.GetLength(0); i++)
        {
            for (int j = 0; j < _targets.GetLength(1); j++)
            {
                _targets[i, j] = TargetModelFactory.createTargetModel(); 
            }
        }
    }
}
