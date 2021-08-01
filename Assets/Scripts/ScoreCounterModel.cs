using UniRx;

public class ScoreCounterModel
{
    private ReactiveProperty<int> _score = new ReactiveProperty<int>(0);
    public IReadOnlyReactiveProperty<int> Score => _score;
    
    public ScoreCounterModel()
    {
        DependencyContainer.Get<IGameModel>().TargetDestroyed += (sender, args) =>
        {
            _score.Value++;
        };
    }
}