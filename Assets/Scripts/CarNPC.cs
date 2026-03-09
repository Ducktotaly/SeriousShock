using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarNPC : MonoBehaviour
{
    public List<Car.WheelData> Wheels = new();
    public float Speed;
    public float turnAngle = 30f;
    public Waypoint targetWaypoint;

    private float axisX;
    private float axisY;

    private void Update()
    {
        var direction = (targetWaypoint.transform.position - transform.position).normalized;
        axisX = transform.InverseTransformDirection(direction).x;

        if (Vector3.Distance(transform.position, targetWaypoint.transform.position) < 3f)
        {
            if (targetWaypoint.NextWaypoints.Count < 2)
            {
                targetWaypoint = targetWaypoint.NextWaypoints[0];
            }
            else
            {
                targetWaypoint = targetWaypoint.NextWaypoints[Random.Range(0, targetWaypoint.NextWaypoints.Count)];
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
