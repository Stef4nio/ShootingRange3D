using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class TargetView : MonoBehaviour
{
    private ITargetModel _targetModel;

    public void SetModel(ITargetModel model)
    {
        _targetModel = model;
        _targetModel.IsDestroyed
            .TakeUntilDestroy(this)
            .Subscribe(isDestroyed =>
            {
                gameObject.SetActive(!isDestroyed);
            });
        _targetModel.IsHighlighted
            .TakeUntilDestroy(this)
            .Subscribe(isHighlighted =>
            {
                SetHighlight(isHighlighted);
            });
    }

    public int GetId()
    {
        return _targetModel.Id;
    }

    public void SetHighlight(bool isHighlighted)
    {
        transform.GetChild(0).gameObject.SetActive(isHighlighted);
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
