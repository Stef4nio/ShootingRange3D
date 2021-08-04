public static class TargetModelFactory
{
    private static int _id = 0;

    /// <summary>
    /// Returns a newly created target with unique id
    /// </summary>
    /// <returns></returns>
    public static TargetModel CreateTargetModel()
    {
        return new TargetModel(_id++);
    }
}
