public class TargetModel:ITargetModel
{
    private int _id;
    public int Id => _id;

    public TargetModel(int id)
    {
        _id = id;
    }
}