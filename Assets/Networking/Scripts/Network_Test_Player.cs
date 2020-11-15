using UnityEngine;
using Mirror;

public class Network_Test_Player : NetworkBehaviour
{
    void Start()
    {
        Debug.Log("Spawned");
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            this.transform.position += new Vector3(0, 0.25f, 0);
        }
    }
}