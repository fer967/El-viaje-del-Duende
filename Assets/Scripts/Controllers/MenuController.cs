using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadScene("Bosque1");
    }

    public void ExitGame()
    {
        Application.Quit();
        Debug.Log("Juego cerrado");
    }

    public void OpenConfig()
    {
        // Pod�s abrir un panel de opciones m�s adelante
        Debug.Log("Abrir configuraci�n");
    }
}


