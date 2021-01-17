using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class npc : character
{
	public Rigidbody npc_rigidbody = null;

	private Dictionary<string, float> _axis_values = new Dictionary<string, float>();
	private Dictionary<string, k_key_input_type> _button_values = new Dictionary<string, k_key_input_type>();

	void Start()
	{
	}

	void Update()
	{
		_set_movement_value();
	}

	private void _set_movement_value()
	{
		NavMeshPath path = new NavMeshPath();
		bool path_found = NavMesh.CalculatePath(npc_rigidbody.position, new Vector3(4, 0, -4), -1, path);
		if (path_found && path.corners.Length > 1)
		{
			Vector3 next_goal = path.corners[1] - npc_rigidbody.position;
			next_goal.Normalize();
			_axis_values["movement_x"] = next_goal.x;
			_axis_values["movement_z"] = next_goal.z;
		}
		else
		{
			_axis_values["movement_x"] = 0.0f;
			_axis_values["movement_z"] = 0.0f;
		}
	}

	public override float get_axis_value(string name)
	{
		if (_axis_values.ContainsKey(name))
		{
			return _axis_values[name];
		}

		return 0.0f;
	}

	public override bool get_button_pressed(string name)
	{
		if (_button_values.ContainsKey(name))
		{
			return _button_values[name] == k_key_input_type.pressed;
		}

		return false;
	}

	public override bool get_button_down(string name)
	{
		if (_button_values.ContainsKey(name))
		{
			return _button_values[name] == k_key_input_type.pressed ||
			_button_values[name] == k_key_input_type.down;
		}

		return false;
	}

	public override bool get_button_released(string name)
	{
		if (_button_values.ContainsKey(name))
		{
			return _button_values[name] == k_key_input_type.released;
		}

		return false;
	}
}
