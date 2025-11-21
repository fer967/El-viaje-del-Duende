using UnityEngine;
using UnityEngine.SceneManagement;

public class VictoryPortal : MonoBehaviour
{
    [Header("Configuración")]
    public string victorySceneName = "PantallaFinal";
    public float delayBeforeLoad = 1.5f;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Verificar si la bruja fue derrotada
            GameObject bruja = GameObject.FindGameObjectWithTag("Enemy");
            if (bruja == null) // Bruja no existe = fue derrotada
            {
                Debug.Log("🎯 Victoria! Jugador entró al portal de victoria");

                // Reproducir música de victoria
                if (AudioManager.instance != null)
                {
                    AudioManager.instance.PlayVictoryMusic();
                }

                // Deshabilitar player durante transición
                other.GetComponent<PlayerController>().enabled = false;

                // Cargar escena de victoria después de delay
                Invoke(nameof(LoadVictoryScene), delayBeforeLoad);
            }
            else
            {
                Debug.Log("La bruja aún no ha sido derrotada");
            }
        }
    }

    private void LoadVictoryScene()
    {
        SceneManager.LoadScene(victorySceneName);
    }
}




