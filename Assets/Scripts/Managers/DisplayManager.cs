using UnityEngine;

public class DisplayManager : MonoBehaviour
{
    void Start()
    {
        // Pequeño delay para asegurar que Unity inicializó todo
        Invoke(nameof(SetFullscreen), 0.1f);
    }

    void SetFullscreen()
    {
        try
        {
            Resolution nativeResolution = Screen.currentResolution;
            Screen.SetResolution(nativeResolution.width, nativeResolution.height, FullScreenMode.FullScreenWindow);
            Debug.Log($"🖥️ Pantalla completa: {nativeResolution.width}x{nativeResolution.height}");
        }
        catch (System.Exception e)
        {
            Debug.LogError($"❌ Error configurando pantalla: {e.Message}");
        }
    }
}