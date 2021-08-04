using System;

//A class to save player's results to, so that it can be further serealized into JSON and saved to PlayerPrefs
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

    //Returns a formatted string of player's time
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
