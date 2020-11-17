using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player_movement : MonoBehaviour
{
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
		float axis_x = (Input.GetKey(KeyCode.D) ? 1.0f : 0.0f) + (Input.GetKey(KeyCode.A) ? -1.0f : 0.0f);
		float axis_z = (Input.GetKey(KeyCode.W) ? 1.0f : 0.0f) + (Input.GetKey(KeyCode.S) ? -1.0f : 0.0f);

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
