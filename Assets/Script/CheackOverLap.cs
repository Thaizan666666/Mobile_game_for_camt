using UnityEngine;

public class CheackOverLap : MonoBehaviour
{
    [Header("Settings")]
    public string targetTag = "Plantform";
    public bool Canjump => overlapCount > 0; // ✅ true ตราบใดที่ยังมีพื้นอยู่

    private CapsuleCollider2D capsule;
    private int overlapCount = 0; // นับจำนวน Platform ที่สัมผัสอยู่

    private void Start()
    {
        capsule = GetComponent<CapsuleCollider2D>();
        if (capsule == null)
            Debug.LogError("ไม่พบ CapsuleCollider2D บน " + gameObject.name);

        capsule.isTrigger = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(targetTag))
        {
            overlapCount++; // เพิ่มเมื่อแตะ Platform ใหม่
            Debug.Log($"Enter: {collision.name} | Count: {overlapCount}");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag(targetTag))
        {
            overlapCount = Mathf.Max(0, overlapCount - 1); // ลดเมื่อออกจาก Platform
            Debug.Log($"Exit: {collision.name} | Count: {overlapCount}");
        }
    }
}