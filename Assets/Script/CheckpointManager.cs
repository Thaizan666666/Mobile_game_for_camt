using UnityEngine;

public class CheckpointManager : MonoBehaviour
{
    public static CheckpointManager Instance { get; private set; }

    private Vector3 respawnPoint;
    private bool hasCheckpoint = false;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void SetCheckpoint(Vector3 position)
    {
        respawnPoint = position;
        hasCheckpoint = true;
        Debug.Log($"Checkpoint updated: {position}");
    }

    public Vector3 GetRespawnPoint(Vector3 defaultPosition)
    {
        return hasCheckpoint ? respawnPoint : defaultPosition;
    }
}