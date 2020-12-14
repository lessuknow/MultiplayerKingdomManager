using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player_movement : MonoBehaviour
{
	public input_manager local_input_manager = null;

	public Transform player_camera_transform = null;
	public new Rigidbody rigidbody = null;

	public float speed = 2.0f;
	public float velocity_lerp_strength = 0.7f;

	public float ground_raycast_distance = 0.04f;
	public float spherecast_radius = 0.08f;

	public float initial_jump_force = 250.0f;

	private const int fixme_framerate_value = 60;   // FIXME : create `time_util.cs`

	// TODO : make this a jump state enum for finer control, less bugs
	private bool jumping = false;

	void Start()
	{
		
	}

	void Update()
	{		
		if(!local_input_manager)
		{
			return;
		}

		/* inputs */

		float axis_x = local_input_manager.get_movement().x;
		float axis_z = local_input_manager.get_movement().y;

		/* movement */

		Vector3 goal_velocity = rigidbody.velocity;

		if (axis_x != 0 || axis_z != 0)
		{
			Vector3 movement_velocity = axis_x * transform.right + axis_z * transform.forward;
			movement_velocity = movement_velocity * speed;
			goal_velocity.x = movement_velocity.x;
			goal_velocity.z = movement_velocity.z;
		}
		else
		{
			goal_velocity.x = 0.0f;
			goal_velocity.z = 0.0f;
		}
		
		/* jumping */
		// TODO : solve for temporary hack because UNITY IGNORES RAYCASTS IF THEY START IN AN OBJECT (WHY)
		bool on_ground = Physics.SphereCast(transform.position + Vector3.up, spherecast_radius, Vector3.down, out RaycastHit ray_hit, ground_raycast_distance + 1, 1 << 8);

		if (!jumping)
		{
			if (on_ground)
			{
				// if on ground, we don't want any vertical velocity
				// FIXME : this will not completely work. needs to be handled in FixedUpdate (I think)
				rigidbody.velocity = new Vector3(rigidbody.velocity.x, 0, rigidbody.velocity.y);
			}

			if (local_input_manager.get_jump_down())
			{
				if (on_ground)
				{
					jumping = true;
					rigidbody.AddForce(Vector3.up * initial_jump_force * rigidbody.mass - Physics.gravity);
				}
			}
		}
		else
		{
			jumping = local_input_manager.get_jump_down();
			
			if (rigidbody.velocity.y < 0 && !on_ground)
			{
				// (for now) set jumping to false automatically when velocity goes negative
				jumping = false;
			}
		}

		/* velocity application */
		
		rigidbody.velocity = Vector3.Lerp(rigidbody.velocity, goal_velocity, velocity_lerp_strength * Time.deltaTime * fixme_framerate_value);
	}

	void FixedUpdate()
	{
		// setting player rotation to camera rotation here so
		//	that all position work is in the physics step
		float rotation_difference = player_camera_transform.rotation.eulerAngles.y - transform.rotation.eulerAngles.y;
		transform.Rotate(Vector3.up, rotation_difference, Space.World);
	}
}
