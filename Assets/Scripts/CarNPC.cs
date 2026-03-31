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
    public Renderer renderCar;
    public Camera cam;


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
                    if (IsPlayerSee())
                    {
                        respawnTime = 2.5f;
                        return;
                    }
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

    private bool IsPlayerSee()
    {
        Plane[] planes = GeometryUtility.CalculateFrustumPlanes(cam);
        var viewPos = cam.WorldToViewportPoint(targetWaypoint.transform.position);
        var isPointVisible = viewPos.z > 0 && viewPos.x >= 0 && viewPos.x <= 1 && viewPos.y >= 0 && viewPos.y <= 1;
        var isNPCVisible = GeometryUtility.TestPlanesAABB(planes, renderCar.bounds);
        return isPointVisible || isNPCVisible;
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
