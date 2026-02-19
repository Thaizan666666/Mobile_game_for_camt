using UnityEngine;
using Unity.Cinemachine;



public class CameraZoneController : MonoBehaviour
{
    [Header("References")]
    public Transform player;
    public Transform cameraAnchor;

    [Header("Mario Camera Settings")]
    public float rightTrigger = 2f;   // เดินขวาเกินนี้ กล้องตาม
    public float leftTrigger = -3f;   // เดินซ้ายเกินนี้ กล้องตาม

    private float maxCameraX;         // ค่า X มากสุดที่กล้องเคยไป

    void Start()
    {
        maxCameraX = cameraAnchor.position.x;
    }

    void LateUpdate()
    {
        float playerX = player.position.x;
        float anchorX = cameraAnchor.position.x;

        // คำนวณ offset ของ player เทียบกับกล้อง
        float diff = playerX - anchorX;

        float targetX = anchorX;

        if (diff > rightTrigger)
        {
            // Player เดินเกินขวา → กล้องตาม
            targetX = playerX - rightTrigger;
            maxCameraX = Mathf.Max(maxCameraX, targetX);
        }
        else if (diff < leftTrigger)
        {
            // Player เดินเกินซ้าย → กล้องตาม (แต่ไม่เกิน maxCameraX ที่เคยไป)
            targetX = playerX - leftTrigger;
            // ไม่อัพเดท maxCameraX เพราะกล้องถอยซ้าย
        }
        else
        {
            // อยู่ในโซน → กล้องหยุดนิ่ง
            targetX = anchorX;
        }

        // กล้องไม่ถอยเกินซ้ายสุดที่เคยไป (optional)
        // targetX = Mathf.Min(targetX, maxCameraX);

        Vector3 pos = cameraAnchor.position;
        pos.x = Mathf.Lerp(anchorX, targetX, Time.deltaTime * 8f);
        cameraAnchor.position = pos;
    }

    void OnDrawGizmos()
    {
        if (cameraAnchor == null) return;

        float anchorX = cameraAnchor.position.x;

        Gizmos.color = Color.green;
        Gizmos.DrawLine(new Vector3(anchorX + rightTrigger, -10, 0),
                        new Vector3(anchorX + rightTrigger, 10, 0));

        Gizmos.color = Color.red;
        Gizmos.DrawLine(new Vector3(anchorX + leftTrigger, -10, 0),
                        new Vector3(anchorX + leftTrigger, 10, 0));
    }
}