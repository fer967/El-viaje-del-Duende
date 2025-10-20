using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal : MonoBehaviour
{
    [Header("Configuraci�n del portal")]
    public string sceneToLoad;    // Nombre exacto de la escena destino
    public Vector2 spawnPosition; // Coordenadas donde aparecer� el player en la nueva escena

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Guardar coordenadas del pr�ximo punto de aparici�n
            PlayerPrefs.SetFloat("SpawnX", spawnPosition.x);
            PlayerPrefs.SetFloat("SpawnY", spawnPosition.y);
            PlayerPrefs.Save();

            // Cargar escena destino
            SceneManager.LoadScene(sceneToLoad);
        }
    }
}






//using UnityEngine;
//using UnityEngine.SceneManagement;

//public class Portal : MonoBehaviour
//{
//    public string sceneToLoad;
//    public Vector2 spawnPosition;

//    private void OnTriggerEnter2D(Collider2D other)
//    {
//        if (other.CompareTag("Player"))
//        {
//            PlayerPrefs.SetFloat("SpawnX", spawnPosition.x);
//            PlayerPrefs.SetFloat("SpawnY", spawnPosition.y);
//            SceneManager.LoadScene(sceneToLoad);
//        }
//    }
//}

