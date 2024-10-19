using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wheel : MonoBehaviour
{
    public new WheelCollider collider;

    private void LateUpdate()
    {
        collider.GetWorldPose(out Vector3 pos, out Quaternion rotation);
        transform.position = pos;
        transform.rotation = rotation;
    }
}