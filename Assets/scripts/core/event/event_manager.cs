using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class event_manager : MonoBehaviour
{
    public static event_manager instance { get; set; }

    [SerializeField]
    public event_state state;

    [SerializeField]
    public List<event_entry> event_entries;

    private void Awake()
    {
        if(instance != null && instance != this)
        {
            Destroy(this);
        }

        instance = this;
    }

    private void Start()
    {
        state.instantiate_event_parameters(event_entries);
    }

    public void start_event(int id)
    {
        event_entry entry = event_entries.Find(event_entry => event_entry.id == id);

        GameObject obj = Instantiate(entry.event_object);
        event_runner runner = obj.GetComponent<event_runner>();
        runner.start_event(state.get_event_parameter(id));

        state.add_event(id, runner);
    }

    public void end_event(int id)
    {
        UnityEvent end_event = event_entries.Find(entry => entry.id == id).event_action;

        state.end_event(id);
        end_event.Invoke();
    }
}
