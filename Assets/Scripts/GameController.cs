using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController
{
    private IControllerGameModel _gameModel;

    public GameController()
    {
        _gameModel = DependencyContainer.Get<IControllerGameModel>();
    }

    /// <summary>
    /// Queries desired target to be destroyed, by changing it's IsDestroyed param to true
    /// </summary>
    /// <param name="targetId">Id of target wanted to be destroyed</param>
    public void DestroyTarget(int targetId)
    {
        _gameModel.DestroyTarget(targetId);
    }
}
