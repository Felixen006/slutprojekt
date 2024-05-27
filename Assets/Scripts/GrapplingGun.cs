using UnityEngine;

public class GrapplingGun : MonoBehaviour
{
    private LineRenderer lr;
    private Vector3 grapplePoint;
    public LayerMask whatIsGrappleable;
    public Transform gunTip, camera, player;
    private float maxDistance = 100f;
    private SpringJoint joint;
    private bool isGrappling = false;
    private Vector3 currentGrapplePosition;

    public float grapplingSpeed = 10f; // Adjust this speed as needed
    public float swingForceMagnitude = 5f; // Adjust this magnitude as needed

    void Awake()
    {
        lr = GetComponent<LineRenderer>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            StartGrapple();
        }
        else if (Input.GetMouseButtonUp(0))
        {
            StopGrapple();
        }

        if (isGrappling)
        {
            MoveTowardsGrapplePoint();
        }
    }

    void LateUpdate()
    {
        DrawRope();
    }

    void StartGrapple()
    {
        RaycastHit hit;
        if (Physics.Raycast(camera.position, camera.forward, out hit, maxDistance, whatIsGrappleable))
        {
            grapplePoint = hit.point;
            joint = player.gameObject.AddComponent<SpringJoint>();
            joint.autoConfigureConnectedAnchor = false;
            joint.connectedAnchor = grapplePoint;

            float distanceFromPoint = Vector3.Distance(player.position, grapplePoint);

            // The distance grapple will try to keep from grapple point.
            joint.maxDistance = distanceFromPoint * 0.8f;
            joint.minDistance = distanceFromPoint * 0.25f;

            // Adjust these values to fit your game.
            joint.spring = 4.5f;
            joint.damper = 7f;
            joint.massScale = 4.5f;

            lr.positionCount = 2;
            currentGrapplePosition = gunTip.position;
            isGrappling = true;
        }
    }

    void StopGrapple()
    {
        lr.positionCount = 0;
        Destroy(joint);
        isGrappling = false;
    }

    void DrawRope()
    {
        if (!joint) return;

        currentGrapplePosition = Vector3.Lerp(currentGrapplePosition, grapplePoint, Time.deltaTime * 8f);

        lr.SetPosition(0, gunTip.position);
        lr.SetPosition(1, currentGrapplePosition);
    }

    void MoveTowardsGrapplePoint()
    {
        // Calculate movement input
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        Vector3 movementInput = new Vector3(horizontalInput, 0f, verticalInput).normalized;

        // Calculate direction from player to grapple point
        Vector3 grappleDirection = (grapplePoint - player.position).normalized;

        // Calculate the combined movement direction, influenced by both grapple direction and input direction
        Vector3 moveDirection = grappleDirection * 0.5f + movementInput * 0.5f;

        // Apply additional force perpendicular to grapple direction to simulate swinging
        Vector3 swingForce = Vector3.Cross(grappleDirection, Vector3.up).normalized * horizontalInput * swingForceMagnitude;
        moveDirection += swingForce;

        // Apply movement to the player's Rigidbody with a fixed speed
        player.GetComponent<Rigidbody>().velocity = moveDirection * grapplingSpeed;
    }
}