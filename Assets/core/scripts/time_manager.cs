using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class time_manager : MonoBehaviour
{
	public int default_framerate = 60;

	private int _time_ms = 0;

	private int _fixed_time_ms = 0;

	private int _gameplay_time_ms = 0;

	void Start()
	{
		set_framerate(default_framerate);
	}

	void Update()
	{
		_time_ms = _time_ms + (int)(Time.deltaTime * 1000.0f);
		_gameplay_time_ms = _gameplay_time_ms + (int)(Time.deltaTime * 1000.0f);
	}

	void FixedUpdate()
	{
		_fixed_time_ms = _fixed_time_ms + (int)(Time.fixedDeltaTime * 1000.0f);
	}

	/// <summary>
	/// Set the framerate of the application.
	/// </summary>
	public void set_framerate(int framerate)
	{
		if (framerate < 1)
		{
			debug.print_warning("Value framerate cannot be set to any value less than 1");
			return;
		}

		Application.targetFrameRate = framerate;
	}

	/// <summary>
	/// The time in milliseconds since the player started running the game.
	/// To be used for math and timestamps that are not affected by playtime and network.
	/// Updated in Update()
	/// </summary>
	/// <returns> The time in ms since start calculated from Update() </returns>
	public int get_time_ms()
	{
		return _time_ms;
	}

	/// <summary>
	/// The fixed time in milliseconds since the player started running the game.
	/// To be used for math and timestamps that are not affected by playtime and network.
	/// Updated in FixedUpdate()
	/// </summary>
	/// <returns> The time in ms since start calculated from FixedUpdate() </returns>
	public int get_fixed_time_ms()
	{
		return _fixed_time_ms;
	}

	/// <summary>
	/// The time in milliseconds since the player started the current play session.
	/// To be used for math and timestamps that are affected by playtime and network.
	/// Updated in Update()
	/// </summary>
	/// <returns> The time in ms since current play session start calculated from Update() </returns>
	public int get_gameplay_time_ms()
	{
		return _gameplay_time_ms;
	}

	/// <summary>
	/// Set the current gameplay time.
	/// To be used on play session start, or to sync up to another player's gameplay time.
	/// </summary>
	public void set_gameplay_time_ms(int value)
	{
		if (value >= 0)
		{
			_gameplay_time_ms = value;
		}
	}
}
