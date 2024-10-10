using UnityEngine;
using UnityEngine.SceneManagement;

public class OnClickPlay : MonoBehaviour
{
    public void LoadNextSceneInBuild()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex + 1); // Loads the next scene in build order
    }
}
