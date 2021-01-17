using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player_movement : MonoBehaviour
{
	public character controlling_character = null;

	public Transform player_look_transform = null;
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

	enum k_jump_state
	{
		none,
		jump_pressed,
		jumping
	};
	private k_jump_state jump_state = k_jump_state.none;

	void Start()
	{
		
	}

	void Update()
	{
		if (controlling_character.get_button_down("jump") && jump_state == k_jump_state.none)
		{
			jump_state = k_jump_state.jump_pressed;
		}
	}

	void FixedUpdate()
	{
		/* inputs */

		Vector2 movement_input = Vector2.zero;
		if (controlling_character)
		{
			movement_input.x = controlling_character.get_axis_value("movement_x");
			movement_input.y = controlling_character.get_axis_value("movement_z");
		}

		/* movement */

		Vector3 goal_velocity = self_rigidbody.velocity;

		if (movement_input.x != 0 || movement_input.y != 0)
		{
			Vector3 input_velocity = movement_input.x * transform.right + movement_input.y * transform.forward;
			bool sprinting = controlling_character.get_button_down("sprint");
			float speed_total = speed * (sprinting ? sprint_multiplier : 1.0f);
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
		
		if (jump_state == k_jump_state.jump_pressed)
		{
			if (on_ground)
			{
				jump_state = k_jump_state.jumping;
				self_rigidbody.AddForce(Vector3.up * initial_jump_force * self_rigidbody.mass - Physics.gravity);
			}
			else
			{
				jump_state = k_jump_state.none;
			}
		}
		else if (jump_state == k_jump_state.jumping)
		{
			if (on_ground)
			{
				jump_state = k_jump_state.none;
			}
		}

		/* velocity application */

		self_rigidbody.velocity = Vector3.Lerp(self_rigidbody.velocity, goal_velocity, velocity_lerp_strength * Time.deltaTime * fixme_framerate_value);

		/* rotation */
		
		if (controlling_character is player_character)
		{
			float rotation_difference = player_look_transform.rotation.eulerAngles.y - transform.rotation.eulerAngles.y;
			transform.Rotate(Vector3.up, rotation_difference, Space.World);
		}
	}

	/// <summary>
	/// Return if the player is on the ground, or not, as well as writing out the ground's y position.
	/// </summary>
	/// <param name="ground_collision_y"> The y value in world space that a collision occurred if this function returns true. </param>
	/// <returns> true if on the ground, false otherwise </returns>
	bool get_ground_collision(out float ground_collision_y)
	{
		const int k_max_collisions = 16;	// TODO : expose
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

			if (collisions[i].gameObject.layer == 10)
            {
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

			// TODO : in the future, account for collision's velocity and tag,
			//		i.e. don't treat a book flying through the air as the ground

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
