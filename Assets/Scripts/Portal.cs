using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal : MonoBehaviour
{
    [Header("Configuración del portal")]
    public string sceneToLoad;    
    public Vector2 spawnPosition; 

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // 🔹 Guardar salud antes de cambiar de escena
            PlayerHealthController health = other.GetComponent<PlayerHealthController>();
            if (health != null && GameManager.Instance != null)
            {
                GameManager.Instance.playerMaxHealth = health.maxHealth;
                GameManager.Instance.playerCurrentHealth = health.currentHealth;
            }

            // 🔹 Guardar coordenadas del próximo punto de aparición
            PlayerPrefs.SetFloat("SpawnX", spawnPosition.x);
            PlayerPrefs.SetFloat("SpawnY", spawnPosition.y);
            PlayerPrefs.Save();

            
            SceneManager.LoadScene(sceneToLoad);
        }
    }
}












