using UnityEngine;

public class LifePickup : MonoBehaviour
{
    public int lifeAmount = 1;
    public AudioClip pickupSound;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerHealthController health = collision.GetComponent<PlayerHealthController>();
            if (health != null)
            {
                // 🔹 Aumenta los corazones máximos
                health.AddMaxHeart(lifeAmount);

                
                if (pickupSound)
                    AudioSource.PlayClipAtPoint(pickupSound, transform.position);

                Destroy(gameObject);
            }
        }
    }
}







