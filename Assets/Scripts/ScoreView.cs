using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class ScoreView : MonoBehaviour
{
    [SerializeField] private Text _scoreboardTextbox;
    // Start is called before the first frame update
    void Start()
    {
        DependencyContainer.Get<ScoreCounterModel>().Score.Subscribe(x =>
        {
            _scoreboardTextbox.text = x.ToString();
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
