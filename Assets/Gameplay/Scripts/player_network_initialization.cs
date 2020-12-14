using Mirror;
using Mirror.Experimental;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player_network_initialization : NetworkBehaviour
{
	public NetworkIdentity player_identity = null;

	[SerializeField]
	private player_movement _player_movement;

	[SerializeField]
	private GameObject _player_camera;

	[SerializeField]
	private NetworkRigidbody _network_rigidbody;

	public override void OnStartClient()
	{
		Debug.Log(input_manager.instance);

		if (!isLocalPlayer)
		{
			_player_movement.enabled = false;
			_player_camera.SetActive(false);
		}
		else
		{
			_player_movement.local_input_manager = input_manager.instance;
			Debug.Log(_player_movement.local_input_manager);
			_player_camera.GetComponent<player_camera>().local_input_manager = input_manager.instance;
		}
	}

	public string get_player_id()
	{
		return player_identity.name;
	}

	public bool is_player_local()
	{
		return player_identity.isLocalPlayer;
	}
}
