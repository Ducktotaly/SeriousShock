using System;
using System.Collections;
using System.Collections.Generic;
using System.Transactions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
    public Button BuyButton;
    public Button ExitButton;
    public Button SelectButton;
    public Button DeselectButton;
    public Button LeftArrow;
    public Button RightArrow;
    public TextMeshProUGUI NameText;
    public TextMeshProUGUI CostText;
    public Slider Speed;
    public List<CarData> Cars;
    public List<Car> CARSPREFABS = new();
    public Transform CarSpawnPoint;

    private Car selectedCar;
    private int index;

    private void Start()
    {
        RightArrow.onClick.AddListener(() => SwitchCar(true));
        LeftArrow.onClick.AddListener(() => SwitchCar(false));
        BuyButton.onClick.AddListener(() => BuyCar());
        SelectButton.onClick.AddListener(() => SelectCar());
        ExitButton.onClick.AddListener(() => 
        { 
            CoreManager.Instance.CloseShop();
            SpawnCar();
        });
    }

    private void BuyCar()
    {
        var money = PlayerPrefs.GetInt("Money");
        if (money < Cars[index].Cost) { return; }
        
        PlayerPrefs.SetInt("Money", money - Cars[index].Cost);
        PlayerPrefs.SetInt($"Car{index}", 1);
        BuyButton.gameObject.SetActive(false);
        SelectButton.gameObject.SetActive(true);
        CoreManager.Instance.UpdateMoney();
    }

    public Car GetSelectedCar()
    {
        return selectedCar;
    }

    public void SpawnCar()
    {
        if (selectedCar != null) 
        {
            Destroy(selectedCar.gameObject);
        }
        var savedCar = PlayerPrefs.GetInt("SelectedCar");
        if (savedCar == -1)
        {
            return;
        }
        selectedCar = Instantiate(CARSPREFABS[PlayerPrefs.GetInt("SelectedCar")]);
        selectedCar.transform.position = CarSpawnPoint.position;
    }

    private void SelectCar()
    {
        PlayerPrefs.SetInt("SelectedCar", index);
        SelectButton.gameObject.SetActive(false);
        DeselectButton.gameObject.SetActive(true);
    }
    public void UpdateShop()
    {
        index = 0;
        LeftArrow.gameObject.SetActive(false);
        UpdateCar();
    }

    private void SwitchCar(bool Right)
    {
        index += Right ? 1 : -1;
        RightArrow.gameObject.SetActive(index < Cars.Count-1);
        LeftArrow.gameObject.SetActive(index > 0);
        UpdateCar();
    }

    private void UpdateCar()
    {

        foreach (var c in Cars)
        {
            c.Car.gameObject.SetActive(false);
        }
        var isBought = PlayerPrefs.GetInt($"Car{index}") == 0;
        BuyButton.gameObject.SetActive(isBought);
        var isSelected = PlayerPrefs.GetInt("SelectedCar") == index;
        DeselectButton.gameObject.SetActive(!isBought & isSelected);
        SelectButton.gameObject.SetActive(!isBought & !isSelected);

        

        var car = Cars[index];
        car.Car.gameObject.SetActive(true);
        car.Car.transform.localEulerAngles = Vector3.zero;
        NameText.text = car.Name;
        CostText.text = $"{car.Cost}$";
        Speed.value = car.Speed;
    }

    private void Update()
    {
        Cars[index].Car.transform.Rotate(Vector3.up * Time.deltaTime * 10f);
    }
}

[Serializable]
public class CarData
{
    public string Name;
    public GameObject Car;
    public int Cost;
    public float Speed;
}
