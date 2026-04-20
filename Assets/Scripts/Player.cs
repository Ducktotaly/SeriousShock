using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public GameObject rig;
    public Animator Animator;
    public CharacterController Controller;

    public float Speed;
    public CameraView CameraView;

    private bool onDeath;

    private void Awake()
    {
        CameraView.SetOffset(transform.position);
        CameraView.SetTarget(transform, true);
    }
    // Update is called once per frame
    void Update()
    {
       if (onDeath) {return;}
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

    public float GetPlayerSpeed()
    {
        return Speed;
    }

    private void OnCollisionEnter(Collision collision)
    {
        CoreManager.Instance.GetDamage(collision.transform.tag);
    }
    private void SetRagdoll()
    {
        if (onDeath)
        {
            return;
        }
        rig.SetActive(true);
        Animator.enabled = false;
        CoreManager.Instance.DeathAnim();
    }

    public void OnPlayerDeath()
    {
        SetRagdoll();
        onDeath = true;
    }

    private void setAnim(bool IsIdle)
    {
        Animator.SetBool("isIdle", IsIdle);
        Animator.SetBool("isWalk", !IsIdle);
    }
}
