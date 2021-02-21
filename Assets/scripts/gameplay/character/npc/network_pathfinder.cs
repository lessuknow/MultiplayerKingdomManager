using Mirror;
using Mirror.Experimental;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class network_pathfinder : NetworkBehaviour
{
	// FIXME : all of these values are being replicated :(

	//[SerializeField]
	//private npc _npc = null;

	[SerializeField]
	private location_node_map _level_node_map = null;

	[SerializeField]
	private int _ai_value = 0;

	[SyncVar]
	private Vector3 _goal_position = Vector3.zero;

	void Start()
	{
		if (isServer)
		{
			_level_node_map = FindObjectOfType<location_node_map>();
		}
		else
		{
			enabled = false;
		}
	}

	void Update()
	{
		if (!isServer)
		{
			debug.print_warning("Update called when not server. Disabling self.");
			enabled = false;
			return;
		}
		
		_determine_goal();
	}

	private void _determine_goal()
	{
		_ai_value += 1;
		if (_ai_value > 15000)
		{
			_ai_value = 0;
		}
		
		_goal_position = _level_node_map.get_location_node_of_nearest_ai_value(_ai_value);
	}

	public Vector3 get_goal()
	{
		return _goal_position;
	}
}
