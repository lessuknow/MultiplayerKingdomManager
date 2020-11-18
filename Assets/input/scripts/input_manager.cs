using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class input_manager : MonoBehaviour
{
	public keymap default_keymap { get { return _default_keymap.copy(); } }
	[SerializeField]
	private keymap _default_keymap = null;

	public float deadzone = 0.15f;

	void Start()
	{
		Cursor.lockState = CursorLockMode.Locked;
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
		return get_movement(_default_keymap);
	}

	public Vector2 get_movement(keymap user_keymap)
	{
		Vector2 movement_input = new Vector2(user_keymap.movement_x.get_value(deadzone), user_keymap.movement_z.get_value(deadzone));
		return movement_input.normalized;
	}

	public Vector2 get_camera()
	{
		return get_camera(_default_keymap);
	}

	public Vector2 get_camera(keymap user_keymap)
	{
		return new Vector2(user_keymap.camera_x.get_value(deadzone), user_keymap.camera_y.get_value(deadzone));
	}
}

[System.Serializable]
public class keymap
{
	public input_axis camera_x = null;
	public input_axis camera_y = null;
	public input_axis movement_x = null;
	public input_axis movement_z = null;

	public keymap copy()
	{
		return (keymap)MemberwiseClone();
	}
}

[System.Serializable]
public class input_axis
{
	public List<KeyCode> axis_up = new List<KeyCode>();
	public List<KeyCode> axis_down = new List<KeyCode>();
	public List<string> axis_name = new List<string>();

	/// <summary>
	/// Returns the axis value.
	/// </summary>
	public float get_value(float deadzone)
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
			debug.print_line(axis_value + " " + axis_name[i]);
			value += Mathf.Abs(axis_value) > deadzone ? axis_value : 0.0f;
		}
		return value;
	}
}
