using UnityEngine;
using Mirror;

public class player_network : NetworkBehaviour
{
    void Start()
    {
        Debug.Log("Spawnded");
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            this.transform.position += new Vector3(0, 0.25f, 0);
        }
    }

    public override void OnStartLocalPlayer()
    {
        Debug.Log("Wowza!");
    }
}