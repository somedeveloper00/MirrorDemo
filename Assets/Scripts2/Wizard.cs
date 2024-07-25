using System.Threading.Tasks;
using Mirror;
using UnityEngine;

public sealed class Wizard : NetworkBehaviour
{
    public float maxHp = 200;
    [SyncVar] public float hp;
    public float speed;
    public GameObject missilePrefab;
    public Transform handPos;
    [SerializeField] private Animator animator;
    [SerializeField] private float shootDelay;

    public void ShootRpc()
    {
        Debug.LogFormat("shot");
        animator.SetTrigger("shoot");
        ShootDelayed();
    }

    [Command]
    private void SpawnMissile()
    {
        Debug.LogFormat("spawning missile");
        var obj = Instantiate(missilePrefab, handPos.position, transform.rotation);
        NetworkServer.Spawn(obj);
    }

    public async void ShootDelayed()
    {
        float targetTime = Time.time + shootDelay;
        while (Time.time < targetTime)
        {
            await Task.Yield();
        }
        SpawnMissile();
    }

    private void Update()
    {
        if (isLocalPlayer)
        {
            // move
            var rot = Camera.main.transform.rotation;
            var moveX = Input.GetAxis("Horizontal");
            var moveZ = Input.GetAxis("Vertical");
            if (moveX != 0 || moveZ != 0)
            {
                var dir = rot * Vector3.forward * moveZ + rot * Vector3.right * moveX;
                var r = Quaternion.LookRotation(dir);
                var p = transform.position + speed * Time.deltaTime * dir;
                transform.SetPositionAndRotation(p, r);
                animator.SetTrigger("walking");
            }
            else
            {
                animator.SetTrigger("idle");
            }

            // speed
            animator.SetBool("running", Input.GetKeyDown(KeyCode.LeftShift));

            // shoot
            if (Input.GetKeyDown(KeyCode.Space))
            {
                ShootRpc();
            }
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (isLocalPlayer && other.gameObject.CompareTag("Missile"))
        {
            hp -= 10;
            if (hp <= 0)
            {
                Die();
            }
        }
    }

    private void Die()
    {
        animator.SetTrigger("die");
    }
}