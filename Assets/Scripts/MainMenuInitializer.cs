using UnityEngine;

public class MainMenuInitializer : MonoBehaviour
{
    private void Start()
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.UnlockMusic();
        }
    }
}
