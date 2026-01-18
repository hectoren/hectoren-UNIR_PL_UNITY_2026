using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

public class GameOverUI : MonoBehaviour
{
    [SerializeField] private GameObject gameOverText;
    [SerializeField] private Volume gameOverVolume;

    private bool gameOver;

    void Start()
    {
        gameOverText.SetActive(false);

        Player player = FindObjectOfType<Player>();
        if (player != null)
        {
            HealthSystem health = player.GetComponent<HealthSystem>();
            if (health != null)
                health.OnDeath += ShowGameOver;
        }
    }

    void Update()
    {
        if (!gameOver) return;

        if (Keyboard.current.anyKey.wasPressedThisFrame)
        {
            QuitGame();
        }
    }

    private void ShowGameOver()
    {
        gameOver = true;
        gameOverText.SetActive(true);

        if (gameOverVolume != null)
            gameOverVolume.weight = 1f;

        Time.timeScale = 0f;
    }

    private void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
