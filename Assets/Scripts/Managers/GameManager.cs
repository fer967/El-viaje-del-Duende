// corregido
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.SceneManagement;

//public class GameManager : MonoBehaviour
//{
//    public static GameManager Instance;

//    [Header("Monedas")]
//    public int coins = 3;
//    public int maxCoins = 10;
//    public GameObject coinPrefab;        // asignar en el prefab de GameManager (o en la escena inicial)
//    public Transform coinsContainer;     // asignar en el prefab (Canvas.coinsParent)
//    private List<GameObject> coinIcons = new List<GameObject>();

//    [Header("Salud del jugador (persistente)")]
//    public int playerMaxHealth = 3;
//    public int playerCurrentHealth = 3;

//    [Header("Habilidad")]
//    public bool hasTitanPunch = false;

//    //public bool punchMessageShown = false;

//    // PlayerPrefs keys
//    const string KEY_COINS = "GM_Coins";
//    const string KEY_MAX_HEALTH = "GM_MaxHealth";
//    const string KEY_CUR_HEALTH = "GM_CurHealth";
//    const string KEY_HAS_PUNCH = "GM_HasPunch";

//    private void Awake()
//    {
//        if (Instance == null)
//        {
//            Instance = this;
//            DontDestroyOnLoad(gameObject);

//            // Load saved state (si existe)
//            LoadState();

//            // Nos suscribimos para re-inicializar UI cuando se cargue una nueva escena,
//            // así coinsContainer asignado en el prefab puede apuntar a un Canvas de la escena
//            SceneManager.sceneLoaded += OnSceneLoaded;
//        }
//        else
//        {
//            Destroy(gameObject);
//        }
//    }

//    private void Start()
//    {
//        // Init UI si ya tenemos referencia al container
//        InitCoinsUI();
//        // Sincronizar UI de hearts si existe UIManager
//        if (UIManager.Instance != null)
//            UIManager.Instance.UpdateHearts(playerCurrentHealth, playerMaxHealth);
//    }

//    private void OnDestroy()
//    {
//        SceneManager.sceneLoaded -= OnSceneLoaded;
//    }

//    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
//    {
//        // Si el coinsContainer no está asignado (o apunta a un objeto de escena anterior),
//        // intenta localizar el coinsParent en la escena (si tu Canvas prefab usa siempre el mismo nombre).
//        if (coinsContainer == null)
//        {
//            // ejemplo: busca un objeto llamado "CoinsPanel" o "CoinsParent" en la escena
//            var found = GameObject.Find("CoinsParent");
//            if (found != null) coinsContainer = found.transform;
//        }

//        // Re-inicializar la UI de monedas (siempre que coinsContainer exista)
//        InitCoinsUI();

//        // Actualizar UI de salud si existe UIManager
//        if (UIManager.Instance != null)
//            UIManager.Instance.UpdateHearts(playerCurrentHealth, playerMaxHealth);
//    }

//    // Inicializa los iconos de UI para monedas (máximo)
//    void InitCoinsUI()
//    {
//        if (coinsContainer == null || coinPrefab == null) return;

//        // destruir hijos existentes (por si quedaron de escenas anteriores)
//        foreach (Transform child in coinsContainer)
//            Destroy(child.gameObject);

//        coinIcons.Clear();

//        for (int i = 0; i < maxCoins; i++)
//        {
//            GameObject icon = Instantiate(coinPrefab, coinsContainer);
//            icon.SetActive(i < coins); // mostrar activos según la cantidad actual
//            coinIcons.Add(icon);
//        }
//    }

//    void UpdateCoinsUI()
//    {
//        if (coinIcons == null || coinIcons.Count == 0) return;

//        for (int i = 0; i < coinIcons.Count; i++)
//            coinIcons[i].SetActive(i < coins);
//    }

//    // ---------------- Monedas ----------------
//    public void AddCoins(int amount)
//    {
//        coins += amount;
//        if (coins > maxCoins) coins = maxCoins;
//        UpdateCoinsUI();
//        SaveState();
//        // opcional: notificar a UIManager para efectos extra
//        if (UIManager.Instance != null) UIManager.Instance.AddCoins(amount);
//    }

//    public void SpendCoins(int amount)
//    {
//        coins -= amount;
//        if (coins < 0) coins = 0;
//        UpdateCoinsUI();
//        SaveState();
//    }

//    // ---------------- Salud ----------------
//    // Setea ambos valores (llamado por Player al iniciar o al cambiar vida)
//    public void SetPlayerHealth(int current, int max)
//    {
//        playerCurrentHealth = Mathf.Clamp(current, 0, max);
//        playerMaxHealth = Mathf.Max(1, max);
//        SaveState();

//        if (UIManager.Instance != null)
//            UIManager.Instance.UpdateHearts(playerCurrentHealth, playerMaxHealth);
//    }

//    // Actualiza solo la salud actual
//    public void UpdatePlayerHealth(int newHealth)
//    {
//        playerCurrentHealth = Mathf.Clamp(newHealth, 0, playerMaxHealth);
//        SaveState();

//        if (UIManager.Instance != null)
//            UIManager.Instance.UpdateHearts(playerCurrentHealth, playerMaxHealth);
//    }

//    // ---------------- Habilidad Puño Titánico ----------------
//    public void UnlockTitanPunch()
//    {
//        if (hasTitanPunch) return;
//        hasTitanPunch = true;
//        PlayerPrefs.SetInt(KEY_HAS_PUNCH, 1);
//        PlayerPrefs.Save();

//        // Mostrar mensaje por UIManager
//        if (UIManager.Instance != null)
//            UIManager.Instance.ShowMessage("¡Has obtenido la habilidad Puño Titánico!");

//        Debug.Log("GameManager: Titan Punch desbloqueado y guardado.");
//        SaveState();
//    }

//    public bool HasTitanPunch() => hasTitanPunch;

//    // ---------------- Game Over / Reinicio ----------------
//    public void TriggerGameOver()
//    {
//        if (UIManager.Instance != null)
//            UIManager.Instance.ShowGameOver("GAME OVER");

//        Time.timeScale = 0f;
//    }

//    public void RestartGame()
//    {
//        if (UIManager.Instance != null)
//            UIManager.Instance.HideGameOver();

//        // Reset básico (podés ajustarlo)
//        PlayerPrefs.DeleteKey("SpawnX");
//        PlayerPrefs.DeleteKey("SpawnY");

//        coins = 3;
//        playerCurrentHealth = playerMaxHealth; // restaurar salud
//        UpdateCoinsUI();
//        SaveState();

//        Time.timeScale = 1f;
//        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
//    }

//    // ---------------- Guardado / Carga ----------------
//    public void SaveState()
//    {
//        PlayerPrefs.SetInt(KEY_COINS, coins);
//        PlayerPrefs.SetInt(KEY_MAX_HEALTH, playerMaxHealth);
//        PlayerPrefs.SetInt(KEY_CUR_HEALTH, playerCurrentHealth);
//        PlayerPrefs.SetInt(KEY_HAS_PUNCH, hasTitanPunch ? 1 : 0);
//        PlayerPrefs.Save();
//    }

//    public void LoadState()
//    {
//        if (PlayerPrefs.HasKey(KEY_COINS)) coins = PlayerPrefs.GetInt(KEY_COINS);
//        if (PlayerPrefs.HasKey(KEY_MAX_HEALTH)) playerMaxHealth = PlayerPrefs.GetInt(KEY_MAX_HEALTH);
//        if (PlayerPrefs.HasKey(KEY_CUR_HEALTH)) playerCurrentHealth = PlayerPrefs.GetInt(KEY_CUR_HEALTH);
//        if (PlayerPrefs.HasKey(KEY_HAS_PUNCH)) hasTitanPunch = PlayerPrefs.GetInt(KEY_HAS_PUNCH) == 1;
//    }

//    // Llamado por enemigos o sistemas cuando un enemigo muere
//    public void EnemyKilled()
//    {
//        // placeholder — podés contar kills, spawnear cofres, etc.
//    }

//}




// ultimo sin corregir (no aparece mensaje de habilidad en B5 si vengo de B4)

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

        playerCurrentHealth = playerMaxHealth;

        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }


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



















