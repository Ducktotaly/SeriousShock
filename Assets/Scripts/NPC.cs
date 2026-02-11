using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering;
using UnityEngine.UIElements;

public class NPC : MonoBehaviour
{
    public GameObject rig;
    public Animator animator;
    public NavMeshAgent agent;

    private bool onChangeState;
    private bool onDeath;

    private void Start()
    {
        setAnim(false);
        SetPosition();
    }

    private void SetPosition()
    { 
        var point = NavMesh.CalculateTriangulation();
        var randomPoint = Random.Range(0, point.vertices.Length);
        agent.SetDestination(point.vertices[randomPoint]);
    }

    private void SetRagdoll()
    {
        if (onDeath)
        {
            return;
        }
        agent.enabled = false;
        rig.SetActive(true);
        animator.enabled = false;
        Destroy(this.gameObject, 15f);
        onChangeState = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        SetRagdoll();
        onDeath = true;
    }

    private IEnumerator OnMoveEnd()
    {
        yield return new WaitForSeconds(5f);
        SetPosition();
        onChangeState = false;
        setAnim(false);
    }

    private void Update()
    {
        if (onChangeState) { return; }
        if (onDeath) { return; }
        if (agent.remainingDistance < 0.1f)
        {
            onChangeState = true;
            setAnim(true);
            StartCoroutine(OnMoveEnd());
        }
    }
    private void setAnim(bool IsIdle)
    {
        animator.SetBool("isIdle", IsIdle);
        animator.SetBool("isWalk", !IsIdle);
    }
}
