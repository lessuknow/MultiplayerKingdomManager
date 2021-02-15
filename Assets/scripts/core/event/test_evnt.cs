using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test_evnt: MonoBehaviour, IObserver<event_state>
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.U))
        {
            event_manager.instance.start_event(1);
        }
        if (Input.GetKeyDown(KeyCode.V))
        {
            event_manager.instance.end_event(1);
        }
    }

    public void SayHi()
    {
        debug.print_line("hi");
    }

    public void OnCompleted()
    {
        throw new NotImplementedException();
    }

    public void OnError(Exception error)
    {
        throw new NotImplementedException();
    }

    public void OnNext(event_state value)
    {
        debug.print_line(value.sky_color);
    }
}
