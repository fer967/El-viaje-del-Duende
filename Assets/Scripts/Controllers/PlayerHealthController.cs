using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthController : MonoBehaviour
{
    [Header("Salud del Player")]
    public int maxHealth = 0;
    public int currentHealth;
    public Image[] hearts;       
    public Sprite fullHeart;
    public Sprite emptyHeart;

    private void Start()
    {
        if (GameManager.Instance != null)
        {
            maxHealth = GameManager.Instance.playerMaxHealth;
            currentHealth = GameManager.Instance.playerCurrentHealth;
        }

        UpdateHeartsUI();
    }


    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        if (currentHealth < 0) currentHealth = 0;

        UpdateHeartsUI();

        // 🔹 Sincronizar con GameManager
        if (GameManager.Instance != null)
        {
            GameManager.Instance.playerCurrentHealth = currentHealth;
        }

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void Heal(int amount)
    {
        currentHealth += amount;
        if (currentHealth > maxHealth) currentHealth = maxHealth;

        UpdateHeartsUI();

        // 🔹 Sincronizar con GameManager
        if (GameManager.Instance != null)
        {
            GameManager.Instance.playerCurrentHealth = currentHealth;
        }
    }

    public void AddMaxHeart(int amount)
    {
        maxHealth += amount;
        currentHealth = maxHealth;

        UpdateHeartsUI();

        // 🔹 Sincronizar con GameManager
        if (GameManager.Instance != null)
        {
            GameManager.Instance.playerMaxHealth = maxHealth;
            GameManager.Instance.playerCurrentHealth = currentHealth;
        }
    }

    

    void Die()
    {
        Debug.Log("💀 El jugador murió");
        // Aquí se puede mostrar el panel Game Over o reiniciar la escena
        //GameManager.Instance.ShowGameOver();
    }

    void UpdateHeartsUI()
    {
        for (int i = 0; i < hearts.Length; i++)
        {
            if (i < currentHealth)
                hearts[i].sprite = fullHeart;
            else
                hearts[i].sprite = emptyHeart;

            hearts[i].enabled = i < maxHealth;
        }
    }


}
