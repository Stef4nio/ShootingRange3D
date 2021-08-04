using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UniRx.Triggers;
using UnityStandardAssets.Characters.FirstPerson;

[RequireComponent(typeof(CameraController))]
public class PlayerController : MonoBehaviour
{
    private bool _isMouseButtonDown = false;

    private GameController _controller;
    //Subscribes to gameState change events and to a restart event, to react properly
    void Start()
    {
        _controller = DependencyContainer.Get<GameController>();
        CameraController cameraController = GetComponent<CameraController>();
        DependencyContainer.Get<IScoreCounterModel>().CurrentGameState
            .TakeUntilDestroy(this)
            .Subscribe(gameState =>
            {
                
                if (gameState == GameState.Playing)
                {
                    cameraController.enabled = true;
                    cameraController.LockCursor();
                }
                else
                {
                    cameraController.enabled = false;
                    cameraController.UnlockCursor();
                }
            });
        DependencyContainer.Get<IGameCore>().RestartInitiated
            .TakeUntilDestroy(this)
            .Subscribe(_ =>
            {
                cameraController.LockCursor();
            });
        DependencyContainer.Get<IScoreCounterModel>().CurrentGameState
            .Where(state => state == GameState.Playing)
            .Take(1)
            .Subscribe(x =>
            {
                this.UpdateAsObservable().Where(_ => Input.GetMouseButtonDown(0) && !_isMouseButtonDown).Subscribe(_ =>
                {
                    _isMouseButtonDown = true;
                    Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5F, 0.5F, 0));
                    RaycastHit hit;
                    if (Physics.Raycast(ray, out hit))
                    {
                        _controller.DestroyTarget(hit.transform.gameObject.GetComponent<TargetView>().GetId());
                    }
                });
                this.UpdateAsObservable().Where(_ => !Input.GetMouseButton(0) && _isMouseButtonDown)
                    .Subscribe(_ => _isMouseButtonDown = false);
            });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
