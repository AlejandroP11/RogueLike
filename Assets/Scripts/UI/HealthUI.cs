using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour
{
    public static HealthUI Instance;
    [Header("Referencias")]
    [SerializeField] private Image fillImage;
    [SerializeField] private TextMeshProUGUI healthText;

    private void Awake()
    {
        // Implementing the Singleton pattern to ensure only one instance of HealthUI exists.
        if (Instance == null)
        {
            Instance = this;
            PlayerActions.Instance.OnHealthChanged += PlayerActions_OnHealthChanged;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlayerActions_OnHealthChanged(object sender, PlayerActions.OnHealthChangedEventArgs e) {
        fillImage.fillAmount = e.currentHealth / e.maxHealth;
        healthText.text = $"{e.currentHealth} / {e.maxHealth}";
    }

}
