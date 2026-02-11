using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Animator Animator;
    public CharacterController Controller;
    public float defaultSpeed;
    public float runSpeed;
    public float CameraSpeed;
    public Transform FlyCamera;

    private float defaultAnimSpeed = 1f;
    private Vector3 offset;

    private void Start()
    {
        offset = FlyCamera.transform.position - transform.position;
    }
    // Update is called once per frame
    void Update()
    {
        var x = Input.GetAxis("Horizontal");
        var y = Input.GetAxis("Vertical");
        var motion = new Vector3(x, 0, y);
        var animSpeed = defaultAnimSpeed;

        FlyCamera.position = Vector3.Lerp(FlyCamera.position, transform.position + offset,Time.deltaTime*CameraSpeed);

        if (motion.magnitude != 0)
        {
            var Speed = defaultSpeed;
            if (Input.GetKey(KeyCode.LeftShift))
            {
                Speed = runSpeed;
                animSpeed = 2f;
            }
            setAnim(false,animSpeed);
            var targetRot = Quaternion.LookRotation(motion);
            transform.rotation = Quaternion.Slerp(transform.rotation,targetRot,15f *  Time.deltaTime);
            Controller.Move(motion * Time.deltaTime * Speed);
            return;
        }
        setAnim(true,animSpeed);
        
    }
    private void setAnim(bool IsIdle, float animSpeed)
    {
        Animator.speed = animSpeed;
        Animator.SetBool("isIdle", IsIdle);
        Animator.SetBool("isWalk", !IsIdle);
    }
}
