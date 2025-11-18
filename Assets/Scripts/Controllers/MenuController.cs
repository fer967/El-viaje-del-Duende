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

    [Header("Referencias")]
    public ConfigMenu configMenu;


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
        if (configMenu != null)
        {
            configMenu.OpenConfig();
        }
        else
        {
            Debug.LogWarning("ConfigMenu no asignado en el inspector");
        }
    }

    //public void OpenConfig()
    //{
    //    // ver agregar volumen
    //    Debug.Log("Abrir configuración");
    //}
}


