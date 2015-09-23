using UnityEngine;
using System.Collections;

public class CustomCharacterController : MonoBehaviour
{

    [System.Serializable]
    public class MovementSettings
    {
        public float movementVelocity = 10;
        public float turningVelocity = 100;
        public float jumpVelocity = 12;
        public float distanceToGround = 0.1f;
        public LayerMask ground;
    }

    [System.Serializable]
    public class PhysicsSettings
    {
        public float downwardAcceleration = 0.7f;
    }

    [System.Serializable]
    public class InputSettings
    {
        public float inputDelay = 0.1f;
    }

    public MovementSettings movementSettings = new MovementSettings();
    public PhysicsSettings physicsSettings = new PhysicsSettings();
    public InputSettings inputSettings = new InputSettings();

    private Quaternion targetRotation;
    public Quaternion TargetRotation
    {
        get { return targetRotation; }
    }

    private Rigidbody rigidBody;
    private Animator animator;
    private Vector3 velocity = Vector3.zero;

    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        targetRotation = transform.rotation;
    }

    void FixedUpdate()
    {
        Move();
        Jump();

        rigidBody.velocity = transform.TransformDirection(velocity);
    }

    void Update()
    {
        Turn();
    }

    private void Move()
    {
        animator.SetFloat("Speed", Controller.leftStickVertical);

        if (Mathf.Abs(Controller.leftStickVertical) > inputSettings.inputDelay)
        {
            velocity.z = movementSettings.movementVelocity * Controller.leftStickVertical;
        }
        else
        {
            velocity.z = 0;
        }
    }

    private void Turn()
    {
        if (Mathf.Abs(Controller.leftStickHorizontal) > inputSettings.inputDelay)
        {
            targetRotation *= Quaternion.AngleAxis(movementSettings.turningVelocity * Controller.leftStickHorizontal * Time.deltaTime, Vector3.up);
        }

        transform.rotation = targetRotation;
    }

    private void Jump()
    {
        if (Controller.buttonA && isGrounded())
        {
            velocity.y = movementSettings.jumpVelocity;
        }
        else if (!Controller.buttonA && isGrounded())
        {
            velocity.y = 0;
        }
        else
        {
            velocity.y -= physicsSettings.downwardAcceleration;
        }
    }

    private bool isGrounded()
    {
        return Physics.Raycast(transform.position, Vector3.down, movementSettings.distanceToGround, movementSettings.ground);
    }
}
