using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class CameraView : MonoBehaviour
{
    public float CameraSpeed;
    public Transform Target;
    private Vector3 offset;
    private bool isFixed;
    public Material fadeMat;

    private List<Build> buildings = new();

    public class Build
    {
        public Transform building;
        public Material originalMat;
    }

    public void SetOffset(Vector3 pos)
    {
        offset = transform.position - pos;
    }

    public void SetTarget(Transform target,bool fix)
    {
        Target = target;
        isFixed = fix;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (isFixed == false) { return; }
        transform.position = Vector3.Lerp(transform.position, Target.position + offset, Time.fixedDeltaTime * CameraSpeed);
    }
    void Update()
    {
        if (isFixed == true) { return; }
        transform.position = Vector3.Lerp(transform.position, Target.position + offset, Time.deltaTime * CameraSpeed);
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.TryGetComponent<MeshRenderer>(out var mesh))
        {
            for (var i = buildings.Count-1; i >= 0; i--)
            {
                if (other.transform == buildings[i].building)
                {
                    return;
                }
            }
            var buildData = new Build();
            buildData.building = other.transform;
            buildData.originalMat = mesh.material;
            buildings.Add(buildData);
            mesh.material = fadeMat;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform.TryGetComponent<MeshRenderer>(out var mesh))
        {
            for (var i = buildings.Count-1; i >= 0; i--) 
            {
                var build = buildings[i];
                if (other.transform != build.building)
                {
                    continue;
                }
                mesh.material = build.originalMat;
                buildings.Remove(build);
                
            }
        }
    }
}
    
