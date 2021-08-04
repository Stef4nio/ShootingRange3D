using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
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
        _gameModel.TargetDestroyed
            .Where(targetId => targetId == _currentlyHighlightedTarget.Id)
            .Subscribe(targetId =>
            {
                if (DependencyContainer.Get<IScoreCounterModel>().CurrentGameState.Value == GameState.Playing)
                {
                    StopHighlightingTimer();
                    StartHighlightingTimer();   
                }
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
        List<TargetModel> eligibleTargets = _gameModel.GetAllNotDestroyedTargets();
        if (_currentlyHighlightedTarget != null)
        {
            eligibleTargets = eligibleTargets
                .Where(target => !_gameModel.CheckIfNeighbours(target.Id, _currentlyHighlightedTarget.Id))
                .ToList();
        }

        if (eligibleTargets.Count <= 1)
        {
            Debug.Log("Not so many targets to pick from, adding new ones...");
            eligibleTargets = _gameModel.GetAllNotDestroyedTargets();
            return;
        }
        
        
        int newHighlightedTargetPosition = Random.Range(0, eligibleTargets.Count() - 1);
        if (_currentlyHighlightedTarget != null)
        {
            _currentlyHighlightedTarget.StopHighlighting();
            
            SetupAssistiveLine(_currentlyHighlightedTarget, 
                eligibleTargets[newHighlightedTargetPosition]);   
        }
        _currentlyHighlightedTarget = eligibleTargets[newHighlightedTargetPosition];
        _currentlyHighlightedTarget.Highlight();
    }

    private void SetupAssistiveLine(TargetModel previousTarget, TargetModel currentTarget)
    {
        (int, int) prevTargetPos = _gameModel.GetTargetIndices(previousTarget);
        (int, int) currTargetPos = _gameModel.GetTargetIndices(currentTarget);
        int deltaX = currTargetPos.Item2 - prevTargetPos.Item2;
        int deltaY = currTargetPos.Item1 - prevTargetPos.Item1;
        bool isUp = deltaY > 0;
        bool isRight = deltaX > 0;
        List<TargetModel> linePoints = new List<TargetModel>();
        if (isRight)
        {
            for (int j = prevTargetPos.Item2; j < prevTargetPos.Item2 + deltaX; j++)
            {
                linePoints.Add(_gameModel.Targets[prevTargetPos.Item1][j]);
            }
        }
        else
        {
            for (int j = prevTargetPos.Item2; j > prevTargetPos.Item2 + deltaX; j--)
            {
                linePoints.Add(_gameModel.Targets[prevTargetPos.Item1][j]);
            }
        }
        
        if (isUp)
        {
            for (int i = prevTargetPos.Item1; i <= prevTargetPos.Item1+deltaY; i++)
            {
                linePoints.Add(_gameModel.Targets[i][prevTargetPos.Item2+deltaX]);
            }
        }
        else
        {
            for (int i = prevTargetPos.Item1; i >= prevTargetPos.Item1+deltaY; i--)
            {
                linePoints.Add(_gameModel.Targets[i][prevTargetPos.Item2+deltaX]);
            }
        }
        DependencyContainer.Get<AssistiveLineModel>()
            .setLinePointsId(linePoints.Select(target => target.Id).ToArray());
    }
    
}
