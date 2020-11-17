using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class input_manager : MonoBehaviour
{
	public keymap default_keymap { get { return default_keymap_ref; } }
	[SerializeField]
	private keymap default_keymap_ref = null;

	void Start()
	{
		
	}

	void Update()
	{
		
	}
}

[System.Serializable]
public class keymap
{
	public input_axis camera_x = null;
	public input_axis camera_y = null;
	public input_axis movement_x = null;
	public input_axis movement_z = null;
}

[System.Serializable]
public class input_axis
{
	public List<KeyCode> axis_up = new List<KeyCode>();
	public List<KeyCode> axis_down = new List<KeyCode>();
	public List<string> axis_name = new List<string>();
}
