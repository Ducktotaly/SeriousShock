using DG.Tweening;
using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class CoreManager : MonoBehaviour
{
    public GameObject mainCamera;
    public CanvasGroup blackScreen;
    public Transform shopPoint;
    public ShopManager shopManager;
    public List<DeliverPoint> pointGive;
    public DeliverPoint startPoint;
    public static CoreManager Instance;
    public TextMeshProUGUI moneyText;
    public TextMeshProUGUI timeText;
    public Transform arrow;
    public MapEntity mapEntity;

    private MissionDate saveData;
    private bool activeShop;
    private float health = 100;

    private void Awake()
    {
        Instance = this;

        if (PlayerPrefs.GetInt("IsFirstGame") == 0)
        {
            PlayerPrefs.SetInt("IsFirstGame", 1);
            PlayerPrefs.SetInt("SelectedCar", -1);
        }

        UpdateMoney();

    }

    public void GetDamage(string tag)
    {
        if (tag == "DeathNPC") { return; }
        var damage = 0f;
        if (tag == "NPC")
        {
            damage = 20f;
        }
        if (tag == "CarNPC")
        {
            damage = 100f;
        }
        
        if (mapEntity.GetActivePlayer() != mapEntity.player.transform)
        {
            damage /= 5f; // CarData Defence
        }
        health -= damage;
        Debug.Log($"Damage: {damage}, HP: {health}, Tag: {tag}");
    }

    public void CloseShop()
    {
        DOTween.Sequence()
                .AppendCallback(() => DOTween.To(x => blackScreen.alpha = x, 0, 1, 1f))
                .AppendInterval(1f)
                .AppendCallback(() =>
                {
                    mainCamera.SetActive(true);
                    shopManager.gameObject.SetActive(false);
                })
                .AppendCallback(() => DOTween.To(x => blackScreen.alpha = x, 1, 0, 1f))
                .AppendInterval(1f)
                .AppendCallback(() => activeShop = false);
    }
  
    public void UpdateMoney()
    {
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

    private void TryOpenShop()
    {
        if (activeShop)
        {
            return;
        }

        if (Vector3.Distance(shopPoint.position, mapEntity.player.transform.position) > 3f)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            activeShop = true;
            DOTween.Sequence()
                .AppendCallback(() => DOTween.To(x => blackScreen.alpha = x, 0, 1, 1f))
                .AppendInterval(1f)
                .AppendCallback(() =>
                {
                    mainCamera.SetActive(false);
                    shopManager.UpdateShop();
                    shopManager.gameObject.SetActive(true);
                })
                .AppendCallback(() => DOTween.To(x => blackScreen.alpha = x, 1, 0, 1f));
        }
    }

    private void Update()
    {
        TryOpenShop();
        onMoveArrow();
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

    private void onMoveArrow()
    {
        var player = mapEntity.GetActivePlayer();
        var target = saveData.isActive ? saveData.point.transform : startPoint.transform;

        var direction = target.position - player.position;
        arrow.rotation = Quaternion.LookRotation(direction);
    }

    public struct MissionDate
    {
        public int money;
        public float time;
        public bool isActive;
        public DeliverPoint point;
    }
}
