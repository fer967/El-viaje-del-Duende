using System.Collections.Generic;
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

    [Header("Mensajes temporales")]
    public GameObject messagePanel;
    public TMPro.TextMeshProUGUI messageText;
    public float messageDuration = 3f;
    private Coroutine messageCoroutine;

    [Header("Panel de Victoria")]
    public GameObject victoryPanel;
    public TMPro.TextMeshProUGUI victoryText;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            if (victoryPanel != null)
                victoryPanel.SetActive(true);

            if (exitButton != null)
                exitButton.onClick.AddListener(ReturnToMenu);

            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    
    private void Start()
    {
        if (victoryPanel != null)
            victoryPanel.SetActive(false);

        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);

        if (messagePanel != null)
            messagePanel.SetActive(false);

        Debug.Log("✅ Paneles desactivados correctamente en Start");
    }

          
  
    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
                
        switch (scene.name)
        {
            case "Bosque1":
                ShowMessage("Movimiento -> flechas  Dialoga con Artus");
                break;

            case "Bosque2":
                ShowMessage("Cuidado con los lobos");
                break;

            case "Bosque3":
                ShowMessage("Ayuda al Mercader");
                break;

            case "Bosque4":
                ShowMessage("Encuentra la salida, evita los sapos");
                break;

            case "Bosque5":
                ShowMessage("Busca un Cofre");
                break;

            case "Bosque6":
                ShowMessage("Tienes Puño Titanico");
                break;

            case "Cueva1":
                ShowMessage("Cuidado con los fuegos fatuos.");
                break;

            case "Cueva2":
                ShowMessage("Hay Arqueros acechando");
                break;

            case "Cueva3":
                ShowMessage("Derrota a la malvada Bruja");
                break;

        }
    }

    
    public void ExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }


    public void ReturnToMenu()
    {
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
            Destroy(coin);

        coinImages.Clear();
        currentCoins = 0;
    }

    public int GetCoins() => currentCoins;

    
    public void ShowMessage(string message)
    {
        if (messageText == null)
        {
            Debug.LogWarning("⚠️ UIManager: messageText no está asignado.");
            return;
        }

        if (messageCoroutine != null)
            StopCoroutine(messageCoroutine);
        messageCoroutine = StartCoroutine(ShowMessageCoroutine(message));
    }

    private System.Collections.IEnumerator ShowMessageCoroutine(string message)
    {
        messageText.text = message;

        if (messagePanel != null) messagePanel.SetActive(true);
        else messageText.gameObject.SetActive(true);

        yield return new WaitForSeconds(messageDuration);

        if (messagePanel != null) messagePanel.SetActive(false);
        else messageText.gameObject.SetActive(false);

        messageCoroutine = null;
    }

    public void ShowVictory(string message = "¡VICTORIA! Has derrotado a la Bruja")
    {
        if (victoryPanel != null)
        {
            victoryPanel.SetActive(true);
            if (victoryText != null)
                victoryText.text = message;
        }

        Time.timeScale = 0f;

        var player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            var playerController = player.GetComponent<PlayerController>();
            if (playerController != null)
                playerController.enabled = false;
        }
    }

    
    public void ReturnToMenuFromVictory()
    {
        Time.timeScale = 1f;

        if (victoryPanel != null)
            victoryPanel.SetActive(false);

        SceneManager.LoadScene("MenuInicio");
    }
   
}












