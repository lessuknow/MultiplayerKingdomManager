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
    public Camera camera;

    // Object the player is currently holding.
    private interactable _carried_object;

    // Distance to hold the held object from.
    [SerializeField]
    private float hold_distance = 1.5f;

    void Update()
    {
        update_objects();
        process_inputs();
    }

    private void update_objects()
    {
        // rename to item
        if (_carried_object is interactable_carry)
        {
            Vector3 cursor_worldpos = camera.ScreenToWorldPoint(Input.mousePosition);
            ((interactable_carry)_carried_object).set_goal_position(cursor_worldpos + camera.transform.forward * hold_distance);
        }
    }

    private void process_inputs()
    {
        if (local_input_manager.get_mouse_pressed())
        {
            if (!_carried_object)
            {
                RaycastHit hit;
                // Change to camera direction
                Ray ray = camera.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out hit, range, 1 << 9))
                {
                    _carried_object = hit.transform.gameObject.GetComponent<interactable>();
                    _carried_object.interact();
                }
            }
            else
            {
                _carried_object.interact();
                _carried_object = null;
            }
        }
    }
}
