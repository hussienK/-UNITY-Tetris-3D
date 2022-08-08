/* < 8 - 8 - 2022 >
 * Hussien kenaan
 * 
 * This script is for Managing Menu clicks and loading scenes
 */
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuHandler : MonoBehaviour
{
    public void LoadScene(string SceneToLoad)
    {
        SceneManager.LoadScene(SceneToLoad);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
