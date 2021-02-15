using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class runner_test : event_runner
{
    public override void end_event()
    {
        debug.print_line("Event ended.");
    }

    public override void start_event(string value)
    {
        debug.print_line($"Event started with value {value}");
    }
}
