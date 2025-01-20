using UnityEngine;
using UnityEngine.UI;

public class RetryButton : MonoBehaviour
{
    public void RetryGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
    }
}
