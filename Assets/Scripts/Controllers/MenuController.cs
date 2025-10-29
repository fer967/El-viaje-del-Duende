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
        // Ahora usa el UIManager global
        if (UIManager.Instance != null)
        {
            UIManager.Instance.ExitGame();
        }
        else
        {
            // Fallback por si UIManager no est� disponible
            Application.Quit();
            Debug.Log("Juego cerrado (fallback)");
        }
    }

    
    public void OpenConfig()
    {
        // Pod�s abrir un panel de opciones m�s adelante
        Debug.Log("Abrir configuraci�n");
    }
}


