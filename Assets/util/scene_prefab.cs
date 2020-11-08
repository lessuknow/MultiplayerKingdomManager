using UnityEngine;
using UnityEngine.SceneManagement;

public class scene_prefab : MonoBehaviour
{
	public string scene_name = "";
	// XXX : feel free to add more possible ways to load as necessary

	enum k_scene_state
	{
		not_loaded,
		loaded,
		unloaded
	};
	private int scene_state = (int)k_scene_state.not_loaded;

	void Start()
	{
		SceneManager.LoadSceneAsync(scene_name, LoadSceneMode.Additive);
		scene_state = (int)k_scene_state.loaded;
	}

	public void unload_scene()
	{
		if (scene_state == (int)k_scene_state.loaded)
		{
			SceneManager.UnloadSceneAsync(scene_name);
			scene_state = (int)k_scene_state.unloaded;
			Debug.Log("Scheduling unload of scene " + scene_name + " after call to `unload_scene`.");
		}
		else
		{
			Debug.LogWarning("Call to unload scene " + scene_name + " where an unload is already scheduled and/or completed.");
		}
	}

	void OnDestroy()
	{
		if (scene_state == (int)k_scene_state.loaded)
		{
			SceneManager.UnloadSceneAsync(scene_name);
			Debug.Log("Scheduling unload of scene " + scene_name + " on destroy.");
		}
	}
}
