using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameCore : MonoBehaviour
{
    private void Awake()
    {
        GameModel model = new GameModel();
        DependencyContainer.Set((IGameModel) model);
        DependencyContainer.Set((IControllerGameModel)model);
        DependencyContainer.Set(new GameController());
        DependencyContainer.Set(new ScoreCounterModel());
        Destroy(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
