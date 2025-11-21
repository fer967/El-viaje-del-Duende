using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class PantallaFinalController : MonoBehaviour
{
    [Header("UI Elements")]
    public TMP_Text victoryText;
    public GameObject menuButton;
    
    [Header("Configuración")]
    public string menuScene = "MenuInicio";
    
    void Start()
    {
               
        if (menuButton != null)
        {
            var button = menuButton.GetComponent<UnityEngine.UI.Button>();
            if (button != null)
                button.onClick.AddListener(GoToMenu);
        }

        if (AudioManager.instance != null)
        {
            AudioManager.instance.PlayVictoryMusic();
        }
    }

    public void GoToMenu()
    {
        Debug.Log("📋 Volviendo al menú");
        SceneManager.LoadScene(menuScene);
            
    }
}