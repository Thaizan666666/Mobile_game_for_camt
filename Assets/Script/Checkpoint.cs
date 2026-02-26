using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    [Header("Visual Feedback")]
    public Color inactiveColor = Color.white;
    public Color activeColor = Color.yellow;

    private bool activated = false;
    private SpriteRenderer sr;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();

        if (sr != null)
        {
            sr.color = inactiveColor;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (activated) return;

        if (other.CompareTag("Player"))
        {
            activated = true;
            CheckpointManager.Instance.SetCheckpoint(transform.position);

            // เปลี่ยนสีเป็น active
            if (sr != null)
            {
                sr.color = activeColor;
            }

            Debug.Log($"Checkpoint activated at {transform.position}");
        }
    }

    // ✅ ฟังก์ชันสำหรับ Reset Checkpoint (ถ้าต้องการ)
    public void ResetCheckpoint()
    {
        activated = false;
        if (sr != null)
        {
            sr.color = inactiveColor;
        }
    }
}