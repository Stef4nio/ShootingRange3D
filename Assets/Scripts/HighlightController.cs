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

    private CoroutineManager _coroutineManager;

    private Coroutine _currentHighlightCoroutine;
    //transfer to coroutines
    //private Timer highlightTimer;
    
    public HighlightController()
    {
        _gameModel = DependencyContainer.Get<IControllerGameModel>();
        _coroutineManager = DependencyContainer.Get<CoroutineManager>();
    }

    //Call this function from gameCore
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
        _currentHighlightCoroutine = _coroutineManager.StartCoroutine(HighlightRandomTargetCoroutine());
    }
    
    private void StopHighlightingTimer()
    {
        _coroutineManager.StopCoroutine(_currentHighlightCoroutine);
    }
    
    private void HighlightRandomTarget()
    {
        List<TargetModel> notDestroyedTargets = _gameModel.GetAllNotDestroyedTargets();
        TargetModel highlightCandidate;
        
        _currentlyHighlightedTarget?.StopHighlighting();
        do
        {
            int newHighlightedTargetPosition = Random.Range(0, notDestroyedTargets.Count() - 1);
            highlightCandidate = notDestroyedTargets[newHighlightedTargetPosition];
            if (_currentlyHighlightedTarget == null)
            {
                break;
            }
        } while (highlightCandidate.IsDestroyed.Value ||
                 _gameModel.CheckIfNeighbours(highlightCandidate.Id, _currentlyHighlightedTarget.Id));
        

        _currentlyHighlightedTarget = highlightCandidate;
        _currentlyHighlightedTarget.Highlight();
        
        /*if (_currentlyHighlightedTarget.IsDestroyed.Value)
        {
            Debug.LogError($"Target #{newHighlightedTargetPosition} is destroyed, but highlighted");
        }*/
    }
    
    private IEnumerator HighlightRandomTargetCoroutine()
    {
        while (true)
        {
            HighlightRandomTarget();
            yield return new WaitForSeconds(Config.TARGET_HIGHLIGHT_DURATION);
        }

        yield return null;
    }
}
