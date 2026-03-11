using UnityEngine;
using UnityEngine.SceneManagement;

public class WinTrigger : MonoBehaviour
{
    [Header("Scene Settings")]
    public string winSceneName = "Win";  // ชื่อ Scene ที่จะโหลด

    [Header("Visual Feedback")]
    public bool playSound = true;        // เล่นเสียง
    public AudioClip winSound;           // เสียงชนะ

    private bool hasTriggered = false;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !hasTriggered)
        {
            hasTriggered = true;
            TriggerWin();
        }
    }

    void TriggerWin()
    {
        Debug.Log("Player ชนะ! กำลังโหลด Win Scene...");

        SoundManager.StopAllLoops();
        SoundManager.PlayWinMusic();
        LoadWinScene();
    }

    void LoadWinScene()
    {
        SceneManager.LoadScene(winSceneName);
    }
}