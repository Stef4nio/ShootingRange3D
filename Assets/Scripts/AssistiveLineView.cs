using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class AssistiveLineView : MonoBehaviour
{
    [SerializeField] private TargetSpawner _targetSpawner = null;
    private LineRenderer _lineRenderer;
    
    //Subscribes to event that points are updated, and draws them when needed
    void Start()
    {
        _lineRenderer = GetComponent<LineRenderer>();
        DependencyContainer.Get<AssistiveLineModel>().LinePointsUpdated
            .TakeUntilDestroy(this)
            .Subscribe(pointIds =>
            {
                _lineRenderer.positionCount = pointIds.Count;
                _lineRenderer
                    .SetPositions(pointIds.Select(id => _targetSpawner.GetTargetPositionById(id)).ToArray());

            });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
