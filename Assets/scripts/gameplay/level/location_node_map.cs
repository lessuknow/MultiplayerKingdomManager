using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class location_node_map : MonoBehaviour
{
	// TODO : remove
	private location_node[] _location_node_temp_array;

	private Dictionary<string, location_node> _location_nodes = new Dictionary<string, location_node>();

	void Start()
	{
		_location_node_temp_array = GetComponentsInChildren<location_node>();

		location_node[] nodes = GetComponentsInChildren<location_node>();
		for (int i = 0; i < nodes.Length; i++)
		{
			string node_name = nodes[i].name;
			_location_nodes[node_name] = nodes[i];
		}
	}

	// TODO : remove
	public Vector3 get_location_node_of_nearest_ai_value(int ai_value)
	{
		int best_value_diff = 10000;
		int best_index = 0;
		for (int i = 0; i < _location_node_temp_array.Length; i++)
		{
			int value_diff = Mathf.Abs(_location_node_temp_array[i].ai_value - ai_value);
			if (value_diff < best_value_diff)
			{
				best_value_diff = value_diff;
				best_index = i;
			}
		}

		return _location_node_temp_array[best_index].transform.position;
	}

	public Vector3 get_position_of_node(string node_name)
	{
		return _location_nodes[node_name].transform.position;
	}
}
