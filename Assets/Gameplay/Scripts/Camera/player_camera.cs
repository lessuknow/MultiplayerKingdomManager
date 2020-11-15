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
		debug.print_warning_if(camera == null, "camera is null!");
		debug.print_warning_if(target == null, "target is null!");

		camera.transform.position = target.position;
		debug.print_line("player_camera of user " + "-1" + " initialized");	// TODO : figure out a way to identify who's camera this is in the future
    }

	void Update()
	{
		float mouse_x = Input.GetAxis("Mouse X");
		float mouse_y = Input.GetAxis("Mouse Y");
		
		camera.transform.Rotate(Vector3.up, mouse_x * sensitivity_x, Space.World);
		camera.transform.Rotate(Vector3.right, -mouse_y * sensitivity_y, Space.Self);
		
		if (camera.transform.up.y < 0)
		{
			// camera upside-down, clamp the rotation
			//Vector3 rotation = camera.transform.localRotation.eulerAngles;
			//float clamped_rotation_x = mouse_y > 0 ? 270 : 90;
			//debug.print_line(clamped_rotation_x + "");
			//rotation = new Vector3(clamped_rotation_x, rotation.y, rotation.z);
			//camera.transform.localRotation = Quaternion.Euler(rotation);
			//debug.print_line(camera.transform.localRotation.eulerAngles + "wow");
		}
	}
}
