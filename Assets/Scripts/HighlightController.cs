using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;
using Random = UnityEngine.Random;

public class HighlightController
{
    private IControllerGameModel _gameModel;
    private TargetModel _currentlyHighlightedTarget;
    
    private IDisposable _timerListener;
    //transfer to coroutines
    //private Timer highlightTimer;
    
    public HighlightController()
    {
        _gameModel = DependencyContainer.Get<IControllerGameModel>(); 
        DependencyContainer.Get<IScoreCounterModel>().CurrentGameState
            .Subscribe(gameState =>
        {
            if(gameState == GameState.Playing)
            {
                StartHighlightingTimer();
            }
            else
            {
                StopHighlightingTimer();
            }
        });
        DependencyContainer.Get<IGameCore>().RestartInitiated
            .Subscribe(_ =>
            {
                _currentlyHighlightedTarget = null;
                StopHighlightingTimer();
                StartHighlightingTimer();
            });
    }

    public void StartHighlighting()
    {
        StartHighlightingTimer();
        _gameModel.TargetDestroyed
            .Where(targetId => targetId == _currentlyHighlightedTarget.Id)
            .Subscribe(targetId =>
            {
                StopHighlightingTimer();
                StartHighlightingTimer();
            });
    }

    private void StartHighlightingTimer()
    {
        //_currentHighlightCoroutine = _coroutineManager.StartCoroutine(HighlightRandomTargetCoroutine());
        StopHighlightingTimer();
        HighlightRandomTarget();
        _timerListener = Observable.Interval(TimeSpan.FromSeconds(Config.TARGET_HIGHLIGHT_DURATION))
            .Subscribe(_ =>
        {
            HighlightRandomTarget();
        });
    }
    
    private void StopHighlightingTimer()
    {
        _currentlyHighlightedTarget?.StopHighlighting();
        _timerListener?.Dispose();
    }
    
    private void HighlightRandomTarget()
    {
        List<TargetModel> notDestroyedTargets = _gameModel.GetAllNotDestroyedTargets();
        if (_currentlyHighlightedTarget != null)
        {
            notDestroyedTargets = notDestroyedTargets
                .Where(target => !_gameModel.CheckIfNeighbours(target.Id, _currentlyHighlightedTarget.Id))
                .ToList();
        }

        if (notDestroyedTargets.Count == 0)
        {
            Debug.Log("Game over");
            return;
        }
        
        _currentlyHighlightedTarget?.StopHighlighting();
        int newHighlightedTargetPosition = Random.Range(0, notDestroyedTargets.Count() - 1);
        _currentlyHighlightedTarget = notDestroyedTargets[newHighlightedTargetPosition];
        _currentlyHighlightedTarget.Highlight();
        
        /*if (_currentlyHighlightedTarget.IsDestroyed.Value)
        {
            Debug.LogError($"Target #{newHighlightedTargetPosition} is destroyed, but highlighted");
        }*/
    }
    
}
