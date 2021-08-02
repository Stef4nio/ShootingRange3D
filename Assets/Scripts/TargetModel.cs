using System;
using UniRx;

public class TargetModel:ITargetModel
{
    private int _id;
    public int Id => _id;

    private ReactiveProperty<bool> _isHighlighted = new ReactiveProperty<bool>(false);
    public ReactiveProperty<bool> IsHighlighted => _isHighlighted;

    private ReactiveProperty<bool> _isDestroyed = new ReactiveProperty<bool>(false);

    public IReadOnlyReactiveProperty<bool> IsDestroyed => _isDestroyed;

    public void Reset()
    {
        _isHighlighted.SetValueAndForceNotify(false);
        _isDestroyed.SetValueAndForceNotify(false);
    }
    
    public void Destroy()
    {
        _isDestroyed.Value = true;
    }

    public void Highlight()
    {
        _isHighlighted.SetValueAndForceNotify(true);
    }
    
    public void StopHighlighting()
    {
        _isHighlighted.SetValueAndForceNotify(false);
    }
    
    public override string ToString()
    {
        return $"id: {_id}, isDestroyed: {_isDestroyed}, isHighlighted: {_isHighlighted}";
    }
    
    public TargetModel(int id)
    {
        _id = id;
    }
}