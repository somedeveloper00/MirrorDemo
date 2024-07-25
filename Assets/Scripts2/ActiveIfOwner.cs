using Mirror;
using UnityEngine;

public sealed class ActiveIfOwner : MonoBehaviour
{
    public NetworkIdentity identity;

    private void Update()
    {
        if (!identity.isLocalPlayer)
        {
            gameObject.SetActive(false);
        }
    }
}