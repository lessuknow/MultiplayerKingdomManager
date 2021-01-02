using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player_movement : MonoBehaviour
{
	public input_manager local_input_manager = null;

	public Transform player_camera_transform = null;
	public Rigidbody self_rigidbody = null;
	public Collider self_collider = null;

	public float speed = 2.0f;
	public float sprint_multiplier = 1.7f;
	public float velocity_lerp_strength = 0.7f;

	public float initial_jump_force = 250.0f;

	public float ground_collision_radius = 0.25f;
	public float ground_collision_height = 0.2f;
	public float k_ground_collision_epsilon = 0.01f;
	public float k_vaulting_height = 0.20f;

	private const int fixme_framerate_value = 60;   // FIXME : create `time_util.cs`

	// input cache
	private Vector2 movement_input = Vector2.zero;
	private bool jump_pressed_input = false;

	// TODO : make this a jump state enum for finer control, less bugs
	//private bool jumping = false;

	void Start()
	{
		
	}

	void Update()
	{
		movement_input = local_input_manager.get_movement();

		if (local_input_manager.get_jump_pressed())
		{
			// we clear this value in FixedUpdate so that we don't drop the input
			jump_pressed_input = true;
		}
	}

	void FixedUpdate()
	{
		if (!local_input_manager)
		{
			return;
		}

		/* inputs */
		
		float axis_x = local_input_manager.get_movement().x;
		float axis_z = local_input_manager.get_movement().y;

		//bool jump_pressed = jump_pressed_input;
		jump_pressed_input = false;

		/* movement */

		Vector3 goal_velocity = self_rigidbody.velocity;

		if (axis_x != 0 || axis_z != 0)
		{
			Vector3 input_velocity = axis_x * transform.right + axis_z * transform.forward;
			float speed_total = speed * (local_input_manager.get_sprint_down() ? sprint_multiplier : 1.0f);
			input_velocity = input_velocity * speed_total;
			goal_velocity.x = input_velocity.x;
			goal_velocity.z = input_velocity.z;
		}
		else
		{
			goal_velocity.x = 0.0f;
			goal_velocity.z = 0.0f;
		}

		/* ground collision handling */

		float ground_collision_y = self_rigidbody.position.y;
		bool on_ground = get_ground_collision(out ground_collision_y);

		if (on_ground)
		{
			self_rigidbody.velocity = new Vector3(self_rigidbody.velocity.x, 0.0f, self_rigidbody.velocity.z);
			goal_velocity.y = 0.0f;
			self_rigidbody.position = new Vector3(self_rigidbody.position.x, ground_collision_y - k_ground_collision_epsilon, self_rigidbody.position.z);
		}
		else
		{
			float velocity_change = Physics.gravity.y * Time.fixedDeltaTime;
			self_rigidbody.velocity = new Vector3(self_rigidbody.velocity.x, self_rigidbody.velocity.y + velocity_change, self_rigidbody.velocity.z);
		}
		goal_velocity.y = self_rigidbody.velocity.y;

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

		/* rotation */
		
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
		Vector3 sphere_check_position = self_rigidbody.position + Vector3.up * ground_collision_height;
		int collision_count = Physics.OverlapSphereNonAlloc(sphere_check_position, ground_collision_radius, collisions);

		float k_max_y_value = self_rigidbody.position.y + k_vaulting_height;
		float best_y_value = self_rigidbody.position.y;
		bool value_found = false;
		for (int i = 0; i < collision_count; i++)
		{
			if (collisions[i] == self_collider)
			{
				// this is the player's collider
				continue;
			}

			if (collisions[i].isTrigger)
			{
				// this is a trigger and should not be treated as a collider
				continue;
			}

			// we calculate the bottom of a sphere resting on the collision point, regardless of if the sphere is not centered on said point
			float collision_distance = Vector3.Magnitude(sphere_check_position - collisions[i].ClosestPoint(sphere_check_position));
			float collision_y_value = sphere_check_position.y - collision_distance;

			if (collision_y_value > k_max_y_value || collision_y_value < self_rigidbody.position.y)
			{
				// this value is too high up to be considered walkable, or too low to be standing on
				continue;
			}

			// TODO : account for slopes with some height & horizontal distance calculation?

			// TODO : in the future, account for collision's velocity if existant

			if (collision_y_value > best_y_value)
			{
				best_y_value = collision_y_value;
				value_found = true;
			}
		}

		ground_collision_y = value_found ? best_y_value : self_rigidbody.position.y;
		return value_found;
	}
}
