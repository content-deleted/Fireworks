using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[ExecuteInEditMode]
public class LineBendController : MonoBehaviour
{
    public GameObject holdPoint;
    LineRenderer line;
    void Awake () {
        line = GetComponent<LineRenderer>();
    }
    void Update()
    {
        Vector3 lineDir = (line.GetPosition(1) - line.GetPosition(0)).normalized;
        Vector3 toPoint = holdPoint.transform.position - line.GetPosition(0);
        float d = Vector3.Dot(toPoint, lineDir);
        Vector3 closestPoint = line.GetPosition(0) + lineDir * d;

        Vector4 dir = holdPoint.transform.position - closestPoint;

        line.sharedMaterial.SetVector("_BendLocation", closestPoint);
        line.sharedMaterial.SetVector("_BendDir", dir);
        line.sharedMaterial.SetVector("_Start", line.GetPosition(0));
        line.sharedMaterial.SetVector("_End", line.GetPosition(line.positionCount-1));
    }
}
