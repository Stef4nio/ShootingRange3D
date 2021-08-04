using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class ScoreView : MonoBehaviour
{
    //Subscribes to score update, and gameState or restart events as well, to hide and show itself respectively
    void Start()
    {
        Text scoreboardTextbox = GetComponent<Text>();
        DependencyContainer.Get<IScoreCounterModel>().Score
            .TakeUntilDestroy(this)
            .Subscribe(x =>
        {
            scoreboardTextbox.text = x.ToString();
            
        });
        DependencyContainer.Get<IScoreCounterModel>().CurrentGameState
            .TakeUntilDestroy(this)
            .Subscribe(state =>
            {
                gameObject.SetActive(state == GameState.Playing);
            });
        DependencyContainer.Get<IGameCore>().RestartInitiated
            .TakeUntilDestroy(this)
            .Subscribe(_ =>
            {
                scoreboardTextbox.text = "0";
            });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
