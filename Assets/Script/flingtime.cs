using UnityEngine;

public class FlingTime : MonoBehaviour
{
    [Header("References")]
    public Transform targetObject;

    [Header("Settings")]
    public float moveSpeed = 2f;
    public float moveDistance = 5f;

    [Header("Times")]
    public float waitTime = 1f;  // เวลารอก่อนเคลื่อนที่ครั้งถัดไป

    [Header("Custom Pattern")]
    public bool[] movePattern = { true, false, true, false };  // true = ขึ้น/ขวา, false = ลง/ซ้าย

    [Header("Movement Direction")]
    public bool isHorizontal = false;  // false = แนวตั้ง (Y), true = แนวนอน (X)
    public bool invertDirection = false;  // สลับทิศทาง (ซ้าย/ขวา หรือ ลง/ขึ้น)

    private Vector3 firstPosition;   // ตำแหน่งแรก
    private Vector3 secondPosition;  // ตำแหน่งที่สอง
    private Vector3 targetPosition;
    private bool isMoving = false;
    private int currentIndex = 0;
    private float timer = 0f;
    private bool isWaiting = false;
    private bool hasStarted = false;

    void Start()
    {
        if (targetObject != null)
        {
            firstPosition = targetObject.position;

            if (isHorizontal)
            {
                // เคลื่อนที่แนวนอน (X)
                if (invertDirection)
                    secondPosition = firstPosition + Vector3.left * moveDistance;  // ซ้าย
                else
                    secondPosition = firstPosition + Vector3.right * moveDistance; // ขวา
            }
            else
            {
                // เคลื่อนที่แนวตั้ง (Y)
                if (invertDirection)
                    secondPosition = firstPosition + Vector3.down * moveDistance;  // ลง
                else
                    secondPosition = firstPosition + Vector3.up * moveDistance;    // ขึ้น
            }
        }
        else
        {
            Debug.LogWarning("Target Object ยังไม่ได้กำหนด!");
        }
    }

    void Update()
    {
        // เคลื่อนที่ Object
        if (isMoving && targetObject != null)
        {
            targetObject.position = Vector3.MoveTowards(
                targetObject.position,
                targetPosition,
                moveSpeed * Time.deltaTime
            );

            // ถึงเป้าหมายแล้ว
            if (Vector3.Distance(targetObject.position, targetPosition) < 0.01f)
            {
                isMoving = false;
                targetObject.position = targetPosition;

                // เริ่มนับเวลาสำหรับรอบถัดไป
                if (hasStarted && currentIndex < movePattern.Length)
                {
                    isWaiting = true;
                    timer = 0f;
                }
            }
        }

        // รอเวลาก่อนเคลื่อนที่ครั้งถัดไป
        if (isWaiting)
        {
            timer += Time.deltaTime;

            if (timer >= waitTime)
            {
                isWaiting = false;
                MoveToNextPosition();
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !hasStarted && !isMoving)
        {
            hasStarted = true;
            currentIndex = 0;
            MoveToNextPosition();
        }
    }

    void MoveToNextPosition()
    {
        if (currentIndex < movePattern.Length)
        {
            bool moveToSecond = movePattern[currentIndex];
            targetPosition = moveToSecond ? secondPosition : firstPosition;

            string direction = GetDirectionName(moveToSecond);
            Debug.Log($"รอบที่ {currentIndex + 1}: {direction}");

            isMoving = true;
            currentIndex++;
        }
        else
        {
            Debug.Log("จบ Pattern แล้ว");
        }
    }

    string GetDirectionName(bool moveToSecond)
    {
        if (isHorizontal)
        {
            if (invertDirection)
                return moveToSecond ? "ซ้าย" : "กลับ";
            else
                return moveToSecond ? "ขวา" : "กลับ";
        }
        else
        {
            if (invertDirection)
                return moveToSecond ? "ลง" : "กลับ";
            else
                return moveToSecond ? "ขึ้น" : "กลับ";
        }
    }
}