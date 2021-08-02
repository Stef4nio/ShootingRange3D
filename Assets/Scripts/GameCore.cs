using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class GameCore : MonoBehaviour
{
    [SerializeField] private TargetSpawner _targetSpawner;
    [SerializeField] private CoroutineManager _coroutineManager;
    private void Awake()
    {
        GameModel model = new GameModel();
        DependencyContainer.Set((IGameModel) model);
        DependencyContainer.Set((IControllerGameModel)model);
        DependencyContainer.Set(new GameController());
        DependencyContainer.Set(new ScoreCounterModel());
        DependencyContainer.Set(_coroutineManager);
        DependencyContainer.Set(new HighlightController());
        _targetSpawner.TargetsSpawned.Subscribe(_ =>
        {
            DependencyContainer.Get<HighlightController>().StartHighlighting();
        });
        //Destroy(gameObject);
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
