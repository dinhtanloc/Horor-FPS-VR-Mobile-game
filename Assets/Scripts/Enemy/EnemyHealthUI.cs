using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthUI : MonoBehaviour
{
    public RectTransform healthBarFill; // Khung máu
    private float maxHealth = 100f; // Máu tối đa
    private float currentHealth = 100f; // Máu hiện tại

    void Update()
    {
        if (healthBarFill != null)
        {
            // Cập nhật fill amount dựa trên máu còn lại
            float fillAmount = Mathf.Clamp01(currentHealth / maxHealth);
            healthBarFill.localScale = new Vector3(fillAmount, 1f, 1f); // Điều chỉnh chiều rộng
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
        if (healthBarFill != null)
        {
            float fillAmount = Mathf.Clamp01(currentHealth / maxHealth);
            healthBarFill.localScale = new Vector3(fillAmount, 1f, 1f);
        }
    }
}