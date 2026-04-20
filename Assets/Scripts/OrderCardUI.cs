using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OrderCardUI : MonoBehaviour
{
    public TextMeshProUGUI description;
    public TextMeshProUGUI distance;
    public TextMeshProUGUI time;
    public TextMeshProUGUI money;
    public Button btn;

    private MissionMenuManager manager;
    private MissionData data;


    public void Setup(MissionData mdata, MissionMenuManager mmManager)
    {
        data = mdata;
        manager = mmManager;

        distance.text = $"{(int) data.Distance} m";
        TimeSpan timeval = TimeSpan.FromSeconds(data.time);
        time.text = string.Format("{0:D2}:{1:D2}", timeval.Minutes, timeval.Seconds);
        money.text = $"{data.money}$";

        btn.onClick.RemoveAllListeners();
        btn.onClick.AddListener(OnMissionAccept);
    }
    private void OnMissionAccept()
    {
        data.isActive = true;
        manager.AcceptMission(data);
    }
}
