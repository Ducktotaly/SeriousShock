using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class MapEntity : MonoBehaviour
{
    public List<Car> Cars = new();
    public Player player;

    private Car activeCar;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            TryChangeCar();
        }
    }

    private void TryChangeCar()
    {
        if (activeCar == null)
        {
            foreach (Car car in Cars)
            {
                if (Vector3.Distance(player.transform.position, car.transform.position) < 3f)
                {
                    activeCar = car;
                    player.CameraView.SetTarget(car.transform,true);
                    player.gameObject.SetActive(false);
                    car.SetActive(true);

                }
            }
        }
        else
        {
            if (activeCar.Spawn.IsClear == false) { return; }

            player.transform.position = activeCar.Spawn.transform.position;
            player.CameraView.SetTarget(player.transform, true);
            player.gameObject.SetActive(true);

            activeCar.SetActive(false);
            activeCar = null;
        }
    }
}
