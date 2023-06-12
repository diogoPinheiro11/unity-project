using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingHealthRegen : MonoBehaviour
{
    public float regenAmount = 5f;
    public float regenInterval = 2.5f;

    public AudioClip HealSound;

    private PlayerHealth playerHealth;
    private AudioSource audioSource;
    private bool isPlayerInside = false;
    private Coroutine regenCoroutine;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerHealth = other.GetComponent<PlayerHealth>();
            isPlayerInside = true;

            regenCoroutine = StartCoroutine(RegenerateHealth());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInside = false;

            if (regenCoroutine != null)
            {
                StopCoroutine(regenCoroutine);
            }
        }
    }

    private IEnumerator RegenerateHealth()
    {
        while (isPlayerInside)
        {
            if (playerHealth.GetCurrentHealth() < 100)
            {
                playerHealth.Heal(regenAmount);
                audioSource.clip = HealSound;
                audioSource.Play();
            }

            yield return new WaitForSeconds(regenInterval);
        }
    }
}
