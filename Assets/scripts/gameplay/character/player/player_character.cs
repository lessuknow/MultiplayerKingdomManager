using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player_character : character
{
	public input_manager local_input_manager = null;

	public override float get_axis_value(string name)
	{
		return local_input_manager.get_axis_value(name);
	}

	public override bool get_button_pressed(string name)
	{
		return local_input_manager.get_button_pressed(name);
	}

	public override bool get_button_down(string name)
	{
		return local_input_manager.get_button_down(name);
	}

	public override bool get_button_released(string name)
	{
		return local_input_manager.get_button_released(name);
	}
}
