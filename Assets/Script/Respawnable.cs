using UnityEngine;

public class Respawnable : MonoBehaviour
{
    private Vector3 startPosition;
    private Quaternion startRotation;
    private bool startActive;

    void Awake()
    {
        startPosition = transform.position;
        startRotation = transform.rotation;
        startActive = gameObject.activeSelf;
    }

    // เรียกตอน Respawn
    public void ResetObject()
    {
        transform.position = startPosition;
        transform.rotation = startRotation;
        gameObject.SetActive(startActive);
    }
}