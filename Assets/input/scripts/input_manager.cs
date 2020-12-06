using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class input_manager : MonoBehaviour
{
	public keymap default_keymap { get { return _default_keymap.copy(); } }
	[Tooltip("The default keymap to use for any user.")]
	[SerializeField]
	private keymap _default_keymap = null;
	private keymap _user_keymap = null;

	public float gamepad_deadzone = 0.15f;

	void Start()
	{
		Cursor.lockState = CursorLockMode.Locked;
		_user_keymap = _default_keymap.copy();
	}

	void Update()
	{
		if (Input.GetKeyDown(KeyCode.F2))
		{
			Cursor.lockState = Cursor.lockState == CursorLockMode.Locked ? CursorLockMode.None : CursorLockMode.Locked;
		}
	}

	public Vector2 get_movement()
	{
		Vector2 movement_input = new Vector2(_user_keymap.movement_x.get_value(gamepad_deadzone), _user_keymap.movement_z.get_value(gamepad_deadzone));
		return movement_input.normalized;
	}

	public Vector2 get_camera()
	{
		return new Vector2(_user_keymap.camera_x.get_value(gamepad_deadzone), _user_keymap.camera_y.get_value(gamepad_deadzone));
	}

	public bool get_jump_pressed()
	{
		return _user_keymap.jump.get_value(k_key_input_type.pressed);
	}

	public bool get_jump_down()
	{
		return _user_keymap.jump.get_value(k_key_input_type.down);
	}

	public bool get_jump_released()
	{
		return _user_keymap.jump.get_value(k_key_input_type.released);
	}
}

[System.Serializable]
public class keymap
{
	// TODO : eventually change these to lists of action/input pairs.
	//		so like "camera_look"/input_axis and "jump"/input_button
	public input_axis camera_x = null;
	public input_axis camera_y = null;
	public input_axis movement_x = null;
	public input_axis movement_z = null;

	public input_button jump = null;

	public keymap copy()
	{
		return (keymap)MemberwiseClone();
	}
}

[System.Serializable]
public class input_axis
{
	[Tooltip("Any keys or buttons to treat as the up axis.")]
	public List<KeyCode> axis_up = new List<KeyCode>();
	[Tooltip("Any keys or buttons to treat as the down axis.")]
	public List<KeyCode> axis_down = new List<KeyCode>();
	[Tooltip("An axis to reference by name in Unity's input manager.")]
	public List<string> axis_name = new List<string>();

	/// <summary>
	/// Returns the axis value.
	/// </summary>
	public float get_value(float gamepad_deadzone)
	{
		float value = 0.0f;
		for (int i = 0; i < axis_up.Count; i++)
		{
			value += Input.GetKey(axis_up[i]) ? 1 : 0;
		}
		if (value != 0.0f)
		{
			return Mathf.Clamp(value, -1, 1);
		}

		for (int i = 0; i < axis_down.Count; i++)
		{
			value += Input.GetKey(axis_down[i]) ? -1 : 0;
		}
		if (value != 0.0f)
		{
			return Mathf.Clamp(value, -1, 1);
		}

		for (int i = 0; i < axis_name.Count; i++)
		{
			float axis_value = Input.GetAxisRaw(axis_name[i]);
			value += Mathf.Abs(axis_value) > gamepad_deadzone ? axis_value : 0.0f;
		}
		return value;
	}
}

public enum k_key_input_type
{
	none,
	pressed,
	down,
	released
};

[System.Serializable]
public class input_button
{
	[Tooltip("The keys or buttons to use as input.")]
	public List<KeyCode> button = new List<KeyCode>();

	/// <summary>
	/// Returns the button value.
	/// </summary>
	public bool get_value(k_key_input_type input_type)
	{
		bool value = false;

		for (int i = 0; i < button.Count; i++)
		{
			if (input_type == k_key_input_type.pressed)
			{
				value = value || Input.GetKeyDown(button[i]);
			}
			else if (input_type == k_key_input_type.down)
			{
				value = value || Input.GetKey(button[i]);
			}
			else if(input_type == k_key_input_type.released)
			{
				value = value || Input.GetKeyUp(button[i]);
			}
		}

		return value;
	}
}
