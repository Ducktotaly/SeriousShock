using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Car : MonoBehaviour
{
    public AudioSource engineSound;
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

        engineSound.volume += (axisY != 0) ? Time.deltaTime : -Time.deltaTime ;

    }
    private void OnCollisionEnter(Collision collision)
    {
        if (isActive == false) { return; }
        CoreManager.Instance.GetDamage(collision.transform.tag);
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

    public void SetActiveCar(bool setValue)
    {
        engineSound.volume = 0;
        isActive = setValue;
        if (setValue == false)
        {
            engineSound.Stop();
            foreach (var wheel in Wheels)
            {
                wheel.wheel.motorTorque = 0;
                onBreak(wheel.wheel,brakeSpeed);
            }
        }
        else { engineSound.Play(); }
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
