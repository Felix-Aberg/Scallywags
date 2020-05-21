using UnityEngine;
using UnityEngine.SceneManagement;

namespace ScallyWags
{

    public class PauseGame : MonoBehaviour
    {
        private bool _pause;
        [SerializeField] private GameObject _pauseCanvas;
        private int _maxPlayers = 4;
        private string _backKey = "Interact";
        private LevelEventManager _levelEventManager;
        private Main _main;

        private void Start()
        {
            _levelEventManager = FindObjectOfType<LevelEventManager>();
            _main = FindObjectOfType<Main>();
        }
        
        void Update()
        {
            if (_pause)
            {
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
                
                if (Input.GetButton("Back"))
                {
                    _levelEventManager.SetLevelPlayState(LevelEventManager.LevelPlayState.Quit);
                    SceneManager.LoadSceneAsync("LoseScene");
                }
            }
            
            for (int i = 1; i <= _maxPlayers; i++)
            {
                HandleInput(i);
            }
            
            if (Input.GetButtonDown("Back") || Input.GetKeyDown(KeyCode.Escape))
            {
                _pause = !_pause;
            }

            _pauseCanvas.SetActive(_pause);
            _main.PauseGame(_pause);
        }

        private void HandleInput(int i)
        {
            var button = _backKey + i;
            if (Input.GetButtonDown(button))
            {
                _pause = false;
            }
        }
    }
}
