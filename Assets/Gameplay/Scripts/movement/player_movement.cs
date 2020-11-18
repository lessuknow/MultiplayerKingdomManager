using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player_movement : MonoBehaviour
{
	public input_manager local_input_manager = null;

	public Transform player_camera_transform = null;
	public new Rigidbody rigidbody = null;

	public float speed = 2.0f;

	private const int fixme_framerate_value = 60;	// FIXME : create `time_util.cs`

	void Start()
	{
		
	}

	void Update()
	{
		// these ugly ternaries are temporary, input system will handle this more cleanly
		float axis_x = local_input_manager.get_movement().x;
		float axis_z = local_input_manager.get_movement().y;

		Vector3 movement = axis_x * transform.right + axis_z * transform.forward;
		movement = movement * speed;
		rigidbody.velocity = Vector3.Lerp(rigidbody.velocity, movement, 0.7f * Time.deltaTime * fixme_framerate_value);
	}

	void FixedUpdate()
	{
		float rotation_difference = player_camera_transform.rotation.eulerAngles.y - transform.rotation.eulerAngles.y;
		transform.Rotate(Vector3.up, rotation_difference, Space.World);
	}
}
