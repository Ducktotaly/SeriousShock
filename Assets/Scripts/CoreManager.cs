using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CoreManager : MonoBehaviour
{
    public List<DeliverPoint> pointGive;
    public DeliverPoint startPoint;
    public static CoreManager Instance;
    public TextMeshProUGUI moneyText;
    public TextMeshProUGUI timeText;

    private MissionDate saveData;

    private void Awake()
    {
        Instance = this;
        moneyText.text = $"{PlayerPrefs.GetInt("Money")} $";
    }

    public void OpenMissionMenu()
    {
        GetMission(UnityEngine.Random.Range(50, 300), UnityEngine.Random.Range(5,30));
    }

    public void GetMission(int value, float time)
    {
        saveData.money = value;
        saveData.time = time;
        saveData.isActive = true;
        saveData.point = pointGive[UnityEngine.Random.Range(0, pointGive.Count)];
        saveData.point.ActivePoint();
    }
    public void GetOrder()
    {
        saveData.isActive = false;
        timeText.text = "";
        startPoint.ActivePoint();
        var currentMoney = PlayerPrefs.GetInt("Money") + saveData.money;
        PlayerPrefs.SetInt("Money",currentMoney);
        moneyText.text = $"{currentMoney} $";
    }

    private void Update()
    {
        if (saveData.isActive == false) { return; }
        saveData.time -= Time.deltaTime;
        TimeSpan time = TimeSpan.FromSeconds(saveData.time);
        timeText.text = string.Format("{0:D2}:{1:D2}", time.Minutes, time.Seconds);
        if (saveData.time <= 0)
        {
            saveData.isActive = false;
            saveData.point.gameObject.SetActive(false);
            timeText.text = "";
            startPoint.ActivePoint();
        }
    }

    public struct MissionDate
    {
        public int money;
        public float time;
        public bool isActive;
        public DeliverPoint point;
    }
}
