using UnityEngine;

public class TeleportPoint : MonoBehaviour
{
    public Vector3 position => transform.position;
    public Quaternion rotation; // Góc xoay tại mốc này (có thể tùy chỉnh trong Inspector)
    public Light flagLight;


    private void Awake()
    {
       

        if (flagLight != null)
        {
            flagLight.enabled = false; // ❌ Mặc định tắt đèn
        }
    }
    public void SetLight(bool on)
    {
        if (flagLight != null)
        {
            flagLight.enabled = on;
        }

    }
}