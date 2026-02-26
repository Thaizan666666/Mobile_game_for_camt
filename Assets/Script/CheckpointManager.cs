using UnityEngine;

public class CheckpointManager : MonoBehaviour
{
    public static CheckpointManager Instance { get; private set; }

    private Vector3 respawnPoint;
    private bool hasCheckpoint = false;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);  // ✅ ไม่ให้ถูกทำลายเมื่อเปลี่ยน Scene
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetCheckpoint(Vector3 position)
    {
        respawnPoint = position;
        hasCheckpoint = true;
        Debug.Log($"Checkpoint saved at {position}");
    }

    public Vector3 GetRespawnPoint(Vector3 defaultPosition)
    {
        return hasCheckpoint ? respawnPoint : defaultPosition;
    }

    // ✅ Reset Checkpoint (เมื่อ Restart Level)
    public void ResetCheckpoints()
    {
        hasCheckpoint = false;
        Debug.Log("All checkpoints reset");
    }
}