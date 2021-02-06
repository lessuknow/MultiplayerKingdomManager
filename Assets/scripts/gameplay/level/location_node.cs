using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class location_node : MonoBehaviour
{
	public string name = "unnamed";
	
	// TODO : use collision boxes to define the area, a radius is too broad
	public float radius = 3.0f;

	// TODO : each area node should have references to interactable objects in its area

	// XXX : temporary priority value to help with AI moving about randomly
	public int ai_value = 0;
}
