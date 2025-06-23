using UnityEngine;
using System.Collections;
public class EnemyHealth : MonoBehaviour
{
    public float health = 100f; // Máu ban đầu của quái
    public GameObject bloodEffectPrefab; // Prefab hiệu ứng máu
    public Transform hitPoint; // Vị trí trúng đạn (nếu bạn muốn spawn tại vị trí chính xác)
    private HealthBarController healthBarController;
    private void Start()
    {
        // Ẩn hiệu ứng máu ban đầu
        bloodEffectPrefab.SetActive(false);
        if (healthBarController != null)
        {
            healthBarController = GetComponent<HealthBarController>();
            healthBarController.SetMaxHealth(health);
        }
    }
    public void TakeDamage(float damage, Vector3 hitPosition)
    {
        health -= damage;
        ShowBloodEffect();

        // Hiển thị hiệu ứng máu
        if (bloodEffectPrefab != null)
        {
            // Spawn hiệu ứng máu tại điểm trúng đạn
            GameObject blood = Instantiate(bloodEffectPrefab, hitPosition, Quaternion.identity);
            Destroy(blood, 0.5f); // Tự hủy sau vài giây
        }
        if (healthBarController != null)
        {
            healthBarController.TakeDamage(damage);
            // Debug.Log($"Quái vật bị thương: {health} máu còn lại.");
        }
        else
        {
            Debug.LogWarning("HealthBarController không được gán trong EnemyHealth.");
        }

        if (health <= 0f)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("Quái vật chết!");
        // Có thể thêm hiệu ứng nổ, âm thanh hoặc tắt gameObject
        Destroy(gameObject);
    }
    public void ShowBloodEffect()
    {
        // Hiện hiệu ứng máu
        bloodEffectPrefab.SetActive(true);

        // Tự động ẩn sau vài giây (ví dụ: 2 giây)
        StartCoroutine(HideAfterDelay());
    }

    private IEnumerator HideAfterDelay()
    {
        yield return new WaitForSeconds(2f); // Đợi 2 giây
        bloodEffectPrefab.SetActive(false);
    }
}