using UnityEngine;
using UnityEngine.Events;

public class EndLevelItemEffect : MonoBehaviour, IItemEffect
{
    [Header("End Level Action")]
    [SerializeField] private bool pauseGame = true;
    [SerializeField] private UnityEvent onLevelEnded;

    public void Apply(GameObject collector)
    {
        Debug.Log("[EndLevelItem] Level ended by pickup.");

        if (pauseGame)
            Time.timeScale = 0f;

        onLevelEnded?.Invoke();
    }
}
