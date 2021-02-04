using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// State that stores all relevant evnt data.
/// Fix spelling to event.
/// </summary>
public class event_state: MonoBehaviour
{
    public string sky_color { get; private set; }

    [SerializeField]
    public List<event_parameter> parameters = new List<event_parameter>();

    private List<(int, event_runner)> current_events { get; set;  }
    private List<int> occured_events { get; set; }

    // store current opened gameobjects.    
    public void Start()
    {
        sky_color = "red";
        current_events = new List<(int, event_runner)>();
        occured_events = new List<int>();
    }

    public void add_event(int id, event_runner event_runner)
    {
        current_events.Add((id, event_runner));
    }

    public void end_event(int id)
    {
        bool event_exists = current_events.FindIndex(evnt => evnt.Item1 == id) != -1;
        if (!event_exists)
        {
            debug.print_error($"event with id of {id} is not running");
        }

        if (occured_events.Contains(id))
        {
            debug.print_error($"event with id of {id} has already occured");
        }

        event_runner runner = current_events.Find(evnt => evnt.Item1 == id).Item2;
        runner.end_event();

        current_events.RemoveAll(evnt => evnt.Item1 == id);
        occured_events.Add(id);
    }

    public void instantiate_event_parameters(List<event_entry> entries)
    {
        foreach(event_entry entry in entries)
        {
            if(!parameters.Any(param => param.id == entry.id))
            {
                parameters.Add(new event_parameter()
                {
                    id = entry.id,
                    parameter = ""
                });
            }
        }
    }

    public string get_event_parameter(int id)
    {
        return parameters.Find(parameter => parameter.id == id).parameter;
    }

    public void set_event_parameter(int id, string value)
    {
        event_parameter parameter = parameters.FirstOrDefault(param => param.id == id);

        if(parameter == null)
        {
            parameters.Add(new event_parameter()
            {
                id = id,
                parameter = value
            });
        } 
        else
        {
            parameter.parameter = value;
        }
    }
}
