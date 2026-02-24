using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DeliverPoint : MonoBehaviour
{
    public bool IsGet;

    public void ActivePoint()
    {
        gameObject.SetActive(true);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            gameObject.SetActive(false);
            if (IsGet)
            {
                OnGet();
            }
            else
            {
                OnGive();
            }
        }
    }

    private void OnGet()
    {
        CoreManager.Instance.OpenMissionMenu();
    }

    private void OnGive() 
    {
        CoreManager.Instance.GetOrder();
    }
}
