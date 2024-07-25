using UnityEngine;

public sealed class FollowCamera : MonoBehaviour
{
    public Transform target;
    public float rotSpeed;
    public Vector3 offset;
    public float distance;

    private float _rot;

    private void Update()
    {
        var inpX = Input.GetAxis("Mouse X");
        _rot += inpX * Time.deltaTime * rotSpeed;
        var targetPos = target.position;
        var pos = targetPos;
        pos.x += Mathf.Sin(_rot) * distance;
        pos.z += Mathf.Cos(_rot) * distance;
        transform.rotation = Quaternion.LookRotation(targetPos - pos);
        pos += offset;
        transform.position = pos;
    }
}