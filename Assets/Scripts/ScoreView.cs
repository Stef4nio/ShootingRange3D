using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class ScoreView : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Text _scoreboardTextbox = GetComponent<Text>();
        DependencyContainer.Get<IScoreCounterModel>().Score
            .TakeUntilDestroy(this)
            .Subscribe(x =>
        {
            _scoreboardTextbox.text = x.ToString();
            
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
                _scoreboardTextbox.text = "0";
            });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
