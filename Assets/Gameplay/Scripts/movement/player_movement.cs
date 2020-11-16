using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player_movement : MonoBehaviour
{
	public Transform player_camera_transform = null;
	public new Rigidbody rigidbody = null;

	public float speed = 2.0f;

	internal Vector3 previous_position = Vector3.zero;

	private const int fixme_framerate_value = 60;

	private Vector3 movement = Vector3.zero;

	void Start()
	{
		
	}

	void Update()
	{
		float rotation_difference = player_camera_transform.rotation.eulerAngles.y - transform.rotation.eulerAngles.y;
		transform.Rotate(Vector3.up, rotation_difference, Space.World);

		previous_position = transform.position;

		// these ugly ternaries are temporary, input system will handle this more cleanly
		float axis_z = (Input.GetKey(KeyCode.W) ? 1.0f : 0.0f) + (Input.GetKey(KeyCode.S) ? -1.0f : 0.0f);
		float axis_x = (Input.GetKey(KeyCode.D) ? 1.0f : 0.0f) + (Input.GetKey(KeyCode.A) ? -1.0f : 0.0f);

		movement = axis_x * speed * Time.deltaTime * fixme_framerate_value * transform.right + axis_z * speed * Time.deltaTime * fixme_framerate_value * transform.forward;
		debug.print_line(movement + "");
		rigidbody.AddRelativeForce(movement * rigidbody.mass, ForceMode.Force);
		if (movement != Vector3.zero)
		{
			//rigidbody.AddRelativeForce(movement, ForceMode.VelocityChange);
		}
		else
		{
			//rigidbody.velocity = Vector3.zero;
		}
		//transform.Translate(movement, transform);
	}

	void FixedUpdate()
	{
		//rigidbody.AddRelativeForce(movement, ForceMode.VelocityChange);
	}
}
