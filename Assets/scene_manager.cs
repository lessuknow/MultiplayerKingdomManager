using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scene_manager : MonoBehaviour
{
	// XXX : This manager is not very robust for now. In the future it
	//			will manage more of the loading flow, but for now, it's
	//			best to keep it on the simple side.

	[Tooltip("The scene_prefab for the gameplay level.")]
	[SerializeField]
	scene_prefab level_scene_prefab = null;

	[Tooltip("The scene_prefab for the network level.")]
	[SerializeField]
	scene_prefab network_scene_prefab = null;

	bool tried_load = false;
	
	void Update()
	{
		if (!tried_load)
		{
			// we load the scenes in update, giving time for the debug gui to be created
			level_scene_prefab.load_scene();
			network_scene_prefab.load_scene();

			tried_load = true;

			debug.print_line("scene_manager has loaded the scenes " + level_scene_prefab.scene_name + ".unity and " + network_scene_prefab.scene_name + ".unity");
		}
	}
}
