using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    public float MaxHealth;
    [SerializeField] private float currentHealth;
    private TextMeshProUGUI healthText;
    private bool isMoving = false;

    void Start()
    {
        currentHealth = MaxHealth;
        FindHealthText();
        UpdateHealthText();
    }

    public void TakeDamage(float value)
    {
        currentHealth -= value;

        if (currentHealth <= 0)
        {
            SceneManager.LoadScene("GameOverMenu");
        }

        UpdateHealthText();
    }

    public void Heal(float value)
    {
        currentHealth += value;

        if (currentHealth > MaxHealth)
        {
            currentHealth = MaxHealth;
        }

        UpdateHealthText();
    }

    private void FindHealthText()
    {
        Camera mainCamera = GetComponentInChildren<Camera>();
        healthText = mainCamera.GetComponentInChildren<TextMeshProUGUI>();
    }

    private void UpdateHealthText()
    {
        healthText.text = currentHealth.ToString();
    }

    public void SetIsMoving(bool moving)
    {
        isMoving = moving;
    }

    public bool IsMoving()
    {
        return isMoving;
    }
    
    public float GetCurrentHealth()
    {
        return currentHealth;
    }
    
    public float GetMaxHealth()
    {
        return MaxHealth;
    }
}
