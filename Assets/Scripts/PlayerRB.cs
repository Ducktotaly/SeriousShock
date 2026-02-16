using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class PlayerRB : MonoBehaviour
{
    public Animator Animator;
    public Rigidbody rigidBody;
    public float defaultSpeed;
    public float runSpeed;
    public float CameraSpeed;
    public Transform FlyCamera;

    private float defaultAnimSpeed = 1f;
    private Vector3 offset;
    private Vector3 motion = new Vector3(0,0,0);
    private float Speed = 0;

    private void Start()
    {
        offset = FlyCamera.transform.position - transform.position;
    }
    // Update is called once per frame
    void Update()
    {
        var x = Input.GetAxis("Horizontal");
        var y = Input.GetAxis("Vertical");
        motion = new Vector3(x, 0, y);
        var animSpeed = defaultAnimSpeed;

        FlyCamera.position = Vector3.Lerp(FlyCamera.position, transform.position + offset, Time.deltaTime * CameraSpeed);

        if (motion.magnitude != 0)
        {
            Speed = defaultSpeed;
            if (Input.GetKey(KeyCode.LeftShift))
            {
                Speed = runSpeed;
                animSpeed = 2f;
            }
            setAnim(false, animSpeed);
            var targetRot = Quaternion.LookRotation(motion);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, 15f * Time.deltaTime);
            return;
        }
        setAnim(true, animSpeed);

    }

    private void FixedUpdate()
    {
        Vector3 Velocity = motion.normalized * Speed;

        Vector3 velocityChange = Velocity - rigidBody.velocity;
        velocityChange.y = 0;

        rigidBody.AddForce(velocityChange, ForceMode.VelocityChange);
    }


    private void setAnim(bool IsIdle, float animSpeed)
    {
        Animator.speed = animSpeed;
        Animator.SetBool("isIdle", IsIdle);
        Animator.SetBool("isWalk", !IsIdle);
    }
}
