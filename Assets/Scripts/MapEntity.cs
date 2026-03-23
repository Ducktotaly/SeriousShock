using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class MapEntity : MonoBehaviour
{
    public List<GameObject> CARSPREFABS = new();
    public Transform carSpawnPoint;
    public List<Car> Cars = new();
    public Player player;

    private Car activeCar;

    private void Start()
    {
        SpawnCar();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            TryChangeCar();
        }
    }

    private void SpawnCar()
    {
        var carId = PlayerPrefs.GetInt("SelectedCar");
        if (carId == 0) { return; }
        var spawnedCar = Instantiate(CARSPREFABS[carId - 1]);
        spawnedCar.transform.position = carSpawnPoint.position;
        Cars.Add(spawnedCar.GetComponent<Car>());
       
    }

    public Transform GetActivePlayer()
    {
        return activeCar == null ? player.transform : activeCar.transform;
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
