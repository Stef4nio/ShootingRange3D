using System;
using UniRx;

public class TargetModel:ITargetModel
{
    private int _id;
    public int Id => _id;

    private ReactiveProperty<bool> _isDestroyed = new ReactiveProperty<bool>(false);

    public IReadOnlyReactiveProperty<bool> IsDestroyed => _isDestroyed;


    public void Destroy()
    {
        _isDestroyed.Value = true;
    }
    
    public TargetModel(int id)
    {
        _id = id;
    }
}