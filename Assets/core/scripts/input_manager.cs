using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class input_manager : MonoBehaviour
{
	public static input_manager instance {get;set;}
	public keymap default_keymap { get { return _default_keymap.copy(); } }
	[Tooltip("The default keymap to use for any user.")]
	[SerializeField]
	private keymap _default_keymap = new keymap();
	private keymap _user_keymap = null;

	public time_manager local_time_manager = null;

	// XXX : what does this do?
	public void Awake()
	{
		if(instance != null && instance != this)
		{
			Destroy(this);
		}
		else
		{
			input_manager.instance = this;
		}
	}

	void Start()
	{
		Cursor.lockState = CursorLockMode.None;// Locked;	// FIXME : fixes mouse lock bug on game start
		_default_keymap.init_keymap();
		_user_keymap = _default_keymap.copy();
	}

	void Update()
	{
		if (Input.GetKeyDown(KeyCode.F2))
		{
			Cursor.lockState = Cursor.lockState == CursorLockMode.Locked ? CursorLockMode.None : CursorLockMode.Locked;
		}
	}

	public float get_axis_value(string name)
	{
		return _user_keymap.get_axis_value(name);
	}

	public bool get_button_pressed(string name)
	{
		return _user_keymap.get_button_value(name, k_key_input_type.pressed);
	}

	public bool get_button_down(string name)
	{
		return _user_keymap.get_button_value(name, k_key_input_type.down);
	}

	public bool get_button_released(string name)
	{
		return _user_keymap.get_button_value(name, k_key_input_type.released);
	}
}

[System.Serializable]
public class keymap
{
	public string_axis_pair[] axes;
	public string_button_pair[] buttons;

	public float gamepad_deadzone = 0.15f;

	private Dictionary<string, input_axis> _input_axes = new Dictionary<string, input_axis>();
	private Dictionary<string, input_button> _input_buttons = new Dictionary<string, input_button>();

	private List<int> _input_times_ms = new List<int>();
	private List<Dictionary<string, float>> _axis_values = new List<Dictionary<string, float>>();
	private List<Dictionary<string, k_key_input_type>> _button_values = new List<Dictionary<string, k_key_input_type>>();

	public void init_keymap()
	{
		_input_axes.Clear();
		_input_buttons.Clear();

		for (int i = 0; i < axes.Length; i++)
		{
			_input_axes.Add(axes[i].axis_name, axes[i].axis);
		}

		for (int i = 0; i < buttons.Length; i++)
		{
			_input_buttons.Add(buttons[i].button_name, buttons[i].button);
		}
	}

	public void record_inputs(time_manager tm)
	{
		_input_times_ms.Add(tm.get_time_ms());
	}

	public float get_axis_value(string name)
	{
		return _input_axes[name].get_value(gamepad_deadzone);
	}

	public bool get_button_value(string name, k_key_input_type input_type)
	{
		return _input_buttons[name].get_value(input_type);
	}

	public keymap copy()
	{
		// first, clone shallow class members (ints, floats, etc)
		keymap keymap_copy = (keymap)MemberwiseClone();

		// copy object data
		// TODO : actually copy the object data here lol


		return keymap_copy;
	}
}

[System.Serializable]
public class string_axis_pair
{
	public string axis_name = "";
	public input_axis axis = null;
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
public class string_button_pair
{
	public string button_name;
	public input_button button;
}

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
