using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private int enemiesAlive = 0;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void RegisterEnemy()
    {
        enemiesAlive++;
    }

    public void UnregisterEnemy()
    {
        enemiesAlive = Mathf.Max(0, enemiesAlive - 1);
    }

    public void EnemyKilled()
    {
        UnregisterEnemy();
        // si quer�s hacer spawn de cofre al matar X enemigos, lo manej�s aqu�
    }

    public void TriggerGameOver()
    {
        // Pod�s cargar una escena de GameOver o activar panel
        SceneManager.LoadScene("SampleScene"); // vuelve al menu
    }

    public void ReloadCurrent()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}

