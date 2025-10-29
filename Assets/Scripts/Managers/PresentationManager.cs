using UnityEngine;
using UnityEngine.SceneManagement;

public class PresentationManager : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadScene("MenuInicio");
    }
}
