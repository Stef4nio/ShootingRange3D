using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using UniRx;
using UnityEditor;
using UnityEngine;

public class PlayersResultsManager
{
    private string _currentPlayerName = "";
    private TimeSpan _currentPlayerTime = TimeSpan.Zero; 

    //Subscribes to game start and finish events to capture players name and time respectively
    public PlayersResultsManager()
    {
        DependencyContainer.Get<MainMenuController>().StartButtonPressed.Subscribe(_ =>
        {
            _currentPlayerName = DependencyContainer.Get<MainMenuController>().GetPlayerName();
        });
        DependencyContainer.Get<IScoreCounterModel>().CurrentGameState
            .Where(state => state == GameState.GameWon)
            .Subscribe(_ =>
            {
                _currentPlayerTime = DependencyContainer.Get<IScoreCounterModel>().GameDuration;
                SaveResults();
            });
    }

    //Puts player's results into a container and saves them to PlayerPrefs
    private void SaveResults()
    {
        //Get previous results
        string playerResultsJson = PlayerPrefs.GetString(Config.PLAYERS_RESULTS_PLAYERPREFS_KEY);
        List<PlayerResultsContainer> playersResults;
        if (playerResultsJson != "")
        {
            playersResults = JsonConvert.DeserializeObject<List<PlayerResultsContainer>>(playerResultsJson);
        }
        else
        {
            playersResults = new List<PlayerResultsContainer>();
        }

        //Add new results
        playersResults.Add(new PlayerResultsContainer(_currentPlayerName,_currentPlayerTime));
        
        playersResults.Sort((a, b) =>
        {
            if (a.CurrentPlayerTime < b.CurrentPlayerTime)
            {
                return -1;
            }

            if (a.CurrentPlayerTime > b.CurrentPlayerTime)
            {
                return 1;
            }

            return 0;
        });

        //We only have 10 view slots, so there is no need to save more
        if (playersResults.Count > 10)
        {
            playersResults.RemoveRange(10,playersResults.Count - 10);   
        }

        PlayerPrefs.SetString(Config.PLAYERS_RESULTS_PLAYERPREFS_KEY, JsonConvert.SerializeObject(playersResults));
        PlayerPrefs.Save();
    }
}
