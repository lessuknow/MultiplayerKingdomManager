using UnityEngine;
using UnityEngine.SceneManagement;

public class scene_prefab : MonoBehaviour
{
	public string scene_name = "";
	public bool auto_load = true;
	// XXX : feel free to add more as necessary

	enum k_scene_state
	{
		not_loaded,
		loaded,
		unloaded
	};
	private int scene_state = (int)k_scene_state.not_loaded;

	void Start()
	{
		if (auto_load)
		{
			load_scene();
		}
	}

	public void load_scene()
	{
		if (SceneManager.GetSceneByName(scene_name) == null)
		{
			SceneManager.LoadSceneAsync(scene_name, LoadSceneMode.Additive);
			scene_state = (int)k_scene_state.loaded;
			debug.print_line("Loading scene " + scene_name + ".");
		}
		else
		{
			debug.print_warning("Attempt made to load scene " + scene_name + " when it is already loaded.");
		}
	}

	public void unload_scene()
	{
		if (scene_state == (int)k_scene_state.loaded)
		{
			SceneManager.UnloadSceneAsync(scene_name);
			scene_state = (int)k_scene_state.unloaded;
			debug.print_line("Scheduling unload of scene " + scene_name + " after call to `unload_scene`.");
			// XXX : should this destroy itself after unload? should this stay around and allow a load to be called again?
		}
		else
		{
			debug.print_warning("Call to unload scene " + scene_name + " where an unload is already scheduled and/or completed.");
		}
	}

	void OnDestroy()
	{
		if (scene_state == (int)k_scene_state.loaded)
		{
			SceneManager.UnloadSceneAsync(scene_name);
			debug.print_line("Scheduling unload of scene " + scene_name + " on destroy.");
		}
	}
}
