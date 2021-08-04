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

    // Start is called before the first frame update
    void Start()
    {
        _lineRenderer = GetComponent<LineRenderer>();
        DependencyContainer.Get<AssistiveLineModel>().LinePointsUpdated
            .TakeUntilDestroy(this)
            .Subscribe(pointIds =>
            {
                _lineRenderer.positionCount = pointIds.Count;
                _lineRenderer
                    .SetPositions(pointIds.Select(id => _targetSpawner.getTargetPositionById(id)).ToArray());

            });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
