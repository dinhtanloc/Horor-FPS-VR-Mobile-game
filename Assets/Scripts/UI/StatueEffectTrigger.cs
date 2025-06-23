using UnityEngine;
using System.Collections;
using TMPro;

public class StatueEffectTrigger : MonoBehaviour
{
    [Header("Target References")]
    public Transform player; // Tham chiếu đến người chơi
    public GameObject messagePanel; // Panel chứa văn bản
    public TMP_Text messageText; // Kéo TMP_Text vào đây

    [Header("Settings")]
    public float activationDistance = 3f; // Khoảng cách kích hoạt hiệu ứng
    public float typingSpeed = 0.05f; // Tốc độ chạy chữ (giây/ký tự)

    private bool hasActivated = false;
    private CanvasGroup canvasGroup;

    private void Start()
    {
        if (player == null)
        {
            Debug.LogError("Player transform is not assigned in StatueEffectTrigger.");
        }

        if (messagePanel == null)
        {
            Debug.LogError("Message panel is not assigned in StatueEffectTrigger.");
        }

        messagePanel.SetActive(false);
    }

    void Update()
    {
        if (player == null || messagePanel == null)
            return;

        float distance = Vector3.Distance(transform.position, player.position);

        if (distance <= activationDistance && !hasActivated)
        {
            // Debug.Log("Statue effect triggered by player at distance: " + distance);
            messagePanel.SetActive(true);
            StartCoroutine(FadeInText());
            hasActivated = true;
        }
    }

    IEnumerator FadeInText()
    {
        // Lấy hoặc thêm CanvasGroup
        canvasGroup = messagePanel.GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = messagePanel.AddComponent<CanvasGroup>();
        }

        // Reset text nếu cần
        if (messageText != null)
        {
            messageText.text = ""; // Bắt đầu từ rỗng
        }

        float fadeDuration = 1f;
        float elapsed = 0f;

        // FADE IN BLACK OVERLAY
        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            canvasGroup.alpha = Mathf.Clamp01(elapsed / fadeDuration);
            yield return null;
        }

        canvasGroup.alpha = 1f;

        // TYPING EFFECT CHO VĂN BẢN
        string fullText = "Chỉ dẫn ngàn ma quái: Tại bệnh viện âm u này có một hệ thống quỷ cấp bậc đang thống trị và kiểm soát nơi đây, đứng đầu là tên thờ lĩnh quỷ ngàn ma quái. Bạn cần phải khám phá những bí mật động trời được ẩn giấu đằng sau hành trình và đánh bại lũ yêu quỷ để mở khóa bí mật lớn nhất thời đại";

        if (messageText != null)
        {
            for (int i = 0; i < fullText.Length; i++)
            {
                messageText.text = fullText.Substring(0, i + 1); // Hiển thị từng ký tự
                yield return new WaitForSeconds(typingSpeed); // Delay giữa các ký tự
            }
        }

        // GIỮ NGUYÊN CHỮ TRONG 3 GIÂY
        yield return new WaitForSeconds(3f);

        // FADE OUT BLACK OVERLAY
        elapsed = 0f;
        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            canvasGroup.alpha = 1f - (elapsed / fadeDuration);
            yield return null;
        }

        canvasGroup.alpha = 0f;
        messagePanel.SetActive(false);
    }
}