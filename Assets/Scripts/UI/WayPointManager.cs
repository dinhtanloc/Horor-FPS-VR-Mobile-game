using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointManager : MonoBehaviour
{
    public List<Transform> waypoints = new List<Transform>(); // Serializable
    public bool isMoving; // Unchanged
    public int waypointIndex; // Unchanged
    public float moveSpeed = 5f; // Tốc độ di chuyển (có thể điều chỉnh)

    // Start is called before the first frame update
    void Start()
    {
        StartMoving();
    }

    public void StartMoving()
    {
        waypointIndex = 0;
        isMoving = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isMoving)
        {
            return;
        }

        // Di chuyển đến điểm trạm hiện tại
        transform.position = Vector3.MoveTowards(
            transform.position,
            waypoints[waypointIndex].position,
            Time.deltaTime * moveSpeed
        );

        // Kiểm tra khoảng cách đến điểm trạm
        var distance = Vector3.Distance(transform.position, waypoints[waypointIndex].position);
        if (distance <= 0.05f)
        {
            // Di chuyển đến điểm trạm tiếp theo
            waypointIndex++;

            // Kiểm tra nếu đã đến cuối danh sách điểm trạm
            if (waypointIndex >= waypoints.Count)
            {
                // Dừng di chuyển
                isMoving = false;

                // Hoặc bạn có thể lặp lại từ đầu
                // waypointIndex = 0;
            }
        }
    }
}