using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UniRx.Triggers;

public class MouseHandlerScript : MonoBehaviour
{
    private bool _isMouseButtonDown = false;

    private GameController _controller;
    // Start is called before the first frame update
    void Start()
    {
        _controller = DependencyContainer.Get<GameController>();
    }

    // Update is called once per frame
    void Update()
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
