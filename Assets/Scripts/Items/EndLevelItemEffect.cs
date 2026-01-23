using UnityEngine;

public class EndLevelItemEffect : MonoBehaviour, IItemEffect
{
    [SerializeField] private GameOverUI gameOverUI;

    public void Apply(GameObject collector)
    {
        if (gameOverUI == null) return;

        gameOverUI.ShowLevelCompleted();
    }
}
