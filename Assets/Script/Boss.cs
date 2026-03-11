using UnityEngine;
using System.Collections;

public class Boss : MonoBehaviour
{
    [Header("References")]
    public GameObject referenceObject;  // Object ที่จะแสดง/ซ่อน

    [Header("Timing Settings")]
    public float delayBeforeShow = 0f;      // เวลารอก่อนแสดง (วินาที)
    public float showDuration = 3f;         // เวลาที่แสดงอยู่ (วินาที)

    [Header("Options")]
    public bool canTriggerMultipleTimes = false;  // สามารถ Trigger ซ้ำได้หรือไม่
    public bool resetOnPlayerDeath = true;        // Reset เมื่อ Player ตาย

    private bool hasTriggered = false;
    private Coroutine activeCoroutine;

    void Start()
    {
        // ซ่อน Object ตอนเริ่มต้น
        if (referenceObject != null)
        {
            referenceObject.SetActive(false);
        }
        else
        {
            Debug.LogWarning("Reference Object ยังไม่ได้กำหนด!");
        }

        // Subscribe to Player Die Event
        if (resetOnPlayerDeath && GameManager.Instance != null)
        {
            GameManager.Instance.OnPlayerDie.AddListener(ResetSpawner);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // เช็คว่าสามารถ Trigger ได้หรือไม่
            if (!hasTriggered || canTriggerMultipleTimes)
            {
                TriggerObject();
            }
        }
    }

    void TriggerObject()
    {
        hasTriggered = true;

        // ถ้ามี Coroutine เก่าอยู่ ให้หยุดก่อน
        if (activeCoroutine != null)
        {
            StopCoroutine(activeCoroutine);
        }

        // เริ่ม Coroutine ใหม่
        activeCoroutine = StartCoroutine(ShowObjectSequence());
    }

    IEnumerator ShowObjectSequence()
    {
        // 1. รอตามเวลาที่กำหนดก่อนแสดง
        if (delayBeforeShow > 0)
        {
            yield return new WaitForSeconds(delayBeforeShow);
        }

        // 2. แสดง Object
        if (referenceObject != null)
        {
            referenceObject.SetActive(true);
            Debug.Log($"{referenceObject.name} ปรากฏ!");
        }

        // 3. รอตามเวลาที่แสดง
        yield return new WaitForSeconds(showDuration);

        // 4. ซ่อน Object
        if (referenceObject != null)
        {
            referenceObject.SetActive(false);
            Debug.Log($"{referenceObject.name} หายไป!");
        }

        activeCoroutine = null;
    }

    public void ResetSpawner()
    {
        // หยุด Coroutine ถ้ากำลังทำงานอยู่
        if (activeCoroutine != null)
        {
            StopCoroutine(activeCoroutine);
            activeCoroutine = null;
        }

        // ซ่อน Object
        if (referenceObject != null)
        {
            referenceObject.SetActive(false);
        }

        // Reset state
        hasTriggered = false;

        Debug.Log("TimedObjectSpawner Reset เรียบร้อย");
    }

    void OnDestroy()
    {
        if (resetOnPlayerDeath && GameManager.Instance != null)
        {
            GameManager.Instance.OnPlayerDie.RemoveListener(ResetSpawner);
        }
    }
}