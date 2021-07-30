using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetView : MonoBehaviour
{
    private ITargetModel _targetModel;

    public void SetModel(ITargetModel model)
    {
        _targetModel = model;
    }

    public void Shot()
    {
        Debug.Log($"Target #{_targetModel.Id} is shot!");
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
