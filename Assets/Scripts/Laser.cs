using UnityEngine;
using System.Collections;

public class Laser : MonoBehaviour {

    public Vector3 startPosition;
    public Vector3 endPosition;
    public float size;

    private LineRenderer laserLine;

    void Start()
    {
        laserLine = GetComponentInChildren<LineRenderer>();
        laserLine.SetWidth(size, size);
    }

    void Update()
    {
        laserLine.SetPosition(0, startPosition);
        laserLine.SetPosition(1, endPosition);
    }
}

