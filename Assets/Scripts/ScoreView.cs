using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class ScoreView : MonoBehaviour
{
    [SerializeField] private Text _scoreboardTextbox = null;
    // Start is called before the first frame update
    void Start()
    {
        DependencyContainer.Get<IScoreCounterModel>().Score
            .TakeUntilDestroy(this)
            .Subscribe(x =>
        {
            _scoreboardTextbox.text = x.ToString();
            
        });
        DependencyContainer.Get<IScoreCounterModel>().CurrentGameState
            .TakeUntilDestroy(this)
            .Subscribe(gameState =>
            {
                switch (gameState)
                {
                    case GameState.GameWon:
                        _scoreboardTextbox.text = "YOU WIN!!!";
                        break;
                    case GameState.GameLost:
                        _scoreboardTextbox.text = "you lose...";
                        break;
                }
            });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
