using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    public void PlayGame()
    {
        PlayerPrefs.DeleteKey("SpawnX");
        PlayerPrefs.DeleteKey("SpawnY");
        PlayerPrefs.Save();
        SceneManager.LoadScene("Bosque1");
    }


    public void ExitGame()
    {
        
        if (UIManager.Instance != null)
        {
            UIManager.Instance.ExitGame();
        }
        else
        {
            
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


