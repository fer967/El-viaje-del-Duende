using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Monedas")]
    public int coins = 3;
    public int maxCoins = 10;
    public GameObject coinPrefab;  // Prefab del ícono de moneda
    public Transform coinsContainer; // El contenedor (MonedasUI)
    private List<GameObject> coinIcons = new List<GameObject>();

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

        // Limpia por si ya existían
        foreach (Transform child in coinsContainer)
            Destroy(child.gameObject);

        coinIcons.Clear();

        // Crea los íconos iniciales (máx de monedas posibles)
        for (int i = 0; i < maxCoins; i++)
        {
            GameObject icon = Instantiate(coinPrefab, coinsContainer);
            icon.SetActive(true); // Ocultas hasta que se consiga                ver  --> estaba false
            coinIcons.Add(icon);
        }
    }


    //public void AddCoins(int amount)
    //{
    //    UIManager.Instance.AddCoins(amount);
    //}


    public void AddCoins(int amount)
    {
        coins += amount;
        if (coins > maxCoins) coins = maxCoins;
        UpdateCoinsUI();

        UIManager.Instance.AddCoins(amount);
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
        // lógica de enemigos si la necesitás
    }


    public void TriggerGameOver()
    {
        // 🔹 Llama al UIManager para mostrar el panel
        if (UIManager.Instance != null)
            UIManager.Instance.ShowGameOver("Has sido derrotado...");
        else
            Debug.LogWarning("⚠️ No se encontró el UIManager para mostrar el Game Over.");

        Debug.Log("🎮 GAME OVER (GameManager delegó a UIManager)");
    }


    //public void TriggerGameOver()
    //{
    //    UIManager.Instance.ShowGameOver("Has sido derrotado ... ");
    //    Debug.Log("🎮 GAME OVER");
    //}



}




//using UnityEngine;
//using UnityEngine.UI;

//public class GameManager : MonoBehaviour
//{
//    public static GameManager Instance;

//    //[Header("Monedas")]
//    //public int coins = 0;
//    //public Text coinsText;  

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

//    //public void AddCoins(int amount)
//    //{
//    //    coins += amount;
//    //    UpdateCoinsUI();
//    //}

//    //public void SpendCoins(int amount)
//    //{
//    //    coins -= amount;
//    //    if (coins < 0) coins = 0;
//    //    UpdateCoinsUI();
//    //}

//    //public void UpdateCoinsUI()
//    //{
//    //    if (coinsText != null)
//    //        coinsText.text = coins.ToString();
//    //}

//    public void EnemyKilled()
//    {
//        //UnregisterEnemy();
//        // si querés hacer spawn de cofre al matar X enemigos, lo manejás aquí
//    }

//    public void TriggerGameOver()
//    {
//        // Podés cargar una escena de GameOver o activar panel
//        //SceneManager.LoadScene("SampleScene"); // vuelve al menu
//    }


//    public void ShowGameOver()
//    {
//        UIManager.Instance.ShowGameOver("Has sido derrotado...");
//        Debug.Log("🎮 GAME OVER");
//    }


//}










