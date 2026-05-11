using DG.Tweening;
using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using YG;

public class CoreManager : MonoBehaviour
{
    public GameObject mainCamera;
    public CanvasGroup blackScreen;
    public Transform shopPoint;
    public ShopManager shopManager;
    public DeliverPoint startPoint;
    public static CoreManager Instance;
    public TextMeshProUGUI moneyText;
    public TextMeshProUGUI timeText;
    public Transform arrow;
    public MapEntity mapEntity;
    public Slider hpSlider;
    public MissionMenuManager mmManager;
    public GameObject winScreen;
    public GameObject winAdButton;
    public TextMeshProUGUI winEarnTxt;

    private MissionData saveData = new();
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

    private void Start()
    {
        mmManager.GenerateMissions();
        DOTween.Sequence()
            .AppendCallback(() => DOTween.To(x => blackScreen.alpha = x, 1, 0, 2f))
            .AppendInterval(2f);
    }

    public void GetDamage(string tag, Transform test = null)
    {
        if (saveData.isActive == false) { return; }

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

        if (health <= 0)
        {
            health = 0;
            mapEntity.player.OnPlayerDeath();
        }


        hpSlider.value = health / 100;
    }

    public void DeathAnim()
    {
        DOTween.Sequence()
            .AppendInterval(4f)
            .AppendCallback(() => DOTween.To(x => blackScreen.alpha = x, 0, 1, 2f))
            .AppendInterval(2f)


            //ƒобавить чего нибудь после смерти типо


            .AppendCallback(() => {
                YG2.InterstitialAdvShow();
                SceneManager.LoadScene(0);
            });
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
                    YG2.InterstitialAdvShow();
                })
                .AppendCallback(() => DOTween.To(x => blackScreen.alpha = x, 1, 0, 1f))
                .AppendInterval(1f)
                .AppendCallback(() => activeShop = false);
    }

    public void UpdateMoney()
    {
        moneyText.text = $"{PlayerPrefs.GetInt("Money")} $";
    }

    public void CloseMissionMenu()
    {
        mmManager.CloseMenu();
    }

    public void OpenMissionMenu()
    {
        mmManager.OpenMenu();
    }

    private void FailedOrder()
    {
        health = 100;
        hpSlider.gameObject.SetActive(false);

        saveData.isActive = false;
        saveData.point.gameObject.SetActive(false);
        timeText.text = "";
        startPoint.ActivePoint();
    }

    public void SetMission(MissionData data)
    {
        hpSlider.gameObject.SetActive(true);
        saveData = data;
        startPoint.gameObject.SetActive(false);
        saveData.point.ActivePoint();
    }
    public void GiveOrder()
    {
        health = 100;
        hpSlider.gameObject.SetActive(false);
        saveData.point.gameObject.SetActive(false);
        saveData.isActive = false;
        timeText.text = "";
        startPoint.ActivePoint();
        var currentMoney = PlayerPrefs.GetInt("Money") + saveData.money;
        PlayerPrefs.SetInt("Money", currentMoney);
        moneyText.text = $"{currentMoney} $";
        winEarnTxt.text = $"{saveData.money}$";
        winScreen.SetActive(true);
        winAdButton.SetActive(true);
    }

    public void SetReward()
    {
            YG2.RewardedAdvShow("Double", () =>
            {
                var currentMoney = PlayerPrefs.GetInt("Money") + saveData.money;
                PlayerPrefs.SetInt("Money", currentMoney);
                moneyText.text = $"{currentMoney} $";
                winEarnTxt.text = $"{saveData.money*2}$";
                winAdButton.SetActive(false);
            });
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
            FailedOrder();
        }
    }

    private void onMoveArrow()
    {
        var player = mapEntity.GetActivePlayer();
        var target = saveData.isActive ? saveData.point.transform : startPoint.transform;

        var direction = target.position - player.position;
        arrow.rotation = Quaternion.LookRotation(direction);
    }
}
