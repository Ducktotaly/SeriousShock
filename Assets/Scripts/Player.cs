using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Animator Animator;
    public CharacterController Controller;
<<<<<<< Updated upstream
    public float Speed;
    public float CameraSpeed;
    public Transform FlyCamera;

    private Vector3 offset;
=======
    public float defaultSpeed;
    public float runSpeed;
    public CameraView CameraView;
    public float Speed;

    private float defaultAnimSpeed = 1f;
>>>>>>> Stashed changes

    private void Awake()
    {
        CameraView.SetOffset(transform.position);
        CameraView.SetTarget(transform, true);
    }
    // Update is called once per frame
    void Update()
    {
        var x = Input.GetAxis("Horizontal");
        var y = Input.GetAxis("Vertical");
        var motion = new Vector3(x, 0, y);

        if (motion.magnitude != 0)
        {
            setAnim(false);
            var targetRot = Quaternion.LookRotation(motion);
            transform.rotation = Quaternion.Slerp(transform.rotation,targetRot,15f *  Time.deltaTime);
            Controller.Move(motion * Time.deltaTime * Speed);
            return;
        }
        setAnim(true);
        
    }
    private void setAnim(bool IsIdle)
    {
        Animator.SetBool("isIdle", IsIdle);
        Animator.SetBool("isWalk", !IsIdle);
    }
}
