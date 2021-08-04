using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class TargetView : MonoBehaviour
{
    private ITargetModel _targetModel;

    /// <summary>
    /// Sets a TargetModel into a TargetView, simultaneously subscribing to changes in model,
    /// to display them on the scene 
    /// </summary>
    /// <param name="model">A TargetModel to be assigned to a TargetView</param>
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

    /// <summary>
    /// Returns an Id of current target
    /// </summary>
    /// <returns></returns>
    public int GetId()
    {
        return _targetModel.Id;
    }

    /// <summary>
    /// Highlights(or stops highlighting) the target on the scene
    /// </summary>
    /// <param name="isHighlighted">True for highlighting the target, false otherwise</param>
    private void SetHighlight(bool isHighlighted)
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
