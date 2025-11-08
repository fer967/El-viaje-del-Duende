// corregido
//using UnityEngine;
//using UnityEngine.SceneManagement;

//public class GameManager : MonoBehaviour
//{
//    // ==============================
//    // 🔹 SINGLETON
//    // ==============================
//    private static GameManager instance;
//    public static GameManager Instance => instance;

//    private void Awake()
//    {
//        // Asegura que solo exista una instancia de GameManager
//        if (instance != null && instance != this)
//        {
//            Destroy(gameObject);
//            return;
//        }

//        instance = this;
//        DontDestroyOnLoad(gameObject);
//    }

//    // ==============================
//    // 🔹 DATOS DEL JUGADOR
//    // ==============================
//    [Header("Datos del jugador")]
//    public int maxHealth = 4;
//    public int currentHealth = 4;
//    public int coins = 0;

//    [Header("Habilidades")]
//    public bool hasTitanPunch = false;
//    public bool punchMessageShown = false;

//    // Referencia al jugador actual
//    private Player player;

//    // ==============================
//    // 🔹 REFERENCIAS
//    // ==============================
//    private void Start()
//    {
//        SceneManager.sceneLoaded += OnSceneLoaded;
//    }

//    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
//    {
//        // Busca un nuevo Player cuando cambia la escena
//        player = FindObjectOfType<Player>();

//        // Actualiza los corazones al cargar la nueva escena
//        if (UIManager.Instance != null)
//        {
//            UIManager.Instance.UpdateHearts(currentHealth, maxHealth);
//            UpdateCoinUI();
//        }

//        // Si el jugador ya tiene Puño Titánico y todavía no se mostró mensaje, lo mostramos una sola vez
//        if (hasTitanPunch && !punchMessageShown)
//        {
//            UIManager.Instance?.ShowMessage("¡Has obtenido la habilidad Puño Titánico!");
//            punchMessageShown = true;
//        }
//    }

//    // ==============================
//    // 🔹 SISTEMA DE VIDA
//    // ==============================
//    public void TakeDamage(int amount)
//    {
//        currentHealth -= amount;
//        if (currentHealth < 0) currentHealth = 0;

//        if (UIManager.Instance != null)
//            UIManager.Instance.UpdateHearts(currentHealth, maxHealth);

//        if (currentHealth <= 0)
//            PlayerDied();
//    }

//    public void Heal(int amount)
//    {
//        currentHealth += amount;
//        if (currentHealth > maxHealth) currentHealth = maxHealth;

//        if (UIManager.Instance != null)
//            UIManager.Instance.UpdateHearts(currentHealth, maxHealth);
//    }

//    private void PlayerDied()
//    {
//        Debug.Log("💀 Jugador derrotado");
//        UIManager.Instance?.ShowGameOver("Has sido derrotado...");
//        // Podés agregar respawn o menú aquí
//    }

//    // ==============================
//    // 🔹 MONEDAS
//    // ==============================
//    public void AddCoins(int amount)
//    {
//        coins += amount;
//        UIManager.Instance?.AddCoins(amount);
//    }

//    public void UpdateCoinUI()
//    {
//        // Actualiza el UI con la cantidad actual (si querés sincronizarlo)
//        UIManager.Instance?.ResetCoins();
//        UIManager.Instance?.AddCoins(coins);
//    }

//    // ==============================
//    // 🔹 HABILIDADES
//    // ==============================
//    public void UnlockTitanPunch()
//    {
//        hasTitanPunch = true;

//        // Mostramos el mensaje solo una vez por partida
//        if (!punchMessageShown)
//        {
//            UIManager.Instance?.ShowMessage("¡Has obtenido la habilidad Puño Titánico!");
//            punchMessageShown = true;
//        }

//        Debug.Log("🟢 Puño Titánico desbloqueado");
//    }

//    // ==============================
//    // 🔹 UTILIDAD
//    // ==============================
//    public void UpdatePlayerReference(Player p)
//    {
//        player = p;
//    }

//    public void ResetAll()
//    {
//        // Resetea todos los valores (por ejemplo, al volver al menú)
//        currentHealth = maxHealth;
//        coins = 0;
//        hasTitanPunch = false;
//        punchMessageShown = false;

//        UIManager.Instance?.ResetCoins();
//        UIManager.Instance?.UpdateHearts(currentHealth, maxHealth);
//    }
//}





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

    public bool hasTitanPunch = false;
    public bool punchMessageShown = false;


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
        if (playerMaxHealth == 0)
        {
            playerMaxHealth = 3;
            playerCurrentHealth = playerMaxHealth;
        }

        InitCoinsUI();
    }


    //private void Start()
    //{
    //    InitCoinsUI();
    //}

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

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Cuando carga una nueva escena, actualizamos la UI de corazones
        if (UIManager.Instance != null)
        {
            UIManager.Instance.UpdateHearts(playerCurrentHealth, playerMaxHealth);
        }
    }

    public void UnlockTitanPunch()
    {
        hasTitanPunch = true;
        punchMessageShown = true;

        if (UIManager.Instance != null)
            UIManager.Instance.ShowMessage("¡Has obtenido la habilidad Puño Titánico!");
    }

}



















