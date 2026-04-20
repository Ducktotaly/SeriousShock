using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class MapEntity : MonoBehaviour
{
    public ShopManager ShopManager;
    public Player player;

    private Car activeCar;


    private void Start()
    {
        ShopManager.SpawnCar();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            TryChangeCar();
        }
    }

    public Transform GetActivePlayer()
    {
        return activeCar == null ? player.transform : activeCar.transform;
    }

    private void TryChangeCar()
    {
        var selectedCar = ShopManager.GetSelectedCar();
        if (activeCar == null)
        {
            if (Vector3.Distance(player.transform.position, selectedCar.transform.position) < 3f)
            {
                activeCar = selectedCar;
                player.CameraView.SetTarget(selectedCar.transform,true);
                player.gameObject.SetActive(false);
                selectedCar.SetActiveCar(true);

            }
        }
        else
        {
            if (activeCar.Spawn.IsClear == false) { return; }

            player.transform.position = activeCar.Spawn.transform.position;
            player.CameraView.SetTarget(player.transform, true);
            player.gameObject.SetActive(true);

            activeCar.SetActiveCar(false);
            activeCar = null;
        }
    }
}
