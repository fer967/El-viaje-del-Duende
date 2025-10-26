using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Monedas")]
    public int coins = 3;
    public int maxCoins = 10;
    public GameObject coinPrefab;
    public Transform coinsContainer;
    private List<GameObject> coinIcons = new List<GameObject>();

    [Header("Salud del jugador")]
    public int playerMaxHealth = 0;
    public int playerCurrentHealth = 0;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        InitCoinsUI();
    }

    void InitCoinsUI()
    {
        if (coinsContainer == null || coinPrefab == null) return;

        foreach (Transform child in coinsContainer)
            Destroy(child.gameObject);

        coinIcons.Clear();

        for (int i = 0; i < maxCoins; i++)
        {
            GameObject icon = Instantiate(coinPrefab, coinsContainer);
            icon.SetActive(true);
            coinIcons.Add(icon);
        }
    }

    public void SpendCoins(int amount)
    {
        coins -= amount;
        if (coins < 0) coins = 0;
        UpdateCoinsUI();
    }

    void UpdateCoinsUI()
    {
        if (coinIcons.Count == 0) return;

        for (int i = 0; i < coinIcons.Count; i++)
        {
            coinIcons[i].SetActive(i < coins);
        }
    }

    public void EnemyKilled()
    {
        // lógica de enemigos si hace falta
    }

    public void TriggerGameOver()
    {
        if (UIManager.Instance != null)
            UIManager.Instance.ShowGameOver("GAME OVER");

        Time.timeScale = 0f;
    }

    public void RestartGame()
    {
        if (UIManager.Instance != null)
            UIManager.Instance.HideGameOver();

        PlayerPrefs.DeleteKey("SpawnX");
        PlayerPrefs.DeleteKey("SpawnY");

        coins = 3;
        UpdateCoinsUI();

        // 🔹 Restaurar vidas
        playerCurrentHealth = playerMaxHealth;

        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    // 🔹 Métodos para sincronizar salud del jugador
    public void SetPlayerHealth(int current, int max)
    {
        playerCurrentHealth = current;
        playerMaxHealth = max;
    }

    public void UpdatePlayerHealth(int newHealth)
    {
        playerCurrentHealth = Mathf.Clamp(newHealth, 0, playerMaxHealth);
    }
}







//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.SceneManagement;

//public class GameManager : MonoBehaviour
//{
//    public static GameManager Instance;

//    [Header("Monedas")]
//    public int coins = 3;
//    public int maxCoins = 10;
//    public GameObject coinPrefab;  
//    public Transform coinsContainer; 
//    private List<GameObject> coinIcons = new List<GameObject>();

//    [Header("Salud del jugador")]
//    public int playerMaxHealth = 0;
//    public int playerCurrentHealth = 0;


//    private void Awake()
//    {
//        if (Instance == null)
//        {
//            Instance = this;
//            DontDestroyOnLoad(gameObject);
//        }
//        else
//        {
//            Destroy(gameObject);
//        }
//    }



//    // modificacion para actualizar vidas en cambio de escena
//    private void Start()
//    {
//        if (playerMaxHealth == 0)
//        {
//            playerMaxHealth = 3;
//            playerCurrentHealth = playerMaxHealth;
//        }

//        InitCoinsUI();
//    }


//    //private void Start()
//    //{
//    //    InitCoinsUI();
//    //}

//    void InitCoinsUI()
//    {
//        if (coinsContainer == null || coinPrefab == null) return;

//        // Limpia por si ya existían
//        foreach (Transform child in coinsContainer)
//            Destroy(child.gameObject);

//        coinIcons.Clear();

//        // Crea los íconos iniciales (máx de monedas posibles)
//        for (int i = 0; i < maxCoins; i++)
//        {
//            GameObject icon = Instantiate(coinPrefab, coinsContainer);
//            icon.SetActive(true); 
//            coinIcons.Add(icon);
//        }
//    }



//    public void SpendCoins(int amount)
//    {
//        coins -= amount;
//        if (coins < 0) coins = 0;
//        UpdateCoinsUI();
//    }

//    void UpdateCoinsUI()
//    {
//        if (coinIcons.Count == 0) return;

//        for (int i = 0; i < coinIcons.Count; i++)
//        {
//            coinIcons[i].SetActive(i < coins);
//        }
//    }

//    public void EnemyKilled()
//    {
//        // lógica de enemigos si hace falta
//    }


//    public void TriggerGameOver()
//    {
//         if (UIManager.Instance != null)
//            UIManager.Instance.ShowGameOver("GAME OVER");

//            Time.timeScale = 0f;
//    }


//    public void RestartGame()
//    {
//        if (UIManager.Instance != null)
//           UIManager.Instance.HideGameOver();

//        PlayerPrefs.DeleteKey("SpawnX");
//        PlayerPrefs.DeleteKey("SpawnY");

//        coins = 3;
//        UpdateCoinsUI();

//          Time.timeScale = 1f;

//        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
//    }

//    // modificacion para actualizar vidas en cambio de escena
//    private void OnEnable()
//    {
//        SceneManager.sceneLoaded += OnSceneLoaded;
//    }

//    private void OnDisable()
//    {
//        SceneManager.sceneLoaded -= OnSceneLoaded;
//    }

//    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
//    {
//        // Cuando carga una nueva escena, actualizamos la UI de corazones
//        if (UIManager.Instance != null)
//        {
//            UIManager.Instance.UpdateHearts(playerCurrentHealth, playerMaxHealth);
//        }
//    }


//}














