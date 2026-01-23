using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using TMPro;

public class GameOverUI : MonoBehaviour
{
    [SerializeField] private GameObject gameOverText;
    [SerializeField] private TMP_Text gameOverTMP;
    [SerializeField] private Volume gameOverVolume;
    [SerializeField] private AudioManager audioManager;

    public static bool IsGameOver { get; private set; }

    void Start()
    {
        IsGameOver = false;
        gameOverText.SetActive(false);

        if (gameOverVolume != null)
            gameOverVolume.weight = 0f;
    }

    void Update()
    {
        if (!IsGameOver) return;

        if (Keyboard.current.anyKey.wasPressedThisFrame ||
            (Gamepad.current != null && Gamepad.current.allControls.Any(c => c.IsPressed())))
        {
            QuitGame();
        }
    }

    /*    private void Show(string message)
        {
            if (IsGameOver) return;

            IsGameOver = true;

            gameOverText.SetActive(true);

            if (gameOverTMP != null)
                gameOverTMP.text = message;

            if (gameOverVolume != null)
                gameOverVolume.weight = 1f;

            Time.timeScale = 0f;
        }*/

    private void Show(string message, bool usePostProcess)
    {
        if (IsGameOver) return;

        IsGameOver = true;

        gameOverText.SetActive(true);

        if (gameOverTMP != null)
            gameOverTMP.text = message;

        if (gameOverVolume != null)
            gameOverVolume.weight = usePostProcess ? 1f : 0f;

        Time.timeScale = 0f;
    }


    public void ShowGameOverFromPlayer()
    {
        if (AudioManager.Instance != null)
            AudioManager.Instance.StopMusicAndLock();

        Show("GAME OVER\nPress any key", true);
    }

    public void ShowLevelCompleted()
    {
        Show("LEVEL COMPLETED\nPress any key", false);
    }

    private void QuitGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }
}
