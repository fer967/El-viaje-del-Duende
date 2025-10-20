using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Monedas")]
    public int coins = 0;
    public Text coinsText;  // O Image con sprite si querés mostrar íconos

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

    public void AddCoins(int amount)
    {
        coins += amount;
        UpdateCoinsUI();
    }

    public void SpendCoins(int amount)
    {
        coins -= amount;
        if (coins < 0) coins = 0;
        UpdateCoinsUI();
    }

    public void UpdateCoinsUI()
    {
        if (coinsText != null)
            coinsText.text = coins.ToString();
    }

    public void EnemyKilled()
    {
        //UnregisterEnemy();
        // si querés hacer spawn de cofre al matar X enemigos, lo manejás aquí
    }

    public void TriggerGameOver()
    {
        // Podés cargar una escena de GameOver o activar panel
        //SceneManager.LoadScene("SampleScene"); // vuelve al menu
    }


    public void ShowGameOver()
    {
        UIManager.Instance.ShowGameOver("Has sido derrotado...");
    }

    //public void ShowGameOver()
    //{
    //    // Ejemplo: mostrar panel de Game Over o cargar escena
    //    Debug.Log("🎮 GAME OVER");
    //}
}





//using UnityEngine;
//using UnityEngine.SceneManagement;

//public class GameManager : MonoBehaviour
//{
//    public static GameManager Instance;

//    private int enemiesAlive = 0;

//    private void Awake()
//    {
//        if (Instance == null) Instance = this;
//        else Destroy(gameObject);
//    }

//    public void RegisterEnemy()
//    {
//        enemiesAlive++;
//    }

//    public void UnregisterEnemy()
//    {
//        enemiesAlive = Mathf.Max(0, enemiesAlive - 1);
//    }

//    public void EnemyKilled()
//    {
//        UnregisterEnemy();
//        // si querés hacer spawn de cofre al matar X enemigos, lo manejás aquí
//    }

//    public void TriggerGameOver()
//    {
//        // Podés cargar una escena de GameOver o activar panel
//        SceneManager.LoadScene("SampleScene"); // vuelve al menu
//    }

//    public void ReloadCurrent()
//    {
//        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
//    }
//}

