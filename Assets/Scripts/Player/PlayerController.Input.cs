using Alteruna;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Net.Sockets; // 👈 THÊM DÒNG NÀY
using System.Threading;   // 👈 VÀ CẢ DÒNG NÀY
using System.Text;
using System.Net;
using System;
using System.Collections.Generic;

namespace VRGameMobile
{
    public partial class PlayerController
    {
        private SyncedAxis _horizontal;
        private SyncedAxis _vertical;

        private SyncedKey _jump;
        private SyncedKey _sprint;
        private SyncedKey _reload;
        private SyncedKey _camera;

        private InputSynchronizable _input;

        private float MouseX => Input.GetAxisRaw("Mouse X");
        private float MouseY => Input.GetAxisRaw("Mouse Y");

        public string serverIP = "192.168.137.188";
        private string logHistory = "";
        private const int MaxLogLines = 20;
        private Quaternion receivedRotation = Quaternion.Euler(0f, 0.53f, 0f);
        public float rotationStep = 0.01f;
        private List<string> dataBuffer = new List<string>(); // Buffer tạm cho dữ liệu UDP

        private Vector3 smoothedAimPoint; // Lưu điểm nhắm mượt
        private float lastUpdateTime; // Thời gian cập nhật lần cuối
        private const float UPDATE_INTERVAL = 0.02f; // 50Hz
        private Quaternion[] rotationBuffer = new Quaternion[5]; // Bộ đệm quaternion
        private int bufferIndex = 0;
        private int bufferCount = 0;

        [SerializeField] private float positionSmoothingFactor = 15f; // Làm mượt vị trí súng
        [SerializeField] private float rotationSmoothingFactor = 20f; // Làm mượt quaternion
        [SerializeField] private float aimPointSmoothingFactor = 10f; // Làm mượt điểm nhắm
        [SerializeField] private Vector3 coordinateOffset = new Vector3(90, 0, 0); //
        private float lastDataTime;
        private readonly object bufferLock = new object(); // Đồng bộ thread
        private float lastEnqueueTime; // Thời gian enqueue lần cuối
        private Queue<Quaternion> rotationQueue = new Queue<Quaternion>(10); // Hàng đợi với kích thước tối đa 10
        private const float ENQUEUE_INTERVAL = 0.01f; // Giới hạn tần suất enqueue
        private bool mpuConnected = false;
        private Vector2 imuMouseDelta = Vector2.zero;
        private float sensitivity = 0.01f; // Điều chỉnh theo nhu cầu
        private bool fireButtonPressed = false;
        private bool zoomButtonPressed = false;
        private Vector3 lastGyro = Vector3.zero;
        private Vector3 lastEulerAngles = Vector3.zero;
        private float aimRange = 5f;
        private float aimSpeed = 10f;
        private Vector2 aimPoint2D = Vector2.zero;
        private Vector3 gyroBias = Vector3.zero;
        [SerializeField] private float gyroSensitivity = 0.01f;
        [SerializeField] private float deadzone = 0.1f;
        private bool isCalibrating = false;
        private Vector2 smoothedGyroDelta = Vector2.zero;
        [SerializeField] private float smoothingAlpha = 0.1f;
        private void InitializeInput()
        {
            if (_isOwner)
            {
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
            }

            _input = GetComponent<InputSynchronizable>();

            _horizontal = new SyncedAxis(_input, "Horizontal");
            _vertical = new SyncedAxis(_input, "Vertical");

            _jump = new SyncedKey(_input, KeyCode.Space, SyncedKey.KeyMode.KeyDown);
            _sprint = new SyncedKey(_input, KeyCode.LeftShift);
            _reload = new SyncedKey(_input, KeyCode.R, SyncedKey.KeyMode.KeyDown);
            _camera = new SyncedKey(_input, KeyCode.V, SyncedKey.KeyMode.ToggleKeyDown);
        }



        void OnApplicationQuit()
        {
        }
    }
}