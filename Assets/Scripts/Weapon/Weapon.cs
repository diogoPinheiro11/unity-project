using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Weapon : MonoBehaviour
{
    public float damage = 10f;
    public float range = 100f;
    public float power = 2000f;
    public GameObject projectilePrefab; // Reference to the projectile prefab
    public int maxAmmo = 30; // Maximum ammo capacity

    public Transform muzzle; // Firing point on the weapon
    public TextMeshProUGUI ammoText; // TextMeshProGUI for displaying ammo count
    public TextMeshProUGUI reloadText; // TextMeshProGUI for displaying reload message

    public AudioClip shootSound; // Sound clip for the shoot action
    private AudioSource audioSource; // Reference to the AudioSource component

    private int currentAmmo; // Current ammo count

    void Start()
    {
        currentAmmo = maxAmmo; // Set initial ammo count
        UpdateAmmoText(); // Update the ammo text

        audioSource = GetComponent<AudioSource>(); // Get the AudioSource component
        audioSource.clip = shootSound; // Get the AudioSource component
    }

    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            if (currentAmmo > 0)
            {
                Shoot();
                currentAmmo--;
                UpdateAmmoText();
            }
            else
            {
                Debug.Log("Out of ammo!");
            }
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            Reload();
        }
    }

    void Shoot()
    {
        audioSource.Play();
        RaycastHit hit;
        if (Physics.Raycast(muzzle.position, transform.forward, out hit, range))
        {
            Zombie zombie = hit.transform.GetComponent<Zombie>();
            if (zombie != null)
            {
                zombie.TakeDamage(damage);
            }
        }

        // Instantiate the projectile at the muzzle position and in the direction of the weapon
        GameObject projectile = Instantiate(projectilePrefab, muzzle.position, transform.rotation);
        Rigidbody projectileRigidbody = projectile.GetComponent<Rigidbody>();
        projectileRigidbody.AddForce(transform.forward * power); // Apply a force to launch the projectile
        Destroy(projectile, 2f); // Destroy the projectile after a defined time
    }

    void Reload()
    {
        currentAmmo = maxAmmo;
        UpdateAmmoText();
        StartCoroutine(ShowReloadText());
        Debug.Log("Reloaded!");
    }

    void UpdateAmmoText()
    {
        ammoText.text = currentAmmo.ToString();
    }

    IEnumerator ShowReloadText()
    {
        reloadText.gameObject.SetActive(true);
        yield return new WaitForSeconds(1f);
        reloadText.gameObject.SetActive(false);
    }
}
