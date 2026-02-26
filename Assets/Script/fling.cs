using UnityEngine;

public class Fling : MonoBehaviour
{
    [Header("References")]
    public Transform targetObject;

    [Header("Settings")]
    public float moveSpeed = 2f;
    public float moveDistance = 5f;

    [Header("Custom Pattern")]
    public bool[] movePattern = { true, false, true, false };  // true = ขึ้น, false = ลง

    private Vector3 upPosition;
    private Vector3 downPosition;
    private Vector3 targetPosition;
    private bool isMoving = false;
    private int currentIndex = 0;

    void Start()
    {
        if (targetObject != null)
        {
            downPosition = targetObject.position;
            upPosition = downPosition + Vector3.up * moveDistance;

            // กำหนดตำแหน่งแรก
            targetPosition = movePattern[currentIndex] ? upPosition : downPosition;
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

            if (Vector3.Distance(targetObject.position, targetPosition) < 0.01f)
            {
                isMoving = false;
                targetObject.position = targetPosition;
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !isMoving)
        {

                // กำหนดตำแหน่งตาม Pattern
                bool shouldMoveUp = movePattern[currentIndex];
                Debug.Log($"{currentIndex}");
                targetPosition = shouldMoveUp ? upPosition : downPosition;

                isMoving = true;

            if (currentIndex < movePattern.Length-1)
            {
                currentIndex = (currentIndex + 1);
            }
            else
            {
                return;
            }
        }
    }
}