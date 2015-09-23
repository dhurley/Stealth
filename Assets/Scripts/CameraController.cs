using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

    public Transform target;

    [System.Serializable]
    public class PositionSettings
    {
        public Vector3 targetPositionOffset = new Vector3(0, 1, 0);
        public float lookSmooth = 100;
        public float distanceFromTarget = -4;
        public float zoomSmooth = 6;
        public float maximumZoom = -2;
    }

    [System.Serializable]
    public class OrbitSettings
    {
        public float xRotation = -15;
        public float yRotation = -180;
        public float maxXRotation = 8;
        public float minXRotation = -85;
        public float vOrbitSmooth = 150;
        public float hOrbitSmooth = 150;
    }

    public PositionSettings positionSettings = new PositionSettings();
    public OrbitSettings orbitSettings = new OrbitSettings();

    private Vector3 targetPosition = Vector3.zero;
    private Vector3 destination = Vector3.zero;
    private CustomCharacterController characterController;
    private float vOrbitInput, hOrbitInput, zoomInput, defaultZoom;
    private bool hOrbitSnapInput;

	void Start () {
        defaultZoom = positionSettings.distanceFromTarget;
        SetCamerTarget(target);
        MoveToTarget();
	}

    public void SetCamerTarget(Transform target)
    {
        this.target = target;
        if (target != null)
        {
            if (target.GetComponent<CustomCharacterController>())
            {
                characterController = target.GetComponent<CustomCharacterController>();
            }
            else
            {
                Debug.LogError("The camera's target needs a character controller.");
            }
        }
        else
        {
            Debug.LogError("The camera needs a target.");
        }
    }

    void Update()
    {
        GetInput();
        OrbitTarget();
        ZoomInOnTarget();
    }

    void LateUpdate()
    {
        MoveToTarget();
        LookAtTarget();
    }

    private void GetInput()
    {
        vOrbitInput = Controller.rightStickVertical;
        hOrbitInput = Controller.rightStickHorizontal;
        zoomInput = Controller.leftTrigger;
        Debug.Log(zoomInput);
        hOrbitSnapInput = Controller.rightStickClick;
    }

    private void MoveToTarget()
    {
        targetPosition = target.position + positionSettings.targetPositionOffset;
        destination = Quaternion.Euler(orbitSettings.xRotation, orbitSettings.yRotation + target.eulerAngles.y, 0) * -Vector3.forward * positionSettings.distanceFromTarget;
        destination += targetPosition;
        transform.position = destination;
    }

    private void LookAtTarget()
    {
        Quaternion targetRotation = Quaternion.LookRotation(targetPosition - transform.position);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, positionSettings.lookSmooth * Time.deltaTime);
    }

    private void OrbitTarget()
    {
        if (hOrbitSnapInput)
        {
            orbitSettings.yRotation = -180;
        }

        orbitSettings.xRotation += -vOrbitInput * orbitSettings.vOrbitSmooth * Time.deltaTime;
        orbitSettings.yRotation += -hOrbitInput * orbitSettings.vOrbitSmooth * Time.deltaTime;

        if (orbitSettings.xRotation > orbitSettings.maxXRotation)
        {
            orbitSettings.xRotation = orbitSettings.maxXRotation;
        }

        if (orbitSettings.xRotation < orbitSettings.minXRotation)
        {
            orbitSettings.xRotation = orbitSettings.minXRotation;
        }
    }

    private void ZoomInOnTarget()
    {
        positionSettings.distanceFromTarget += zoomInput * positionSettings.zoomSmooth * Time.deltaTime;

        if (positionSettings.distanceFromTarget > positionSettings.maximumZoom)
        {
            positionSettings.distanceFromTarget = positionSettings.maximumZoom;
        }

        if (positionSettings.distanceFromTarget >= defaultZoom && zoomInput == 0)
        {
            positionSettings.distanceFromTarget -= positionSettings.zoomSmooth * Time.deltaTime;
        }
    }
}
