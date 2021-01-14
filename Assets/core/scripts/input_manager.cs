using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

public class input_manager : MonoBehaviour
{
	public static input_manager instance { get; set; }
	public keymap default_keymap { get { return _default_keymap.copy(); } }
	[Tooltip("The default keymap to use for any user.")]
	[SerializeField]
	private keymap _default_keymap = new keymap();
	private keymap _user_keymap = null;

	private List<input_record> _input_records = new List<input_record>();
	private int _input_read_time_ms = -1;

	public time_manager local_time_manager = null;

	// XXX : what does this do?
	public void Awake()
	{
		if (instance != null && instance != this)
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
		Cursor.lockState = CursorLockMode.None;
		_user_keymap = _default_keymap.copy();
	}

	void Update()
	{
		/* update the input records */

		if (_input_read_time_ms >= 0)
		{
			// input records have been read in FixedUpdate and are now stale
			_input_records.Clear();
			_input_read_time_ms = -1;
		}

		input_record current_input_record = new input_record();
		_user_keymap.record_inputs(current_input_record, local_time_manager);
		_input_records.Add(current_input_record);

		/* mouse cursor handling */

		if (Input.GetKeyDown(KeyCode.F2))
		{
			Cursor.lockState = Cursor.lockState == CursorLockMode.Locked ? CursorLockMode.None : CursorLockMode.Locked;
			Cursor.visible = Cursor.lockState == CursorLockMode.None;
		}
	}

	void FixedUpdate()
	{
		if (_input_read_time_ms < 0)
		{
			// input records have not yet been read
			_input_read_time_ms = local_time_manager.get_fixed_time_ms();
		}
		else
		{
			// input records were read on previous FixedUpdate
			_input_records.Clear();
			_input_read_time_ms = -1;
		}
	}

	public float get_axis_value(string name)
	{
		// XXX : for now, return latest axis input regardless of what update we're
		//		in. in the future, we may want to treat this case differently
		return _input_records[_input_records.Count - 1].get_axis_value(name);
	}

	public bool get_button_pressed(string name)
	{
		if (!Time.inFixedTimeStep)
		{
			input_record latest_record = _input_records[_input_records.Count - 1];
			return latest_record.get_button_value(name) == k_key_input_type.pressed;
		}
		else
		{
			for (int i = 0; i < _input_records.Count; i++)
			{
				if (_input_records[i].get_button_value(name) == k_key_input_type.pressed)
				{
					return true;
				}
			}
		}

		return false;
	}

	public bool get_button_down(string name)
	{
		if (!Time.inFixedTimeStep)
		{
			input_record latest_record = _input_records[_input_records.Count - 1];
			return latest_record.get_button_value(name) == k_key_input_type.pressed ||
				latest_record.get_button_value(name) == k_key_input_type.down;
		}
		else
		{
			for (int i = 0; i < _input_records.Count; i++)
			{
				if (_input_records[i].get_button_value(name) == k_key_input_type.pressed ||
						_input_records[i].get_button_value(name) == k_key_input_type.down)
				{
					return true;
				}
			}
		}

		return false;
	}

	public bool get_button_released(string name)
	{
		if (!Time.inFixedTimeStep)
		{
			input_record latest_record = _input_records[_input_records.Count - 1];
			return latest_record.get_button_value(name) == k_key_input_type.released;
		}
		else
		{
			for (int i = 0; i < _input_records.Count; i++)
			{
				if (_input_records[i].get_button_value(name) == k_key_input_type.released)
				{
					return true;
				}
			}
		}

		return false;
	}

	// TODO : as necessary, add functions that allow other scripts to access all input/time data
}

[System.Serializable]
public class keymap
{
	public string_axis_pair[] axes;
	public string_button_pair[] buttons;

	public void record_inputs(input_record inputs, time_manager tm)
	{
		for (int i = 0; i < axes.Length; i++)
		{
			inputs.record_axis(axes[i]);
		}

		for (int i = 0; i < buttons.Length; i++)
		{
			inputs.record_button(buttons[i]);
		}

		inputs.record_time_ms(tm.get_time_ms());
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

public class input_record
{
	private Dictionary<string, float> _axis_values = new Dictionary<string, float>();
	private Dictionary<string, k_key_input_type> _button_values = new Dictionary<string, k_key_input_type>();
	private int _input_time_ms = -1;

	public void record_axis(string_axis_pair pair)
	{
		_axis_values[pair.axis_name] = pair.axis.get_value();
	}

	public void record_button(string_button_pair pair)
	{
		_button_values[pair.button_name] = pair.button.get_value();
	}

	public void record_time_ms(int time_ms)
	{
		_input_time_ms = time_ms;
	}

	public float get_axis_value(string name)
	{
		return _axis_values[name];
	}

	public k_key_input_type get_button_value(string name)
	{
		return _button_values[name];
	}

	public int get_input_time_ms()
	{
		return _input_time_ms;
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
	// TODO : implement deadzones at the axis level
	// public float axis_deadzone = 0.0f;

	/// <summary>
	/// Returns the axis value.
	/// </summary>
	public float get_value()
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
			value = Input.GetAxisRaw(axis_name[i]);
			if (value != 0.0f)
			{
				return value;
			}
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
	public k_key_input_type get_value()
	{
		for (int i = 0; i < button.Count; i++)
		{
			k_key_input_type input_type =
				Input.GetKeyDown(button[i]) ? k_key_input_type.pressed :
				Input.GetKey(button[i]) ? k_key_input_type.down :
				Input.GetKeyUp(button[i]) ? k_key_input_type.released :
				k_key_input_type.none;

			if (input_type != k_key_input_type.none)
			{
				return input_type;
			}
		}

		return k_key_input_type.none;
	}
}
