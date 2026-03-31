using UnityEngine;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour
{
    [Header("Referencias")]
    public Slider healthSlider;      
    public PlayerActions player;     

    void Update()
    {
        // Make sure we have a reference to the player and the health slider before trying to update the UI.
        if (player != null && healthSlider != null)
        {
            // Update the health slider value based on the player's current health.
            healthSlider.value = player.GetCurrentHealth() / player.playerStats.maxHealth;
        }
    }
}
