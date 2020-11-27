using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayViewer : MonoBehaviour
{
    public float range = 50;
    private Camera cam;

    private void Start()
    {
        cam = GetComponent<Camera>();
    }

    private void Update()
    {
        Vector3 lineOrigin = cam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0));
        Debug.DrawRay(lineOrigin, cam.transform.forward * range);
    }
}
