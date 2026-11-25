
using UnityEngine;

public class ARObjectController : MonoBehaviour
{
    [Header("Target AR")]
    public Transform targetObject;
    public string targetTag = "ARObject";

    [Header("Rotation")]
    public float rotateStep = 15f;

    [Header("Scale")]
    public float scaleFactor = 1.15f;
    public float minScale = 0.2f;
    public float maxScale = 3.0f;

    [Header("Optional Feedback")]
    public AudioSource audioSource;

    void Start()
    {
        FindTargetIfNeeded();
    }

    public void FindTargetIfNeeded()
    {
        if (targetObject != null) return;

        GameObject found = GameObject.FindGameObjectWithTag(targetTag);

        if (found != null)
        {
            Transform pivot = found.transform.Find("ModelPivot");

            if (pivot != null)
            {
                targetObject = pivot;
                Debug.Log("ModelPivot berhasil ditemukan!");
            }
            else
            {
                Debug.LogWarning("ModelPivot tidak ditemukan pada objek AR!");
            }
        }
    }

    private bool TargetReady()
    {
        if (targetObject == null)
        {
            FindTargetIfNeeded();
        }

        if (targetObject == null)
        {
            Debug.LogWarning(
                "Target AR belum ditemukan. Scan marker terlebih dahulu."
            );

            return false;
        }

        return true;
    }

    public void RotateLeft()
    {
        if (!TargetReady()) return;

        targetObject.Rotate(0f, -rotateStep, 0f, Space.Self);

        Debug.Log("Objek AR berputar ke kiri!");

        PlayFeedback();
    }

    public void RotateRight()
    {
        if (!TargetReady()) return;

        targetObject.Rotate(0f, rotateStep, 0f, Space.Self);

        Debug.Log("Objek AR berputar ke kanan!");

        PlayFeedback();
    }

    public void ScaleUp()
    {
        if (!TargetReady()) return;

        ApplyScale(scaleFactor);

        Debug.Log("Ukuran objek AR diperbesar!");

        PlayFeedback();
    }

    public void ScaleDown()
    {
        if (!TargetReady()) return;

        ApplyScale(1f / scaleFactor);

        Debug.Log("Ukuran objek AR diperkecil!");

        PlayFeedback();
    }

    private void ApplyScale(float factor)
    {
        Vector3 current = targetObject.localScale;

        float currentX = Mathf.Max(current.x, 0.0001f);

        float desiredX = Mathf.Clamp(
            currentX * factor,
            minScale,
            maxScale
        );

        float ratio = desiredX / currentX;

        targetObject.localScale = current * ratio;
    }

    private void PlayFeedback()
    {
        if (audioSource != null)
        {
            audioSource.Play();
        }
    }
}