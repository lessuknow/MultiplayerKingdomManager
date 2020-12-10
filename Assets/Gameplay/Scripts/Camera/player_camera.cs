using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player_camera : MonoBehaviour
{
	public input_manager local_input_manager = null;

	public new Component camera = null;
	public Transform camera_target = null;

	public Vector3 camera_position = new Vector3(0, 1.55f, 0);
	public float sensitivity_x = 1.0f;
	public float sensitivity_y = 1.0f;
	public float smooth_factor = 0.25f;

	private Vector2 mouse_move_buffer = Vector2.zero;

	void Start()
	{
		camera.transform.position += camera_position;
		debug.print_line("player_camera of user " + "-1" + " initialized");	// TODO : figure out a way to identify who's camera this is in the future
    }

	void Update()
	{
		if(!local_input_manager)
		{
			return;
		}

		/* inputs */
		Vector2 mouse_move = local_input_manager.get_camera();
		
		// smooth the input
		mouse_move_buffer += mouse_move;
		mouse_move = mouse_move_buffer * smooth_factor;
		mouse_move_buffer = mouse_move_buffer * (1 - smooth_factor);
		
		// rotate along the world's y axis first, looks left/right
		camera.transform.Rotate(Vector3.up, mouse_move.x * sensitivity_x, Space.World);

		/* camera rotation */

		// rotate along the camera's local x axis next, looks up/down
		Vector3 rotation = camera.transform.localRotation.eulerAngles;
		float clamped_rotation_x = rotation.x - mouse_move.y * sensitivity_y;
		// is camera beyond reasonable rotation bounds?
		if (clamped_rotation_x > 90.0f && clamped_rotation_x < 270.0f)
		{
			// camera beyond bounds, clamp to discrete value according to mouse movement direction
			clamped_rotation_x = mouse_move.y < 0 ? 90.0f - Mathf.Epsilon : 270.0f + Mathf.Epsilon;
		}
		rotation = new Vector3(clamped_rotation_x, rotation.y, rotation.z);
		camera.transform.localRotation = Quaternion.Euler(rotation);
		
		/* camera movement */

		camera.transform.position = camera_target.position + camera_position;
	}
}
