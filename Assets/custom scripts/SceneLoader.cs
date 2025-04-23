using UnityEngine;
using UnityEngine.SceneManagement; // This allows us to change scenes.

public class SceneLoader : MonoBehaviour
{
    // This method loads a scene based on its name.
    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}