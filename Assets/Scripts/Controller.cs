using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Controller : MonoBehaviour {

    public GameObject ButtonA;
    public GameObject ButtonB;

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

    private Joystick_Controller leftJoystickController;

	void Start () {
        GameObject leftJoystick = GameObject.Find("Left_Joystick");
        leftJoystickController = leftJoystick.GetComponent<Joystick_Controller>();
	}
	
	void Update () {
        if (Application.platform == RuntimePlatform.Android)
        {
            UseTouchInput();
        }
        else
        {

            foreach (string name in Input.GetJoystickNames())
            {
                controller = name.ToLower();
            }

            if (controller.Contains("xbox"))
            {
                UseXboxInput();
            }
            else
            {
                UseDefaultInput();
            }
        }
    }

    private void UseTouchInput()
    {
        leftStickHorizontal = leftJoystickController.HorizontalInput;
        leftStickVertical = leftJoystickController.VerticalInput;

        UpdateLeftJoystickPosition(leftJoystickController);
    }

    private static void UpdateLeftJoystickPosition(Joystick_Controller leftJoystickController)
    {
        Touch[] touches = Input.touches;

        if (Input.touchCount == 0)
        {
            leftJoystickController.ResetPosition();
        }

        for (int i = 0; i < Input.touchCount; i++)
        {
            if (touches[i].position.x < Screen.width / 3 && touches[i].position.y < Screen.height / 2)
            {
                leftJoystickController.UpdatePosition(touches[i].position);
            }
        }
    }

    private void UseXboxInput()
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

    private void UseDefaultInput()
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

    public void UIJumpButtonPressedDown()
    {
        buttonA = true;
    }

    public void UIJumpButtonPressedUp()
    {
        buttonA = false;
    }

    public void UISlideButtonPressedDown()
    {
        buttonB = true;
    }

    public void UISlideButtonPressedUp()
    {
        buttonB = false;
    }
}
