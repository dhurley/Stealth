using UnityEngine;
using System.Collections;

public class Controller : MonoBehaviour {

    public static string controller;

    public static float leftStickHorizontal;
    public static float leftStickVertical;
    public static float rightStickHorizontal;
    public static float rightStickVertical;

    public static bool buttonA;
    public static bool buttonB;
    public static bool buttonX;
    public static bool buttonY;

    public static bool rightBumper;
    public static bool leftBumper;
    public static float rightTrigger;
    public static float leftTrigger;

    public static bool leftStickClick;
    public static bool rightStickClick;
    public static bool startButton;

	void Start () {
	}
	
	void Update () {
        foreach (string name in Input.GetJoystickNames())
        {
            controller = name.ToLower();
        }

        if (controller.Contains("xbox"))
        {
            Debug.Log("Xbox controller.");
            useXboxInput();
        }
        else
        {
            Debug.Log("Default controller.");
            useDefaultInput();
        }
    }

    private void useXboxInput()
    {
        leftStickHorizontal = Input.GetAxis("Horizontal");
        leftStickVertical = Input.GetAxis("Vertical");
        rightStickHorizontal = Input.GetAxis("Xbox Right Stick Horizontal");
        rightStickVertical = Input.GetAxis("Xbox Right Stick Vertical");

        buttonA = Input.GetButton("Button A");
        buttonB = Input.GetButton("Button B");
        buttonX = Input.GetButton("Button X");
        buttonY = Input.GetButton("Button Y");

        rightBumper = Input.GetButton("Right Bumper");
        leftBumper = Input.GetButton("Left Bumper");
        rightTrigger = Input.GetAxis("Xbox Right Trigger");
        leftTrigger = Input.GetAxis("Xbox Left Trigger");

        leftStickClick = Input.GetButton("Left Stick Click");
        rightStickClick = Input.GetButton("Right Stick Click");
        startButton = Input.GetButton("Xbox Start Button");
    }

    private void useDefaultInput()
    {
        leftStickHorizontal = Input.GetAxis("Horizontal");
        leftStickVertical = Input.GetAxis("Vertical");
        rightStickHorizontal = Input.GetAxis("Right Stick Horizontal");
        rightStickVertical = Input.GetAxis("Right Stick Vertical");

        buttonA = Input.GetButton("Button A");
        buttonB = Input.GetButton("Button B");
        buttonX = Input.GetButton("Button X");
        buttonY = Input.GetButton("Button Y");

        rightBumper = Input.GetButton("Right Bumper");
        leftBumper = Input.GetButton("Left Bumper");
        rightTrigger = Input.GetAxis("Right Trigger");
        leftTrigger = Input.GetAxis("Left Trigger");

        leftStickClick = Input.GetButton("Left Stick Click");
        rightStickClick = Input.GetButton("Right Stick Click");
        startButton = Input.GetButton("Start Button");
    }
}
