public static class TargetModelFactory
{
    private static int _id = 0;

    public static TargetModel createTargetModel()
    {
        return new TargetModel(_id++);
    }
}
