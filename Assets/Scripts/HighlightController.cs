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

    //Subscribes to events that trigger highlighting the targets
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
    

    /// <summary>
    /// Starts highlighting timer, so that targets can become highlighted for a desired amount of time
    /// </summary>
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
    
    /// <summary>
    /// Stops all the highlighting
    /// </summary>
    private void StopHighlightingTimer()
    {
        _currentlyHighlightedTarget?.StopHighlighting();
        _timerListener?.Dispose();
    }
    
    /// <summary>
    /// Picks a random target from available(not destroyed and not neighboring) and highlights it
    /// </summary>
    private void HighlightRandomTarget()
    {
        List<TargetModel> eligibleTargets = _gameModel.GetAllNotDestroyedTargets();
        //If this is the first target to highlight, than there are no neighbours of it
        //If not then we should filter them out
        if (_currentlyHighlightedTarget != null)
        {
            eligibleTargets = eligibleTargets
                .Where(target => !_gameModel.CheckIfNeighbours(target.Id, _currentlyHighlightedTarget.Id))
                .ToList();
        }

        //If all the targets left are destroyed or are our neighbours, then we don't have much of a choice
        //and we pick from what's left
        if (eligibleTargets.Count <= 1)
        {
            Debug.Log("Not so many targets to pick from, adding new ones...");
            eligibleTargets = _gameModel.GetAllNotDestroyedTargets();
            return;
        }
        
        
        int newHighlightedTargetPosition = Random.Range(0, eligibleTargets.Count() - 1);
        //If there was a previously highlighted target, then we want to "deHighlight" it,
        //And start drawing an assistive line
        if (_currentlyHighlightedTarget != null)
        {
            _currentlyHighlightedTarget.StopHighlighting();
            
            SetupAssistiveLine(_currentlyHighlightedTarget, 
                eligibleTargets[newHighlightedTargetPosition]);   
        }
        _currentlyHighlightedTarget = eligibleTargets[newHighlightedTargetPosition];
        _currentlyHighlightedTarget.Highlight();
    }

    /// <summary>
    /// Sets up an assistive line, find the needed targets, to connect, and saves them in the corresponding model
    /// </summary>
    /// <param name="previousTarget">Previously highlighted target, from which we start our line</param>
    /// <param name="currentTarget">Currently highlighted target, where we finish our line</param>
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
