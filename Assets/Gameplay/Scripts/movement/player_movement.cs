using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player_movement : MonoBehaviour
{
	public input_manager local_input_manager = null;

	public Transform player_camera_transform = null;
	public new Rigidbody self_rigidbody = null;
	public Collider self_collider = null;

	public float speed = 2.0f;
	public float sprint_multiplier = 1.7f;
	public float velocity_lerp_strength = 0.7f;

	public float ground_raycast_distance = 0.04f;
	public float spherecast_radius = 0.08f;	// TODO : rename

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

		Vector3 goal_velocity = self_rigidbody.velocity;

		if (axis_x != 0 || axis_z != 0)
		{
			Vector3 movement_velocity = axis_x * transform.right + axis_z * transform.forward;
			float speed_total = speed * (local_input_manager.get_sprint_down() ? sprint_multiplier : 1.0f);
			movement_velocity = movement_velocity * speed_total;
			goal_velocity.x = movement_velocity.x;
			goal_velocity.z = movement_velocity.z;
		}
		else
		{
			goal_velocity.x = 0.0f;
			goal_velocity.z = 0.0f;
		}

		float ground_collision_y = transform.position.y;
		bool on_ground = get_ground_collision(out ground_collision_y);

		if (on_ground)
		{
			goal_velocity.y = 0.0f;	// FIXME : this won't work for downward velocity, need direct set
			self_rigidbody.position = new Vector3(self_rigidbody.position.x, ground_collision_y, self_rigidbody.position.z);
		}

		/* jumping */
		/*RaycastHit ray_hit;
		bool on_ground = Physics.SphereCast(transform.position, spherecast_radius, Vector3.down, out ray_hit, Mathf.Abs(ground_raycast_distance - spherecast_radius), 1 << 8);

		if (!jumping)
		{
			if (on_ground)
			{
				// if on ground, we don't want any vertical velocity
				// FIXME : this will not completely work. needs to be handled in FixedUpdate (I think)
				self_rigidbody.velocity = new Vector3(self_rigidbody.velocity.x, 0, self_rigidbody.velocity.y);
			}

			if (local_input_manager.get_jump_down())
			{
				if (on_ground)
				{
					jumping = true;
					self_rigidbody.AddForce(Vector3.up * initial_jump_force * self_rigidbody.mass - Physics.gravity);
				}
			}
		}
		else
		{
			jumping = local_input_manager.get_jump_down();
			
			if (self_rigidbody.velocity.y < 0 && !on_ground)
			{
				// (for now) set jumping to false automatically when velocity goes negative
				jumping = false;
			}
		}*/

		/* velocity application */

		self_rigidbody.velocity = Vector3.Lerp(self_rigidbody.velocity, goal_velocity, velocity_lerp_strength * Time.deltaTime * fixme_framerate_value);
	}

	void FixedUpdate()
	{
		// setting player rotation to camera rotation here so
		//	that all position work is in the physics step
		float rotation_difference = player_camera_transform.rotation.eulerAngles.y - transform.rotation.eulerAngles.y;
		transform.Rotate(Vector3.up, rotation_difference, Space.World);
	}

	/// <summary>
	/// Return if the player is on the ground, or not, as well as writing out the ground's y position.
	/// </summary>
	/// <param name="ground_collision_y"> The y value in world space that a collision occurred if this function returns true. </param>
	/// <returns> true if on the ground, false otherwise </returns>
	bool get_ground_collision(out float ground_collision_y)
	{
		const int k_max_collisions = 16;
		Collider[] collisions = new Collider[k_max_collisions];
		Vector3 sphere_check_position = transform.position + Vector3.up * spherecast_radius;
		int collision_count = Physics.OverlapSphereNonAlloc(sphere_check_position, spherecast_radius, collisions);

		float k_max_acceptable_y_diff = 0.25f;    // TODO : expose

		float k_max_y_value = transform.position.y + k_max_acceptable_y_diff;
		float highest_y_value = transform.position.y;
		bool value_found = false;
		for (int i = 0; i < collision_count; i++)
		{
			if (collisions[i] == self_collider)
			{
				// this is the player's collider
				continue;
			}

			float collision_y_value = collisions[i].ClosestPointOnBounds(sphere_check_position).y;
			if (collision_y_value > k_max_y_value)
			{
				// this value is too high up to be considered walkable
				continue;
			}

			// TODO : in the future, account for collision's velocity if existant

			if (collision_y_value > highest_y_value)
			{
				highest_y_value = collision_y_value;
				value_found = true;
			}
		}

		ground_collision_y = value_found ? highest_y_value : transform.position.y;
		return value_found;
	}
}
