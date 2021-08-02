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
    // Start is called before the first frame update
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
                    GetComponent<CameraController>().enabled = false;
                    cameraController.UnlockCursor();
                }
             });
        DependencyContainer.Get<IGameCore>().RestartInitiated
            .TakeUntilDestroy(this)
            .Subscribe(_ =>
            {
                cameraController.LockCursor();
            });
    }


    private void EnableControls()
    {
        //GetComponent<FirstPersonController>().
    }
    private void DisableControls()
    {
        
    }
    
    // Update is called once per frame
    void Update()
    {
        if (DependencyContainer.Get<IScoreCounterModel>().CurrentGameState.Value == GameState.Playing)
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
        }
    }
}
