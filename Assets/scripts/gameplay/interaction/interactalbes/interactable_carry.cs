using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// An object that can be carried when interacted with.
/// </summary>
public class interactable_carry : interactable
{
    private Rigidbody _rb;

    // The position this object wants to go to.
    public Vector3 _target_position;

    // If the object has been interacted with yet. Might be worth pulling up to parent?
    protected bool _interacted_with;

    void Start()
    {
        this._rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        if(_interacted_with)
        {
            _rb.velocity = Vector3.Lerp(transform.position, _target_position, Time.deltaTime);
            _rb.MovePosition(_target_position);
        }
    }

    public void set_goal_position(Vector3 pos)
    {
        _target_position = pos;
    }

    public override void interact()
    {
        if(!_interacted_with)
        {
            enable_interaction();
        }
        else
        {
            disable_interaction();
        }
    }

    // Called when picking up.
    private void enable_interaction()
    {
        _rb.useGravity = false;
        _interacted_with = true;
        debug.print_line("Let me go please!");
    }

    // Called when letting go.
    private void disable_interaction()
    {
        _rb.useGravity = true;
        _interacted_with = false;
        debug.print_line("Praise be!");
    }
}
