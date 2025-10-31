using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;  

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [Header("Panel de corazones")]
    public Image[] heartImages;   
    public Sprite fullHeart;      
    public Sprite emptyHeart;     

    [Header("Panel Game Over")]
    public GameObject gameOverPanel;
    public Text gameOverText;

    [Header("🪙 Monedas (iconos)")]
    public GameObject coinPrefab;
    public Transform coinsParent;
    private List<GameObject> coinImages = new List<GameObject>();
    private int currentCoins = 0;

    [Header("Botón Salir Global")]
    public Button exitButton; 

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            if (exitButton != null)
            {
                exitButton.onClick.AddListener(ReturnToMenu);
                Debug.Log("✅ Botón Salir Global configurado en UIManager");
            }
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }


    
    public void ExitGame()
    {
        Debug.Log("🚪 Cerrando juego desde UIManager...");

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
    }

    
    public void ReturnToMenu()
    {
        Debug.Log("🏠 Volviendo al menú principal...");
        Time.timeScale = 1f; 
        SceneManager.LoadScene("MenuInicio");
    }



    public void UpdateHearts(int current, int max)
    {
        if (heartImages == null || heartImages.Length == 0)
        {
            Debug.LogWarning("⚠️ UIManager: no hay corazones asignados en el array heartImages.");
            return;
        }

        for (int i = 0; i < heartImages.Length; i++)
        {
            if (heartImages[i] == null)
            {
                Debug.LogWarning($"⚠️ Corazón {i} no está asignado en el UIManager.");
                continue;
            }
            heartImages[i].sprite = (i < current) ? fullHeart : emptyHeart;
            heartImages[i].enabled = (i < max);
        }
    }


    public void ShowGameOver(string message)
    {
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
            if (gameOverText != null)
                gameOverText.text = message;
        }
    }

 
    public void HideGameOver()
    {
        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);
    }
   
   
    public void AddCoins(int amount)
    {
        currentCoins += amount;

        for (int i = 0; i < amount; i++)
        {
            GameObject coin = Instantiate(coinPrefab, coinsParent);
            coinImages.Add(coin);
        }
    }

   
    public void ResetCoins()
    {
        foreach (GameObject coin in coinImages)
        {
            Destroy(coin);
        }
        coinImages.Clear();
        currentCoins = 0;
    }

   
    public int GetCoins() => currentCoins;
    
}





