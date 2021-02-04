using Mirror;
using Mirror.Experimental;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player_network_initialization : NetworkBehaviour
{
	public NetworkIdentity player_identity = null;

    [SerializeField]
    private player_character _player_character;

    [SerializeField]
    private player_movement _player_movement;
    
    [SerializeField]
    private GameObject _player_camera;

    [SerializeField]
    private player_interaction _player_interaction;

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
            _player_interaction.local_input_manager = input_manager.instance;
            _player_interaction.player_camera = _player_camera.GetComponent<Camera>();
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
