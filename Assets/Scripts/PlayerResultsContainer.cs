using System;

[Serializable]
public class PlayerResultsContainer
{
    public string CurrentPlayerName;
    public TimeSpan CurrentPlayerTime;

    public PlayerResultsContainer(string currentPlayerName, TimeSpan currentPlayerTime)
    {
        CurrentPlayerName = currentPlayerName;
        CurrentPlayerTime = currentPlayerTime;
    }

    public string GetShortPlayerTime()
    {
        return $"{CurrentPlayerTime.Seconds.ToString("00")}:" +
               $"{(CurrentPlayerTime.Milliseconds).ToString("00")}";
    }
    
    public override string ToString()
    {
        return $"Name: {CurrentPlayerName}, Time: {CurrentPlayerTime}";
    }
}
