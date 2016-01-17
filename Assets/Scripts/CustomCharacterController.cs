using UnityEngine;
using System.Collections;

public class CustomCharacterController : MonoBehaviour
{

    [System.Serializable]
    public class MovementSettings
    {
        public LayerMask ground;
        public float movementVelocity = 6;
        public float distanceToGround = 0.1f;
        public float slideDuration = 2;
    }

    [System.Serializable]
    public class InputSettings
    {
        public float inputDelay = 0.1f;
    }

    public MovementSettings movementSettings = new MovementSettings();
    public InputSettings inputSettings = new InputSettings();

    private Rigidbody rigidBody;
    private Animator animator;
    private Vector3 velocity = Vector3.zero;
    private float timeSinceLastSlide;

    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        animator.SetFloat("Speed", 0);
        animator.SetBool("Jump", false);
        animator.SetBool("Slide", false);
        timeSinceLastSlide = Time.time - movementSettings.slideDuration;

    }

    void FixedUpdate()
    {
        Debug.Log(animator.GetCurrentAnimatorStateInfo(0).shortNameHash + "  -------    " + Animator.StringToHash("Jump"));
        Jump();
        Slide();
        rigidBody.velocity = velocity;
    }

    void Update()
    {
        Move();
    }

    private void Move()
    {
        if (IsMoving() && (IsGrounded() || IsJumping()))
        {
            var direction = new Vector3(Controller.leftStickHorizontal, 0, Controller.leftStickVertical);
            velocity = direction * movementSettings.movementVelocity;
            var rotation = Quaternion.LookRotation(direction, Vector3.up);
            transform.rotation = rotation;
        }
        else if (IsMoving())
        {
            var direction = new Vector3(Controller.leftStickHorizontal, -0.5f, Controller.leftStickVertical);
            velocity = direction * movementSettings.movementVelocity;
            var rotation = Quaternion.LookRotation(direction, Vector3.up);
            transform.rotation = rotation;
        }
        else if (IsJumping() || IsGrounded())
        {
            velocity = Vector3.zero;
        }
        else
        {
            velocity =  new Vector3(0,-0.5f,0);
        }

        animator.SetFloat("Speed", velocity.magnitude);
    }

    private bool IsMoving()
    {
        return Mathf.Abs(Controller.leftStickVertical) > inputSettings.inputDelay || Mathf.Abs(Controller.leftStickHorizontal) > inputSettings.inputDelay;
    }

    private void Jump()
    {
        if (Controller.buttonA && IsGrounded())
        {
            animator.SetBool("Jump", true);
        }
        else if (IsGrounded())
        {
            animator.SetBool("Jump", false);
        }

    }

    private void Slide()
    {
        if (Controller.buttonB && CanStartSlideNow())
        {
            animator.SetBool("Slide", true);
            timeSinceLastSlide = Time.time;
        }
        else
        {
            animator.SetBool("Slide", false);
        }
    }

    private bool CanStartSlideNow()
    {
        return Time.time - timeSinceLastSlide > movementSettings.slideDuration;
    }

    private bool IsGrounded()
    {
        return Physics.Raycast(transform.position, Vector3.down, movementSettings.distanceToGround, movementSettings.ground);
    }

    private bool IsJumping()
    {
        return animator.GetCurrentAnimatorStateInfo(0).shortNameHash == Animator.StringToHash("Jump"); 
    }
}
