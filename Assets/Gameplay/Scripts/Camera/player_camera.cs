using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player_camera : MonoBehaviour
{
	public new Component camera = null;
	public Transform camera_target = null;
	private Vector3 _camera_position = Vector3.zero;
	private Vector3 _camera_rotation = Vector3.zero;

	void Start()
	{
		if (camera == null)
		{
			Debug.LogWarning("camera is null!");
		}

		camera.transform.position = camera_target.position;
    }

	void Update()
	{
		float mouse_x = Input.GetAxis("Mouse X");
		float mouse_y = Input.GetAxis("Mouse Y");

		camera.transform.Rotate(Vector3.up, mouse_x, Space.World);
		camera.transform.Rotate(Vector3.right, mouse_y, Space.World);
	}
}
