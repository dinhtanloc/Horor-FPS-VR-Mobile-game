using System.Collections;
using UnityEngine;

public class MultiDoor : MonoBehaviour
{
    public bool IsOpen = false;

    [SerializeField] private float Speed = 5f;
    [SerializeField] private float SlideAmount = 2f; // Khoảng cách trượt (theo trục X)
    [SerializeField] private bool isLeftDoor = true; // Chọn trái/phải

    private Vector3 startPosition;
    private Coroutine animationCoroutine;

    private void Awake()
    {
        startPosition = transform.position;
    }

    public void Open(Vector3 userPosition)
    {
        if (!IsOpen)
        {
            if (animationCoroutine != null)
                StopCoroutine(animationCoroutine);

            animationCoroutine = StartCoroutine(DoSlideOpen());

        }
    }

    public void Close()
    {
        if (IsOpen)
        {
            if (animationCoroutine != null)
                StopCoroutine(animationCoroutine);

            animationCoroutine = StartCoroutine(DoSlideClose());
        }
    }

    private IEnumerator DoSlideOpen()
    {
        Vector3 targetPosition;
        if (isLeftDoor)
        {
            targetPosition = startPosition + Vector3.left * SlideAmount;
        }
        else
        {
            targetPosition = startPosition + Vector3.right * SlideAmount;
        }

        IsOpen = true;

        float time = 0f;
        float duration = 1.0f;

        while (time < duration)
        {
            float t = Mathf.Min(time / duration, 1f);
            t = Mathf.SmoothStep(0f, 1f, t);

            transform.position = Vector3.Lerp(startPosition, targetPosition, t);
            // Debug.Log($"Current Position: {transform.position}");
            time += Time.deltaTime;
            yield return null;
        }

        transform.position = targetPosition;
    }

    private IEnumerator DoSlideClose()
    {
        Vector3 targetPosition = startPosition;

        IsOpen = false;

        float time = 0f;
        float duration = 0.5f;

        while (time < duration)
        {
            float t = Mathf.Min(time / duration, 1f);
            t = Mathf.SmoothStep(0f, 1f, t);

            transform.position = Vector3.Lerp(transform.position, targetPosition, t);
            time += Time.deltaTime;
            yield return null;
        }

        transform.position = targetPosition;
    }
}