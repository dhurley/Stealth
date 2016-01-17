using UnityEngine;
using System;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Joystick_Controller : MonoBehaviour {

    private float radius;
    private Vector2 center;

    private float verticalInput;

    public float VerticalInput
    {
        get { return (transform.position.y - center.y) / radius; }
    }

    private string horizontalInput;

    public float HorizontalInput
    {
        get { return (transform.position.x - center.x) / radius; }
    }


    void Start () {
        center = new Vector2(this.transform.position.x, this.transform.position.y);
        radius = center.x * 0.7f;
	}

    public void UpdatePosition(Vector2 touchPosition)
    {
        if (IsInsideAreaOfJoystick(touchPosition))
        {
            this.transform.position = touchPosition;
        }
        else
        {
            float angle = Mathf.Atan2(touchPosition.y - center.y, touchPosition.x - center.x);
            float x = radius * Mathf.Cos(angle);
            float y = radius * Mathf.Sin(angle);
            this.transform.position = new Vector3(x + center.x, y + center.y, 0);
        }
    }

    public void ResetPosition()
    {
        this.transform.position = center;
    }

    private bool IsInsideAreaOfJoystick(Vector2 touchPosition)
    {
        return Math.Pow(touchPosition.x - center.x, 2) + Math.Pow(touchPosition.y - center.y, 2) < Math.Pow(radius, 2);
    }
}
