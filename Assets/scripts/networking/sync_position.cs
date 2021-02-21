using UnityEngine;
using Mirror;

// Blantently stole and adjusted from
// https://gamedev.stackexchange.com/questions/144933/unity-manually-sync-location-of-players-from-server-to-client
public class sync_position : NetworkBehaviour
{
    [SerializeField]
    private GameObject playerBody = null;

    [SerializeField]
    private Rigidbody physicsRoot = null;

    void Start()
    {
    }

    void Update()
    {
        if (isLocalPlayer)
        {
            CmdSyncPos(
                transform.position,
                transform.localRotation,
                playerBody.transform.localRotation,
                physicsRoot.velocity);
        }
    }

    // Send position to the server and run the RPC for everyone, including the server. 
    [Command]
    protected void CmdSyncPos(Vector3 localPosition, Quaternion localRotation, Quaternion bodyRotation, Vector3 velocity)
    {
        RpcSyncPos(localPosition, localRotation, bodyRotation, velocity);
    }

    // For each player, transfer the position from the server to the client, and set it as long as it's not the local player. 
    [ClientRpc]
    void RpcSyncPos(Vector3 localPosition, Quaternion localRotation, Quaternion bodyRotation, Vector3 velocity)
    {
        if (playerBody == null)
        {
            return;
        }
        if (!isLocalPlayer)
        {
            transform.position = localPosition;
            transform.localRotation = localRotation;
            playerBody.transform.localRotation = bodyRotation;
            physicsRoot.velocity = velocity;
        }
    }
}