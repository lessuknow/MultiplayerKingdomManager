using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class player_interaction : NetworkBehaviour
{
    /// <summary>
    /// The range the player can interact with objects.
    /// </summary>
    public float range = 8.5f;

    public input_manager local_input_manager;
    public Camera player_camera;

    // Object the player is currently holding.
    private interactable _carried_object;

    // Distance to hold the held object from.
    [SerializeField]
    private float hold_distance = 1.5f;

    void Update()
    {
        if(isLocalPlayer)
        {
            process_inputs();
            update_objects();
        }
    }

    private void update_objects()
    {
        // rename to item
        if (_carried_object != null && _carried_object is interactable_carry)
        {
            Vector3 cursor_worldpos = player_camera.ScreenToWorldPoint(Input.mousePosition);
            cmd_set_obj_pos(cursor_worldpos + player_camera.transform.forward * hold_distance);
        }
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


    public void set_obj(GameObject obj)
    {
        _carried_object = obj.GetComponent<interactable>();
        _carried_object.interact();
    }

    private void process_inputs()
    {
        if (local_input_manager.get_mouse_pressed())
        {
            RaycastHit hit;
            // Change to camera direction
            Ray ray = player_camera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));

            if (Physics.Raycast(ray, out hit, range, 1 << 9) && _carried_object == null)
            {
                cmd_set_carried_obj(hit.transform.gameObject);
            }
            else
            {
                if (_carried_object)
                {
                    //if (Physics.Raycast(ray, out hit, range, 1 << 10))
                    //{
                    //    hit.transform.gameObject.GetComponent<player_interaction>()
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
}
