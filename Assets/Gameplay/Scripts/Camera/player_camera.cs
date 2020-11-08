using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player_camera : MonoBehaviour
{
	public new Component camera = null;
	public Transform target = null;

	public const float sensitivity_x = 5.0f;
	public const float sensitivity_y = 5.0f;

	void Start()
	{
		if (camera == null)
		{
			Debug.LogWarning("camera is null!");
		}

		camera.transform.position = target.position;
    }

	void Update()
	{
		float mouse_x = Input.GetAxis("Mouse X");
		float mouse_y = Input.GetAxis("Mouse Y");

		camera.transform.Rotate(Vector3.up, mouse_x * sensitivity_x, Space.World);
		camera.transform.Rotate(Vector3.right, -mouse_y * sensitivity_y, Space.Self);
	}
}
