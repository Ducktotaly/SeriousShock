using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarNPC : MonoBehaviour
{
    public List<Car.WheelData> Wheels = new();
    public float Speed;
    public float turnAngle = 30f;
    public Waypoint targetWaypoint;
    public float respawnTime = 5f;
    public Rigidbody rb;


    private float axisX;

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

        if (rb.velocity.magnitude < 0.3f)
        {
            respawnTime -= Time.deltaTime;
            if (respawnTime <= 0)
            {
                var spawnPos = targetWaypoint.transform.position;
                if (!Physics.CheckSphere(spawnPos, 2f, LayerMask.GetMask("Car")))
                {
                    transform.position = spawnPos + Vector3.up;
                    respawnTime = 5f;
                }
                else
                {
                    respawnTime = 1.5f;
                }
                
                
                
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
