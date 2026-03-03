using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarNPC : MonoBehaviour
{
    public List<Transform> movePoints = new List<Transform>();
    public List<Car.WheelData> Wheels = new();
    public float Speed;
    public float turnAngle = 30f;

    private float axisX;
    private float axisY;
    private int indexPoint;

    private void Update()
    {
        var direction = (movePoints[indexPoint].position - transform.position).normalized;
        axisX = transform.InverseTransformDirection(direction).x;

        if (Vector3.Distance(transform.position, movePoints[indexPoint].position) < 3f)
        {
            indexPoint++;
            if (indexPoint >= movePoints.Count)
            {
                indexPoint = 0;
            }
        }
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
        wheel.motorTorque = Speed;
    }

    private void FixedUpdate()
    {
        foreach (var wheel in Wheels)
        {
            onSteerTurn(wheel.wheel, wheel.isFirst);
            OnMove(wheel.wheel);

            wheel.wheel.GetWorldPose(out var pos, out var rot);
            wheel.model.position = pos;
            wheel.model.rotation = rot;
        }
    }
}
