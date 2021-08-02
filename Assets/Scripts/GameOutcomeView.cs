using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOutcomeView : MonoBehaviour
{
    [SerializeField] private GameObject _gameFinishedPanel = null;
    [SerializeField] private Text _gameOutcomeTextbox = null;

    [SerializeField] private Button _restartButton = null;
    // Start is called before the first frame update
    void Start()
    {
        _gameFinishedPanel.SetActive(false);
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
            .TakeUntilDestroy(this)
            .Subscribe(gameState =>
            {
                switch (gameState)
                {
                    case GameState.GameWon:
                        _gameOutcomeTextbox.text = "You Won!!!";
                        _gameFinishedPanel.SetActive(true);
                        break;
                    case GameState.GameLost:
                        _gameOutcomeTextbox.text = "You Lost...";
                        _gameFinishedPanel.SetActive(true);
                        break;
                }
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
