using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UniRx;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOutcomeView : MonoBehaviour
{
    [SerializeField] private GameObject _gameFinishedPanel = null;
    [SerializeField] private Text _gameOutcomeTextbox = null;
    [SerializeField] private Text _playerTimeTextbox = null;
    [SerializeField] private GameObject _leaderboardPanel = null;
    [SerializeField] private Button _leaderboardBackButton;
    
    [SerializeField] private Button _restartButton = null;
    [SerializeField] private Button _leaderboardButton = null;
    // Start is called before the first frame update
    void Start()
    {
        _gameFinishedPanel.SetActive(false);
        _leaderboardButton.OnClickAsObservable()
            .TakeUntilDestroy(this)
            .Subscribe(_ =>
            {
                gameObject.SetActive(false);
                _leaderboardPanel.GetComponent<LeaderboardView>().Show(
                    JsonConvert.DeserializeObject<List<PlayerResultsContainer>>(
                        PlayerPrefs.GetString(Config.PLAYERS_RESULTS_PLAYERPREFS_KEY)));
            });
        _leaderboardBackButton.OnClickAsObservable()
            .TakeUntilDestroy(this)
            .Subscribe(_ =>
            {
                _leaderboardPanel.GetComponent<LeaderboardView>().Hide();
                gameObject.SetActive(true);
            });
        _restartButton.OnClickAsObservable()
            .TakeUntilDestroy(this)
            .Subscribe(_ =>
            {
                RestartGame();
            });
        DependencyContainer.Get<IGameCore>().RestartInitiated
            .TakeUntilDestroy(this)
            .Subscribe(_ =>
            {
                _gameFinishedPanel.SetActive(false);
            });
        DependencyContainer.Get<IScoreCounterModel>().CurrentGameState
            .Where(state => state == GameState.GameWon || state == GameState.GameLost)
            .TakeUntilDestroy(this)
            .Subscribe(gameState =>
            {

                if(gameState == GameState.GameWon)
                {
                    _gameOutcomeTextbox.text = "You Won!!!";
                    TimeSpan gameDuration = DependencyContainer.Get<IScoreCounterModel>().GameDuration;
                    _playerTimeTextbox.text =
                        $"Well played! Your time: {gameDuration.Seconds.ToString("00")}:" +
                        $"{(gameDuration.Milliseconds).ToString("00")}";
                    //time format is wrong, first number of milliseconds is now omitted...
                }
                else
                {
                    _gameOutcomeTextbox.text = "You Lost...";
                    switch (DependencyContainer.Get<IScoreCounterModel>().LoseCause)
                    {
                        case LoseCause.OutOfTime:
                            _playerTimeTextbox.text = "You ran out of time...";

                            break;
                        case LoseCause.NotEnoughTargetsLeft:
                            _playerTimeTextbox.text = "You didn't shoot the HIGHLIGHTED targets...";
                            break;
                    }
                }
                _gameFinishedPanel.SetActive(true);
            });
    }

    private void RestartGame()
    {
        DependencyContainer.Get<IRestartableScoreCounterModel>().InitiateRestart();
    }
    
    
    // Update is called once per frame
    void Update()
    {
        
    }
}
