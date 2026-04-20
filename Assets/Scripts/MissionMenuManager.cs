using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionMenuManager : MonoBehaviour
{
    public List<DeliverPoint> givePoints = new();
    public List<MissionData> missions = new();
    public List<GameObject> cards = new();
    public GameObject cardTemplate;
    public Transform missionContainer;
    public Transform takePoint;
    public Player player;

    public void GenerateMissions()
    {
        missions.Clear();
        var missionQuantity = Random.Range(2,7);
        List<DeliverPoint> tempPoints = new(givePoints);
        for (int i = 0; i < missionQuantity; i++) 
        { 
            MissionData data = new MissionData();
            data.point = tempPoints[Random.Range(0, tempPoints.Count)];
            tempPoints.Remove(data.point);
            data.Distance = Vector3.Distance(takePoint.position, data.point.transform.position);
            data.time = data.Distance / player.GetPlayerSpeed();
            data.money = (int) (50 + (data.Distance * 0.5));
            data.isActive = false;
            missions.Add(data);
        }
    }

    public void AcceptMission(MissionData data)
    {
        CoreManager.Instance.SetMission(data);
        CloseMenu();
        GenerateMissions();
    }


    public void OpenMenu()
    {
        
        foreach (var mission in missions) 
        {
            var card = Instantiate(cardTemplate, missionContainer);
            cards.Add(card);
            var cardUI = card.GetComponent<OrderCardUI>();
            cardUI.Setup(mission, this);
        }
        /*
          ароче криво работает типо анимаци€ чтобы увеличивалось и уменьшалось но выгл€дит топорно
        DOTween.Sequence()
            .AppendCallback(() => DOTween.To(x => missionContainer.localScale = Vector3.one * x, 1f, 1.25f, 0.3f))
            .AppendInterval(0.3f)
            .AppendCallback(() => DOTween.To(x => missionContainer.localScale = Vector3.one * x, 1.25f, 1, 0.25f));
        */
        gameObject.SetActive(true);
    }

    private void ClearCards()
    {
        foreach(var card in cards) 
        {
            Destroy(card);
        }
    }

    public void CloseMenu()
    {
        ClearCards();
        gameObject.SetActive(false);
    }
}

public class MissionData
{
    public int money;
    public float Distance;
    public float time;
    public bool isActive = false;
    public DeliverPoint point;
}
