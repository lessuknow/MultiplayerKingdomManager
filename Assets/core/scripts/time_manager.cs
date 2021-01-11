using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class time_manager : MonoBehaviour
{
	private int time_ms = 0;

	private int fixed_time_ms = 0;

	private int gameplay_time_ms = 0;

	void Update()
	{
		time_ms = time_ms + (int)(Time.deltaTime * 1000.0f);
		gameplay_time_ms = gameplay_time_ms + (int)(Time.deltaTime * 1000.0f);
	}

	void FixedUpdate()
	{
		fixed_time_ms = fixed_time_ms + (int)(Time.fixedDeltaTime * 1000.0f);
	}

	/// <summary>
	/// The time in milliseconds since the player started running the game.
	/// To be used for math and timestamps that are not affected by playtime and network.
	/// Updated in Update()
	/// </summary>
	/// <returns> The time in ms since start calculated from Update() </returns>
	public int get_time_ms()
	{
		return time_ms;
	}

	/// <summary>
	/// The fixed time in milliseconds since the player started running the game.
	/// To be used for math and timestamps that are not affected by playtime and network.
	/// Updated in FixedUpdate()
	/// </summary>
	/// <returns> The time in ms since start calculated from FixedUpdate() </returns>
	public int get_fixed_time_ms()
	{
		return fixed_time_ms;
	}

	/// <summary>
	/// The time in milliseconds since the player started the current play session.
	/// To be used for math and timestamps that are affected by playtime and network.
	/// Updated in Update()
	/// </summary>
	/// <returns> The time in ms since current play session start calculated from Update() </returns>
	public int get_gameplay_time_ms()
	{
		return gameplay_time_ms;
	}

	/// <summary>
	/// Set the current gameplay time.
	/// To be used on play session start, or to sync up to another player's gameplay time.
	/// </summary>
	public void set_gameplay_time_ms(int value)
	{
		if (value >= 0)
		{
			gameplay_time_ms = value;
		}
	}
}
