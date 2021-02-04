using System;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class event_entry
{
    public int id;
    public GameObject event_object;
    public UnityEvent event_action;
}