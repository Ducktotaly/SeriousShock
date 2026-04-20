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

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (IsGet) 
            {
                CloseMenu();
            }
        }
    }

    private void CloseMenu()
    {
        CoreManager.Instance.CloseMissionMenu();
    }

    private void OnGet()
    {
        CoreManager.Instance.OpenMissionMenu();
    }

    private void OnGive() 
    {
        CoreManager.Instance.GiveOrder();
    }
}
