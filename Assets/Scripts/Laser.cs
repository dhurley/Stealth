using UnityEngine;
using System.Collections;

public class Laser : MonoBehaviour {

    public float start_X;
    public float start_Y;
    public float start_Z;

    public float end_X;
    public float end_Y;
    public float end_Z;

    public float size;

    private Vector3 startPosition;
    private Vector3 endPosition;

    private LineRenderer laserLine;

    void Start()
    {
        laserLine = GetComponentInChildren<LineRenderer>();
        laserLine.SetWidth(size, size);
        startPosition = new Vector3(start_X, start_Y, start_Z);
        endPosition = new Vector3(end_X, end_Y, end_Z);
    }

    void Update()
    {
        laserLine.SetPosition(0, startPosition);
        laserLine.SetPosition(1, endPosition);
    }
}

