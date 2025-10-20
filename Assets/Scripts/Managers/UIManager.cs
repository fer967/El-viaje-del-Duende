using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [Header("❤️ Vidas (Hearts)")]
    [SerializeField] private GameObject heartPrefab;      // Prefab del ícono de corazón
    [SerializeField] private Transform heartsParent;      // Contenedor en el Canvas
    [SerializeField] private int maxHearts = 3;
    private List<GameObject> heartImages = new List<GameObject>();

    [Header("🪙 Monedas")]
    [SerializeField] private GameObject coinPrefab;        // Prefab del ícono de moneda
    [SerializeField] private Transform coinsParent;        // Contenedor en el Canvas
    private List<GameObject> coinImages = new List<GameObject>();
    private int currentCoins = 0;


    [Header("💬 Diálogo NPC")]
    public GameObject dialogPanel;
    public TextMeshProUGUI dialogText;

    [Header("☠️ Game Over")]
    public GameObject gameOverPanel;
    public TextMeshProUGUI gameOverText;
    public Button retryButton;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }
    }


    private void Start()
    {
        // Iniciar corazones visibles
        for (int i = 0; i < maxHearts; i++)
        {
            GameObject heart = Instantiate(heartPrefab, heartsParent);
            heartImages.Add(heart);
        }

        // Asegurar paneles ocultos
        if (dialogPanel) dialogPanel.SetActive(false);
        if (gameOverPanel) gameOverPanel.SetActive(false);

        if (retryButton != null)
            retryButton.onClick.AddListener(() =>
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            });
    }

    //private void Start()
    //{
    //    InicializarHearts();
    //}

    // 🔹 Inicializa los corazones en pantalla
    //private void InicializarHearts()
    //{
    //    heartImages.Clear();
    //    for (int i = 0; i < maxHearts; i++)
    //    {
    //        GameObject heart = Instantiate(heartPrefab, heartsParent);
    //        heartImages.Add(heart);
    //    }
    //}

    // 🔹 Actualiza cantidad de corazones visibles
    public void SetLives(int lives)
    {
        for (int i = 0; i < heartImages.Count; i++)
        {
            heartImages[i].SetActive(i < lives);
        }
    }

    // 🔹 Agrega monedas (visualmente con íconos)
    public void AddCoins(int amount)
    {
        currentCoins += amount;

        for (int i = 0; i < amount; i++)
        {
            GameObject coin = Instantiate(coinPrefab, coinsParent);
            coinImages.Add(coin);
        }
    }

    // 🔹 Reinicia monedas (por ejemplo al cambiar de escena)
    public void ResetCoins()
    {
        foreach (GameObject coin in coinImages)
        {
            Destroy(coin);
        }
        coinImages.Clear();
        currentCoins = 0;
    }

    // 🔹 Devuelve cantidad actual de monedas
    public int GetCoins() => currentCoins;


    // === DIÁLOGOS ===
    public void ShowDialog(string text)
    {
        if (dialogPanel == null || dialogText == null) return;

        dialogPanel.SetActive(true);
        dialogText.text = text;
    }

    public void HideDialog()
    {
        if (dialogPanel == null) return;
        dialogPanel.SetActive(false);
    }

    // === GAME OVER ===
    public void ShowGameOver(string message = "Game Over")
    {
        if (gameOverPanel == null) return;

        gameOverPanel.SetActive(true);
        if (gameOverText != null) gameOverText.text = message;
    }

}






//using UnityEngine;
//using UnityEngine.UI;
//using TMPro;
//using System.Collections.Generic;

//public class UIManager : MonoBehaviour
//{
//    public static UIManager Instance;

//    [Header("Lives (hearts)")]
//    public GameObject heartPrefab;          // prefab Image (heart full)
//    public Transform heartsParent;          // contenedor en Canvas
//    public int maxHearts = 3;

//    [Header("Coins")]
//    public Image coinImagePrefab;
//    public Transform coinsParent;
//    public TextMeshProUGUI coinText;        // si querés también un número

//    private int currentCoins = 0;
//    private List<GameObject> heartImages = new List<GameObject>();

//    private void Awake()
//    {
//        if (Instance == null) Instance = this;
//        else Destroy(gameObject);
//    }

//    private void Start()
//    {
//        // inicializar corazones
//        for (int i = 0; i < maxHearts; i++)
//        {
//            var go = Instantiate(heartPrefab, heartsParent);
//            heartImages.Add(go);
//        }
//    }

//    public void SetLives(int lives)
//    {
//        for (int i = 0; i < heartImages.Count; i++)
//        {
//            heartImages[i].SetActive(i < lives);
//        }
//    }

//    public void AddCoins(int amount)
//    {
//        currentCoins += amount;
//        if (coinText != null) coinText.text = currentCoins.ToString();
//        // si querés mostrar sprite por cada coin, podés instanciar coinImagePrefab
//    }

//    public int GetCoins() => currentCoins;
//}
