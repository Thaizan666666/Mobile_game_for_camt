using UnityEngine;

public class Moveto : MonoBehaviour
{
    [Header("References")]
    public Transform targetObject;  // The object that will move up
    public float moveSpeed = 2f;    // Speed of upward movement
    public float moveDistance = 3f; // How far up to move

    private Vector3 targetPosition;
    private Vector3 startPosition;
    private bool isMoving = false;

    void Start()
    {
        if (targetObject != null)
        {
            startPosition = targetObject.position;
            targetPosition = startPosition + Vector3.right * moveDistance;
        }
    }

    void Update()
    {
        if (isMoving && targetObject != null)
        {
            targetObject.position = Vector3.MoveTowards(
                targetObject.position,
                targetPosition,
                moveSpeed * Time.deltaTime
            );

            // Stop when reached
            if (Vector3.Distance(targetObject.position, targetPosition) < 0.01f)
            {
                isMoving = false;
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isMoving = true;
        }
    }
}
