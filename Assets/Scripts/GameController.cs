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

    public void DestroyTarget(int targetId)
    {
        _gameModel.DestroyTarget(targetId);
    }
}
