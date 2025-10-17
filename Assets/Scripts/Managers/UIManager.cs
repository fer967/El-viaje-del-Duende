using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [Header("Lives (hearts)")]
    public GameObject heartPrefab;          // prefab Image (heart full)
    public Transform heartsParent;          // contenedor en Canvas
    public int maxHearts = 3;

    [Header("Coins")]
    public Image coinImagePrefab;
    public Transform coinsParent;
    public TextMeshProUGUI coinText;        // si querés también un número

    private int currentCoins = 0;
    private List<GameObject> heartImages = new List<GameObject>();

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        // inicializar corazones
        for (int i = 0; i < maxHearts; i++)
        {
            var go = Instantiate(heartPrefab, heartsParent);
            heartImages.Add(go);
        }
    }

    public void SetLives(int lives)
    {
        for (int i = 0; i < heartImages.Count; i++)
        {
            heartImages[i].SetActive(i < lives);
        }
    }

    public void AddCoins(int amount)
    {
        currentCoins += amount;
        if (coinText != null) coinText.text = currentCoins.ToString();
        // si querés mostrar sprite por cada coin, podés instanciar coinImagePrefab
    }

    public int GetCoins() => currentCoins;
}
