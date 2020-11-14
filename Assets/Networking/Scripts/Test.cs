using UnityEngine;
using Mirror;

public class Test : NetworkBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("E");
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            this.transform.position += new Vector3(0, 0.25f, 0);
        }
    }
}