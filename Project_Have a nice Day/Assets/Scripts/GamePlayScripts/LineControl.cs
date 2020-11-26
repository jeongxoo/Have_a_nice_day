using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineControl : MonoBehaviour
{
    public LineRenderer line;
    public List<Vector3> listPoint;

    public void Reset()
    {
        listPoint = new List<Vector3>();
        line = GetComponent<LineRenderer>();
        line.positionCount = 0;
    }

    public void AddPoint(Vector3 pos)
    {
        listPoint.Add(pos);
        Vector3[] points = listPoint.ToArray();

        line.positionCount = points.Length;
        line.SetPositions(points);
    }

    public void RemovePoint()
    {
        listPoint.RemoveAt(listPoint.Count - 1);
        Vector3[] points = listPoint.ToArray();

        line.positionCount = points.Length;
        line.SetPositions(points);
    }
}
