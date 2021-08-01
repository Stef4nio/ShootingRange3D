using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class TargetView : MonoBehaviour
{
    //Subscribe to a reactive bool property that is to be created
    private ITargetModel _targetModel;

    public void SetModel(ITargetModel model)
    {
        _targetModel = model;
        _targetModel.IsDestroyed
            .Where(x => x)
            .TakeUntilDestroy(this)
            .Subscribe(x =>
            {
                Debug.Log($"Target #{_targetModel.Id} is properly destroyed!");
                Destroy(gameObject);
            });
    }

    public int GetId()
    {
        return _targetModel.Id;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
