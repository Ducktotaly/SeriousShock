using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Car : MonoBehaviour
{
    public List<WheelData> Wheels = new();
    public CarCollider Spawn;
    public float Speed;
    public float brakeSpeed;
    public float turnAngle;
    
    private bool isActive;
    private float axisX;
    private float axisY;

    [Serializable]
    public struct WheelData
    {
        public WheelCollider wheel;
        public Transform model;
        public bool isFirst;
    }

    // Update is called once per frame

    private void Update()
    {
        if (isActive == false) { return;}
        axisX = Input.GetAxis("Horizontal");
        axisY = Input.GetAxis("Vertical");
    }

    private void FixedUpdate()
    {
        if (isActive == false) { return;}
        foreach (var wheel in Wheels)
        {
            onSteerTurn(wheel.wheel, wheel.isFirst);

            if (axisY == 0)
            {
                onBreak(wheel.wheel,brakeSpeed);
            }
            else
            {
                onBreak(wheel.wheel, 0);
                OnMove(wheel.wheel);
            }

            wheel.wheel.GetWorldPose(out var pos, out var rot);
            wheel.model.position = pos;
            wheel.model.rotation = rot;
        }
    }

    public void SetActive(bool setValue)
    {
        isActive = setValue;
        if (setValue == false)
        {
            foreach (var wheel in Wheels)
            {
                wheel.wheel.motorTorque = 0;
                onBreak(wheel.wheel,brakeSpeed);
            }
        }
    }

    private void onBreak(WheelCollider wheel,float speed)
    {
        wheel.brakeTorque = speed;
    }

    private void onSteerTurn(WheelCollider wheel, bool isFirst)
    {
        if (isFirst)
        {
            wheel.steerAngle = turnAngle * axisX;
        }
    }
    private void OnMove(WheelCollider wheel)
    {
        wheel.motorTorque = Speed * axisY;
    }
}
