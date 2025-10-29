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
            // Fallback por si UIManager no está disponible
            Application.Quit();
            Debug.Log("Juego cerrado (fallback)");
        }
    }

    
    public void OpenConfig()
    {
        // Podés abrir un panel de opciones más adelante
        Debug.Log("Abrir configuración");
    }
}


