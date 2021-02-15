using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class used to run events.
/// </summary>
public abstract class event_runner : MonoBehaviour
{
    public abstract void start_event(string value);

    public abstract void end_event();
}
