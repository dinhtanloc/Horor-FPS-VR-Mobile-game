Dưới đây là toàn bộ nội dung `README.md` được **bao bọc trong dấu ngoặc kép `"`**, đảm bảo **không bị mất định dạng khi sử dụng trong mã nguồn hoặc chuỗi Python**, bạn có thể copy trực tiếp:

````markdown
"# 🧠 Abandoned Asylum Adventure - Mobile VR Unity Game with ESP32 Controller

Welcome to **Abandoned Asylum Adventure**, a mobile VR first-person shooting game designed for immersive interaction at low cost. This project explores how to optimize virtual reality on mobile platforms using Unity, custom hardware, and artificial intelligence — with a strong emphasis on human-computer interaction (HCI), gameplay, and system performance.

![Abandoned Asylum Environment](assets/asylum_map.jpg)

## 🎯 Project Overview

Traditional PC-based VR systems are often expensive and limited by high-end hardware. In contrast, our project proposes a **cost-efficient alternative** using:

- Unity (Mobile VR)
- A custom IoT gun controller (ESP32 + MPU6050)
- AI-based gameplay personalization
- Multiplayer via Photon Networking

### Key Goals:
- Create an immersive and responsive VR experience
- Replace traditional gamepads with a physical gun-shaped motion controller
- Allow real-time mobile interaction using a smartphone (Android)
- Apply machine learning to improve gameplay and user engagement

## 🎮 Game Concept

Set in a haunted asylum map, the player explores and fights monsters using a gun-shaped controller. The game is designed to work with Google Cardboard or similar mobile VR kits, allowing natural hand movement to control aiming and shooting.

Features include:
- 3D VR environment: “Abandoned Asylum” from Unity Asset Store
- Horror + shooter gameplay loop
- Motion tracking-based weapon control
- Multiplayer mode: Co-op and PvP using Photon
- AI enemies with behavior adaptation

![Gameplay Flow](assets/gameplay_flow.png)

## 🕹️ Game Control & Interaction (HCI)

### 🔫 Hardware-based Gun Controller
- Built using **ESP32** and **MPU6050**
- Captures real-world hand orientation (pitch, roll, yaw)
- Data processed and sent to Unity using UDP protocol
- Buttons map to shooting / interaction

### 🎮 In-Game Mapping
- Aim: Based on MPU6050 orientation
- Fire: Trigger button on ESP32 board
- Move: Head rotation via Google Cardboard

```plaintext
[ESP32] --(MPU6050)--> Process motion --> UDP --> [Unity (Mobile VR)]
````

## 🤖 AI & Machine Learning Integration

To increase user engagement, AI modules are trained on player behavior:

* Recognize player strategies (aggressive, defensive, etc.)
* Adapt bot difficulty and placement accordingly
* Collect sensor + gameplay logs for analysis

We trained and compared multiple models (Random Forest, LSTM, MLB). **MLB** was chosen for its balance of speed and performance on mobile devices.

| Model         | Accuracy | F1-Score | AUC    |
| ------------- | -------- | -------- | ------ |
| Random Forest | 0.83     | 0.80     | 0.86   |
| LSTM          | 0.90     | 0.90     | 0.93   |
| **MLB**       | 0.89     | 0.88     | 0.91 ✅ |

### AI Use Cases

* Smart enemy behavior
* Dynamic level adaptation
* Player skill assessment

![AI Integration](assets/ai_pipeline.png)

## 🌐 Multiplayer Architecture

Using **Photon Unity Networking (PUN)**, we allow:

* Real-time multiplayer gameplay (low server cost)
* Player-vs-player or team-vs-bots scenarios
* VR sessions synchronized across devices

This makes the game scalable, suitable for training simulations, STEM education, and emergency drills.

## ⚙️ Technical Architecture

| Component         | Tech/Protocol               |
| ----------------- | --------------------------- |
| VR Engine         | Unity + Google Cardboard    |
| Controller Board  | ESP32                       |
| Motion Sensor     | MPU6050                     |
| Communication     | UDP (Wi-Fi LAN)             |
| Multiplayer       | Photon Unity Networking     |
| AI Model Training | Python (TensorFlow/sklearn) |

## 💡 Development Highlights

* Built with Unity + PlatformIO
* Kalman Filter for smoothing MPU6050 output
* Custom asset integration for visual horror experience
* Easy deployment on Android smartphones

## 🚀 Advantages

* ✅ **Low Cost**: Uses affordable electronics and open-source tools
* ✅ **Immersive**: Realistic aiming, not just button pressing
* ✅ **Scalable**: Can be expanded to training or education scenarios
* ✅ **Intelligent**: AI bots adapt to players’ behaviors

## ⚠️ Known Limitations

* Sensor drift may impact long-term precision
* Performance depends on Wi-Fi quality for real-time communication
* Still less immersive than PC-based VR systems

## 📈 Future Development

* Add dual-controller support for full hand tracking
* Improve multiplayer matchmaking system
* Use BNO055 sensor for better orientation tracking
* Add haptics (vibration motor) to enhance realism
* Expand to emergency response training and simulation

## 📽️ Demo Preview

![Gameplay Demo](assets/demo.gif)


## 📂 Repository Structure

```
/unity-project/       ← Unity VR Game Source
/platformio/          ← ESP32 firmware using PlatformIO
/assets/              ← Images, GIFs, diagrams
/ml/                  ← Machine learning training scripts
```

## 🧩 References

* Unity Asset: *Abandoned Asylum* (2019)
* Stack Overflow: UDP vs TCP (2008)
* Newzoo Global Games Market Report (2022)
  "

```

---

✅ Bạn có thể chép chuỗi trên vào file `.py`, `.json`, hoặc bất kỳ hệ thống nào yêu cầu chuỗi giữ nguyên format Markdown.

Nếu bạn muốn chia nhỏ chuỗi theo từng đoạn hoặc chuyển sang file `.md` thực tế, mình có thể tạo giúp ngay. Bạn cũng có thể yêu cầu mình tạo ảnh sơ đồ hoặc minh họa như `gameplay_flow.png` và `ai_pipeline.png`. Muốn không?
```
