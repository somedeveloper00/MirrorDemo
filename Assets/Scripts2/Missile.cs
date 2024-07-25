
using Mirror;
using UnityEngine;

public sealed class Missile : NetworkBehaviour
{
    public float speed;

    private void Update()
    {
        transform.position += transform.forward * speed * Time.deltaTime;
    }
}