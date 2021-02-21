using Mirror;
using Mirror.Experimental;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class network_player_manager : NetworkBehaviour
{
	public NetworkIdentity player_identity = null;

    [SerializeField]
    private player_character _player_character = null;

    [SerializeField]
    private player_movement _player_movement = null;
    
    [SerializeField]
    private GameObject _player_camera = null;

    public override void OnStartClient()
    {
        Debug.Log(input_manager.instance);
        Physics.IgnoreLayerCollision(10, 10, true);

        if (!isLocalPlayer)
        {
            _player_movement.enabled = false;
            _player_camera.SetActive(false);
        }
        else
        {
			_player_character.local_input_manager = input_manager.instance;
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
