using UnityEngine;

public class AnimalMove : MonoBehaviour
{
    public float moveSpeed = 3f;
    public float rotationSpeed = 5f;
    public float gravity = -9.81f;
    public float groundOffset = 0.1f;
    public LayerMask terrainLayer;

    private float minChangeDirectionTime = 3f;
    private float maxChangeDirectionTime = 5f;
    private CharacterController controller;
    private Vector3 moveDirection;
    private float verticalVelocity = 0f;
    private float timer = 0f;
    private float changeDirectionTime;
    private Animator animator;
    private int isWalkingId;
    private float angle;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        SetRandomDirection();

        angle = Random.Range(0f, 360f);

        animator = GetComponent<Animator>();
        isWalkingId = Animator.StringToHash("isWalking");

        changeDirectionTime = Random.Range(minChangeDirectionTime, maxChangeDirectionTime);
    }

    void Update()
    {
        animator.SetBool(isWalkingId, true);

        ApplyGravity();
        MoveAnimal();

        timer += Time.deltaTime;
        if (timer >= changeDirectionTime)
        {
            timer = 0f;
            SetRandomDirection();

            changeDirectionTime = Random.Range(minChangeDirectionTime, maxChangeDirectionTime);
        }

        KeepCloseToGround();
    }

    void ApplyGravity()
    {
        if (controller.isGrounded)
        {
            verticalVelocity = 0f;
        }
        else
        {
            verticalVelocity += gravity * Time.deltaTime;
        }
    }

    void MoveAnimal()
    {
        Vector3 move = moveDirection * moveSpeed + Vector3.up * verticalVelocity;
        controller.Move(move * Time.deltaTime);
        RotateTowardsDirection();
    }

    void SetRandomDirection()
    {
        angle = Random.Range(angle - 60f, angle + 60f);
        float radians = angle * Mathf.Deg2Rad;
        moveDirection = new Vector3(Mathf.Cos(radians), 0, Mathf.Sin(radians)).normalized;
    }

    void RotateTowardsDirection()
    {
        if (moveDirection != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }
    void KeepCloseToGround()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, Mathf.Infinity, terrainLayer))
        {
            float distanceToGround = hit.distance;
            if (distanceToGround > groundOffset)
            {
                transform.position = new Vector3(transform.position.x, hit.point.y + groundOffset, transform.position.z);
            }
        }
    }
}