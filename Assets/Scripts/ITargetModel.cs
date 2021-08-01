using UniRx;

public interface ITargetModel
{
    int Id { get; }
    IReadOnlyReactiveProperty<bool> IsDestroyed { get; }

}
