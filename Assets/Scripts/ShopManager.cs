using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
    public Button BuyButton;
    public Button ExitButton;
    public Button ChooseButton;
    public Button LeftArrow;
    public Button RightArrow;
    public TextMeshProUGUI NameText;
    public TextMeshProUGUI CostText;
    public Slider Speed;
    public List<CarData> Cars;


    private int index;

    private void Start()
    {
        RightArrow.onClick.AddListener(() => SwitchCar(true));
        LeftArrow.onClick.AddListener(() => SwitchCar(false));
        BuyButton.onClick.AddListener(() => BuyCar());
        ChooseButton.onClick.AddListener(() => ChooseCar());
        ExitButton.onClick.AddListener(() => CoreManager.Instance.CloseShop());


    }

    private void BuyCar()
    {
        var money = PlayerPrefs.GetInt("Money");
        if (money < Cars[index].Cost) { return; }
        
        PlayerPrefs.SetInt("Money", money - Cars[index].Cost);
        PlayerPrefs.SetInt($"Car{index}", 1);
        BuyButton.gameObject.SetActive(false);
        ChooseButton.gameObject.SetActive(true);
        CoreManager.Instance.UpdateMoney();
    }

    private void ChooseCar()
    {

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
            c.Car.SetActive(false);
        }
        var isBought = PlayerPrefs.GetInt($"Car{index}") == 0;
        BuyButton.gameObject.SetActive(isBought);
        ChooseButton.gameObject.SetActive(!isBought);

        var car = Cars[index];
        car.Car.SetActive(true);
        car.Car.transform.localEulerAngles = Vector3.zero;
        NameText.text = car.Name;
        CostText.text = $"{car.Cost}$";
        Speed.value = car.Speed;
    }

    private void Update()
    {
        Cars[index].Car.transform.Rotate(Vector3.up * Time.deltaTime * 100f);
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
