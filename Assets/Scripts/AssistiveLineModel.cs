using System;
using System.Collections.Generic;
using UniRx;

public class AssistiveLineModel
{
    private List<int> _assistiveLinePointsIds = new List<int>();

    private Subject<List<int>> _linePointsUpdated = new Subject<List<int>>();

    public IObservable<List<int>> LinePointsUpdated => _linePointsUpdated;

    public void setLinePointsId(int[] pointsIds)
    {
        _assistiveLinePointsIds.Clear();
        foreach (var id in pointsIds)
        {
            _assistiveLinePointsIds.Add(id);
        }
        _linePointsUpdated.OnNext(_assistiveLinePointsIds);
    }
}
