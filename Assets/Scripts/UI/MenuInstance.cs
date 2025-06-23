using UnityEngine;
using UnityEngine.SceneManagement;

namespace VRGameMobile
{
    public class MenuInstance : MonoBehaviour
    {
        public static MenuInstance Instance { get; private set; }

        [SerializeField] private GameObject _menuObject;

        private void Awake()
        {
            Instance = this;
            _menuObject.SetActive(false);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                ToggleMenu();
            }
        }

        private void ToggleMenu()
        {
            bool isActive = !_menuObject.activeSelf;
            _menuObject.SetActive(isActive);
            Cursor.visible = isActive;
            Cursor.lockState = isActive ? CursorLockMode.None : CursorLockMode.Locked;
        }

        /// <summary>
        /// Trả về trạng thái menu (bật/tắt)
        /// </summary>
        public bool IsMenuOpen => _menuObject.activeSelf;

        public void LoadScene(int sceneIndex)
        {
            SceneManager.LoadScene(sceneIndex);
        }

        public void LoadScene(string sceneName)
        {
            SceneManager.LoadScene(sceneName);
        }
    }
}
