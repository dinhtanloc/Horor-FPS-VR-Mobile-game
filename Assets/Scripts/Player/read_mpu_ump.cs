using System.Collections.Concurrent;
using System.Text;
using UnityEngine;
using System.Net.Sockets;
using TMPro;
using System;
using System.Net;
using System.Threading;

public class read_mpu_udp : MonoBehaviour
{
    public Gun gun;
    public Vector3 rotationOffset;
    public float speedFactor = 15.0f;
    public string imuName = "r";
    public Camera cam;
    public string serverIP = "192.168.137.188";
    public int port = 8080;

    private UdpClient udpClient;
    private Thread receiveThread;
    private bool isRunning = false;
    private bool isConnected = false;

    public Transform cameraRig;

    private Quaternion targetRotation = Quaternion.identity;
    //private ConcurrentQueue<Action> mainThreadActions = new ConcurrentQueue<Action>();

    private string logHistory = "";
    private const int MaxLogLines = 20;

    void Start()
    {
        isRunning = true;
        receiveThread = new Thread(ReceiveDataUDP);
        receiveThread.IsBackground = true;
        receiveThread.Start();

        SendInitialPing();
    }

    void Update()
    {
 

        cam.transform.localRotation = Quaternion.Slerp(
            cam.transform.localRotation,
            targetRotation,
            Time.deltaTime * speedFactor
        );
    }

    void SendInitialPing()
    {
        UdpClient sender = new UdpClient();
        IPEndPoint target = new IPEndPoint(IPAddress.Parse(serverIP), port);

        byte[] data = Encoding.UTF8.GetBytes("r,1.0,0.0,0.0,0.0");
        sender.Send(data, data.Length, target);

        AppendLog("📤 Đã gửi gói khởi động tới ESP");
        sender.Close();
    }

    void ReceiveDataUDP()
    {
        try
        {
            udpClient = new UdpClient(port); // Listen on the given port
            UnityMainThreadDispatcher.Enqueue(() =>
            {
                AppendLog("📡 Đang lắng nghe trên cổng: " + port);
                isConnected = true;
            });

            IPEndPoint remoteEndPoint = new IPEndPoint(IPAddress.Any, port);

            while (isRunning)
            {
                byte[] data = udpClient.Receive(ref remoteEndPoint);
                string message = Encoding.UTF8.GetString(data).Trim();

                UnityMainThreadDispatcher.Enqueue(() =>
                {
                    AppendLog($"📥 Nhận được gói tin từ {remoteEndPoint.Address}:{remoteEndPoint.Port}");
                });

                string[] lines = message.Split('\n');
                foreach (string line in lines)
                {
                    string cleanLine = line.Trim();
                    if (!string.IsNullOrWhiteSpace(cleanLine))
                    {
                        HandleData(cleanLine);
                    }
                }
            }
        }
        catch (Exception e)
        {
            Debug.LogWarning("UDP Receive error: " + e.Message);
            UnityMainThreadDispatcher.Enqueue(() =>
            {
                AppendLog("⚠️ Lỗi khi nhận dữ liệu UDP: " + e.Message);
            });
        }
    }

    void HandleData(string receivedData)
    {
        string[] values = receivedData.Split(',');
        Debug.Log($"Received data: {receivedData}");

        if (values.Length == 5 && values[0] == imuName)
        {
            if (float.TryParse(values[1], out float w) &&
                float.TryParse(values[2], out float x) &&
                float.TryParse(values[3], out float y) &&
                float.TryParse(values[4], out float z))
            {
                Quaternion imuRotation = new Quaternion(x, y, z, w);
                UnityMainThreadDispatcher.Enqueue(() =>
                {
                    targetRotation = imuRotation;
                    string log = $"⏱ {Time.time:F2}s\nQuaternion:\nw:{w:F2}, x:{x:F2}, y:{y:F2}, z:{z:F2}";
                    AppendLog(log);
                });
            }
        }
        else if (values.Length == 1 && values[0] == "1")
        {
            UnityMainThreadDispatcher.Enqueue(() =>
            {
                gun.Shoot();
                AppendLog("🎯 Action: Fire!");
            });
        }
        else
        {
            UnityMainThreadDispatcher.Enqueue(() =>
            {
                AppendLog("⚠ Unknown data: " + receivedData);
            });
        }
    }

    void AppendLog(string message)
    {
        string timeStamp = $"[ {DateTime.Now:HH:mm:ss} ]  ";
        string newLog = timeStamp + message;

        var lines = (logHistory + "\n" + newLog).Split('\n');
        if (lines.Length > MaxLogLines)
        {
            logHistory = string.Join("\n", lines[^MaxLogLines..]);
        }
        else
        {
            logHistory = string.Join("\n", lines);
        }

    }

    void OnApplicationQuit()
    {
        isRunning = false;

        try
        {
            udpClient?.Close();
            if (receiveThread != null && receiveThread.IsAlive)
                receiveThread.Join(100);// Hoặc dùng join/killsafe
        }
        catch
        {
            // Không log lỗi khi thoát
            // Không log lỗi khi thoát
        }
    }
}
