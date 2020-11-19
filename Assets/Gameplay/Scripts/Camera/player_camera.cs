using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player_camera : MonoBehaviour
{
	public input_manager local_input_manager = null;

	public new Component camera = null;
	public Transform camera_target = null;

	public Vector3 camera_position = new Vector3(0, 1.55f, 0);
	public float sensitivity_x = 5.0f;
	public float sensitivity_y = 5.0f;

	void Start()
	{
		camera.transform.position += camera_position;
		debug.print_line("player_camera of user " + "-1" + " initialized");	// TODO : figure out a way to identify who's camera this is in the future
    }

	void Update()
	{
		// TODO : add mouse smoothing
		float mouse_x = local_input_manager.get_camera().x;
		float mouse_y = local_input_manager.get_camera().y;
		
		// TODO : comment!
		camera.transform.Rotate(Vector3.up, mouse_x * sensitivity_x, Space.World);

		Vector3 rotation = camera.transform.localRotation.eulerAngles;
		float clamped_rotation_x = rotation.x - mouse_y * sensitivity_y;
		if (clamped_rotation_x > 90.0f && clamped_rotation_x < 270.0f)
		{
			// camera will be upside-down, clamp value
			clamped_rotation_x = mouse_y > 0 ? 270.0f + Mathf.Epsilon : 90.0f - Mathf.Epsilon;
		}
		rotation = new Vector3(clamped_rotation_x, rotation.y, rotation.z);
		camera.transform.localRotation = Quaternion.Euler(rotation);
		
		camera.transform.position = camera_target.position + camera_position;
	}
}
