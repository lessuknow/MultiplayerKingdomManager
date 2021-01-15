using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class npc_base : MonoBehaviour
{
	public Rigidbody npc_rigidbody = null;

	void Start()
	{
		NavMeshPath path = new NavMeshPath();
		NavMesh.CalculatePath(Vector3.zero, new Vector3(4, 0, -4), NavMesh.AllAreas, path);
		debug.print_warning("" + path.corners);
	}

	void Update()
	{
		//NavMeshPath path = new NavMeshPath();
		//NavMesh.CalculatePath(transform.position, new Vector3(4, 0, -4), -1, path);
	}

	public Vector2 get_movement_input()
	{
		NavMeshPath path = new NavMeshPath();
		bool path_found = NavMesh.CalculatePath(npc_rigidbody.position, new Vector3(4, 0, -4), -1, path);
		if (path_found)
		{
			Vector3 next_goal = path.corners[1] - npc_rigidbody.position;
			next_goal.Normalize();
			debug.print_warning("WOPW" + path.corners[1]);
			return new Vector2(next_goal.x, next_goal.z);
		}
		
		return Vector2.zero;
	}
}
