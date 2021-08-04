using UniRx;
using UnityEngine;
 
public class CameraController : MonoBehaviour
{
	[SerializeField]private float _minY = -90f;
	[SerializeField]private float _maxY = 90f;

	[SerializeField]private float _sensitivity = 5f;
	private Camera _camera;

	private float _rotationY;
	private float _rotationX;

	void Start()
	{
		LockCursor();
		_camera = transform.GetChild(0).GetComponent<Camera>();
		DependencyContainer.Get<IGameCore>().RestartInitiated
			.TakeUntilDestroy(this)
			.Subscribe(_ =>
			{
				LockCursor();
			});
	}

	void Update()
	{
		_rotationY += Input.GetAxis("Mouse X") * _sensitivity;
		_rotationX += Input.GetAxis("Mouse Y") * _sensitivity;

		_rotationX = Mathf.Clamp(_rotationX, _minY, _maxY);

		transform.localEulerAngles = new Vector3(0, _rotationY, 0);
		_camera.transform.localEulerAngles = new Vector3(-_rotationX, 0, 0);

		if (Input.GetKeyDown(KeyCode.Escape))
		{
			UnlockCursor();
		}

		if (Cursor.visible && Input.GetMouseButtonDown(0))
		{
			LockCursor();
		}
	}

	public void UnlockCursor()
	{
		Cursor.lockState = CursorLockMode.None;
		Cursor.visible = true;
	}
	
	public void LockCursor()
	{
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
	}
}