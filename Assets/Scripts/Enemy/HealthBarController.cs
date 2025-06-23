using UnityEngine;
using UnityEngine.UI;

public class HealthBarController : MonoBehaviour
{
    public Image fillBar; // Kéo `FillBar` vào đây từ Inspector
    private float maxHealth = 100f; // Máu tối đa
    private float currentHealth = 100f; // Máu hiện tại

    void Update()
    {
        if (fillBar != null)
        {
            // Cập nhật fill amount dựa trên máu còn lại
            float fillAmount = Mathf.Clamp01(currentHealth / maxHealth);
            fillBar.fillAmount = fillAmount;
        }
    }

    public void SetMaxHealth(float value)
    {
        maxHealth = value;
        currentHealth = maxHealth;
        UpdateHealthBar();
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        UpdateHealthBar();
    }

    private void UpdateHealthBar()
    {
        if (fillBar != null)
        {
            float fillAmount = Mathf.Clamp01(currentHealth / maxHealth);
            fillBar.fillAmount = fillAmount;
        }
    }
}