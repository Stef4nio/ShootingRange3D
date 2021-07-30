using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetSpawner : MonoBehaviour
{
    [SerializeField] [Range(1, Mathf.Infinity)]private float _radius = 1f;
    [SerializeField] private GameObject _target;

    private IGameModel _gameModel;
    // Start is called before the first frame update
    void Start()
    {
        _gameModel = DependencyContainer.Get<IGameModel>();
        float verticalAngleDelta = Mathf.PI / (Config.TARGETS_AMOUNT - 1);
        float horizontalAngleDelta = Mathf.PI / Config.TARGETS_AMOUNT;
        for (int i = 0; i < Config.TARGETS_AMOUNT; i++)
        {
            Vector3 targetPosition = polarToCartesian(_radius,  i * verticalAngleDelta);
            targetPosition = Quaternion.Euler(-90,0,0) * targetPosition;
            for (int j = 0; j < Mathf.Ceil(Config.TARGETS_AMOUNT / 2f); j++)
            {
                targetPosition = Quaternion.Euler(0,2f * Mathf.Rad2Deg * horizontalAngleDelta , 0) * targetPosition;
                GameObject targetClone = Instantiate(_target, targetPosition, new Quaternion());
                if (i < Config.TARGETS_AMOUNT / 2)
                {
                    targetClone.GetComponent<TargetView>().SetModel(_gameModel.Targets[i,j]);    
                }
                else
                {
                    targetClone.GetComponent<TargetView>().SetModel(_gameModel.Targets[Config.TARGETS_AMOUNT - i - 1,j + 5]);
                }
            }
            //Instantiate(_target, targetPosition, new Quaternion());
            
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// Function that converts coordinates in polar system to cartesian system
    /// </summary>
    /// <param name="radius">Radius parameter of polar coords</param>
    /// <param name="angle">Angle parameter(in radians) of polar coords</param>
    /// <returns>Coords in cartesian system</returns>
    private Vector3 polarToCartesian(float radius, float angle)
    {
        return new Vector3(radius*Mathf.Cos(angle),0,radius*Mathf.Sin(angle));
    }
}
