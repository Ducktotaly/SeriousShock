using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class CameraView : MonoBehaviour
{
    public float CameraSpeed;
    public Transform Target;
    private Vector3 offset;
    private bool isFixed;

    public void SetOffset(Vector3 pos)
    {
        offset = transform.position - pos;
    }

    public void SetTarget(Transform target,bool fix)
    {
        Target = target;
        isFixed = fix;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (isFixed == false) { return; }
        transform.position = Vector3.Lerp(transform.position, Target.position + offset, Time.fixedDeltaTime * CameraSpeed);
    }
    void Update()
    {
        if (isFixed == true) { return; }
        transform.position = Vector3.Lerp(transform.position, Target.position + offset, Time.deltaTime * CameraSpeed);
    }
}
