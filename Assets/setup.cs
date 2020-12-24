using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class setup : MonoBehaviour
{
    void Start()
    {
        Physics.IgnoreLayerCollision(10, 10, true);
    }
}
