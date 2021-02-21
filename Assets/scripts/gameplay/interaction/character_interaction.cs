using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class character_interaction : NetworkBehaviour
{
    /// <summary>
    /// The range the character can interact with objects.
    /// </summary>
    public float range = 8.5f;

    public character controlling_character;
    public Camera player_camera;

	// Object the character is currently holding.
	[SyncVar(hook = "_carried_object_changed"]
	private GameObject _carried_game_object;

    private interactable _carried_object;

    // Distance to hold the held object from.
    [SerializeField]
    private float hold_distance = 1.5f;

    void Update()
    {
		// TODO : refactor so that the actual input logic is done in `player_character.cs` or `npc.cs` depending
        if (isLocalPlayer)
        {
            process_inputs();
            update_objects();
        }
    }

    private void update_objects()
    {
        if (_carried_object != null && _carried_object is interactable_carry)
        {
            Vector3 cursor_worldpos = player_camera.ScreenToWorldPoint(Input.mousePosition);
            cmd_set_obj_pos(cursor_worldpos + player_camera.transform.forward * hold_distance);
        }
    }
	
	private void _carried_object_changed()
	{
		_carried_object = _carried_game_object ? _carried_game_object.GetComponent<interactable>() : null;
	}

	[Command]
    private void cmd_set_obj_pos(Vector3 pos)
    {
        rpc_set_obj_pos(pos);
    }

    [ClientRpc]
    private void rpc_set_obj_pos(Vector3 pos)
    {
        if(_carried_object)
        {
            ((interactable_carry)_carried_object).set_goal_position(pos);
        }
    }

    [Command]
    private void cmd_set_carried_obj(GameObject obj)
    {
        rpc_set_carried_obj(obj);
    }

    [ClientRpc]
    private void rpc_set_carried_obj(GameObject obj)
    {
        _carried_object = obj.GetComponent<interactable>();
        _carried_object.interact();
    }

    [Command]
    private void cmd_drop_carried_obj()
    {
        rpc_drop_carried_obj();
    }

    [ClientRpc]
    private void rpc_drop_carried_obj()
    {
        _carried_object.interact();
        _carried_object = null;
    }

	[Command]
	public void cmd_give_carried_object(GameObject target_character)
	{
		if (!_carried_object)
		{
			return;
		}

		character_interaction target_character_interaction = target_character.gameObject.GetComponent<character_interaction>();
		target_character_interaction.set_obj(_carried_object);
		_carried_object = null;
	}

	public void set_obj(interactable obj)
    {
		_carried_object = obj;
		_carried_object.interact();
    }

    private void process_inputs()
    {
        if (controlling_character.get_button_pressed("mouse_click"))
        {
            RaycastHit hit;
            // Change to camera direction
            Ray ray = player_camera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));

			if (Physics.Raycast(ray, out hit, range, 1 << 9))
			{
				if (!_carried_object)
				{
					cmd_set_carried_obj(hit.transform.gameObject);
				}
			}
			else if (Physics.Raycast(ray, out hit, range, 1 << 10))
			{
				debug.print_warning("WOW I HIT");
				bool self_carrying_object = _carried_object != null;
				character target_character = hit.collider.gameObject.GetComponentInParent<character>();
				debug.print_warning("WOW I AM " + target_character + " WOW WHAT WAS " + hit.collider.gameObject);
				character_interaction target_character_interaction = target_character.gameObject.GetComponentInChildren<character_interaction>();
				bool target_carrying_object = target_character_interaction.is_carrying_object();

				if (self_carrying_object && !target_carrying_object)
				{
					cmd_give_carried_object(target_character_interaction.gameObject);
				}
				else if (!self_carrying_object && target_carrying_object)
				{
					target_character_interaction.cmd_give_carried_object(gameObject);
				}
			}
			else
			{
				if (_carried_object)
				{
					//if (Physics.Raycast(ray, out hit, range, 1 << 10))
					//{
					//    hit.transform.gameObject.GetComponent<character_interaction>()
					//        .set_obj(_carried_object);
					//}
					//else
					{
						cmd_drop_carried_obj();
					}
					//}
				}
			}
        }
    }

	public bool is_carrying_object()
	{
		return _carried_object != null;
	}
}
