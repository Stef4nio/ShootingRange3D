using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

//Rename to GameView
public class TargetSpawner : MonoBehaviour
{
    [SerializeField] [Range(1f, 25f)]private float _radius = 1f;
    [SerializeField] private GameObject _target = null;

    private List<TargetView> _allTargetViews = new List<TargetView>();
    
    private Subject<Unit> _targetsSpawned = new Subject<Unit>();

    public IObservable<Unit> TargetsSpawned => _targetsSpawned;

    private IGameModel _gameModel;

    //Subscribes to a game starting, to instantiate tagets
    void Start()
    {
        _gameModel = DependencyContainer.Get<IGameModel>();
        DependencyContainer.Get<IScoreCounterModel>().CurrentGameState
            .Where(state => state == GameState.Playing)
            .Take(1)
            .Subscribe(x =>
            {   
                SpawnTargets();
            });
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// Spawns targets in a hemisphere, assigning them their correspondent models
    /// </summary>
    private void SpawnTargets()
    {
        //How many radians will separate layers of targets
        float verticalAngleDelta = Mathf.PI / (Config.TARGETS_AMOUNT - 1);
        //How many radians will separate targets in their layer
        float horizontalAngleDelta = Mathf.PI / Config.TARGETS_AMOUNT;
        //Iterating over targets layers 
        for (int i = 0; i < Config.TARGETS_AMOUNT; i++)
        {
            //The idea here is that in polar system it's easier to spread the targets evenly
            //Then we convert polar coord to our needed Cartesian coords and start spawning
            Vector3 targetPosition = polarToCartesian(_radius,  i * verticalAngleDelta);
            //In previous line our targets layers are set to a perpendicular direction due to converting 2d polar coords
            //to 2d Cartesian coords, so whe set their z coordinate to a needed value, by rotating them 90 degrees
            targetPosition = Quaternion.Euler(-90,0,0) * targetPosition;
            //Iterating through layers
            for (int j = 0; j < Mathf.Ceil(Config.TARGETS_AMOUNT / 2f); j++)
            {
                //Here we go ahead and create our layer, bu rotating targets around central axis
                targetPosition = 
                    Quaternion.Euler(0,2f * Mathf.Rad2Deg * horizontalAngleDelta , 0) * targetPosition;
                GameObject targetClone = Instantiate(_target, targetPosition, new Quaternion());
                TargetView currTargetView = targetClone.GetComponent<TargetView>();
                //The thing is that we iterate over the layers like through an arch, so at some point our arch will
                //reach its peak and start to go downwards, that will change the way we assign targetModels
                //because in our main game model we want them to be stored layer by layer
                if (i < Config.TARGETS_AMOUNT / 2)
                {
                    currTargetView.SetModel(_gameModel.Targets[i][j]);    
                }
                else
                {
                    currTargetView.SetModel(
                        _gameModel.Targets[Config.TARGETS_AMOUNT - i - 1][j + Config.TARGETS_AMOUNT / 2]);
                }

                targetClone.name = "Target #" + currTargetView.GetId();
                _allTargetViews.Add(currTargetView);
            }
        }
        _targetsSpawned.OnNext(Unit.Default);
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

    /// <summary>
    /// Returns a target position on the scene by it's id
    /// </summary>
    /// <param name="id">Id of a target which position you want to get</param>
    /// <returns>Vector3 position of a target in the scene</returns>
    public Vector3 GetTargetPositionById(int id)
    {
        return _allTargetViews.Find(view => view.GetId() == id).gameObject.transform.position;
    }
}
