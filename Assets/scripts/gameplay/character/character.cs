using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class character : MonoBehaviour
{
	public virtual float get_axis_value(string name)
	{
		return 0.0f;
	}

	public virtual bool get_button_pressed(string name)
	{
		return false;
	}

	public virtual bool get_button_down(string name)
	{
		return false;
	}

	public virtual bool get_button_released(string name)
	{
		return false;
	}
}
