using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie : MonoBehaviour
{
    public Transform shoulders;

    public float MaxSpeed;
    private float Speed;

    private Collider[] hitColliders;
    private RaycastHit hit;

    public float SightRange;
    public float DetectionRange;

    public Rigidbody rb;
    public GameObject Target;

    private bool seePlayer;

    public float Damage;
    public float KOTime;

    private bool canAttack = true;
    private bool isAttacking = false;

    // Vida do zumbi
    private float health = 100f;

    // Referência ao script do jogador para acessar a saúde
    private PlayerHealth playerHealth;

    public AudioSource audioSource;
    public AudioClip hitSound;
    public AudioClip deathSound;

    private Quaternion startRotation;
    private Quaternion targetRotation;

    void Start()
    {
        Speed = MaxSpeed;

        // Obter referência ao script do jogador
        playerHealth = FindObjectOfType<PlayerHealth>();

        // Obter referência ao componente AudioSource
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (health <= 0)
        {
            // O zumbi está morto, faça algo (por exemplo, reproduza o som de morte)
            Die();
            return;
        }

        if (!seePlayer)
        {
            hitColliders = Physics.OverlapSphere(transform.position, DetectionRange);
            foreach (var hitCollider in hitColliders)
            {
                if (hitCollider.CompareTag("Player"))
                {
                    Target = hitCollider.gameObject;
                    seePlayer = true;
                }
            }
        }
        else
        {
            if (Physics.Raycast(transform.position, Target.transform.position - transform.position, out hit, SightRange))
            {
                if (!hit.collider.CompareTag("Player"))
                {
                    seePlayer = false;
                }
                else
                {
                    var heading = Target.transform.position - transform.position;
                    var distance = heading.magnitude;
                    var direction = heading / distance;

                    Vector3 move = new Vector3(direction.x * Speed, 0, direction.z * Speed);
                    rb.velocity = move;
                    transform.forward = move;

                    // Verificar se o jogador está se movendo
                    bool isMoving = playerHealth.IsMoving();

                    // Verificar se o jogador está sendo atacado e não está se movendo
                    if (canAttack && !isMoving)
                    {
                        if (!isAttacking)
                        {
                            isAttacking = true;
                            StartCoroutine(AttackPlayer());
                        }
                    }
                }
            }
        }
    }

    private IEnumerator AttackPlayer()
    {
        canAttack = false;

        // Verificar se o jogador ainda está perto do zumbi
        bool isPlayerClose = Vector3.Distance(transform.position, playerHealth.transform.position) < 1f;

        if (isPlayerClose)
        {
            playerHealth.TakeDamage(Damage);
            PlayHitSound();

            yield return new WaitForSeconds(KOTime);
        }

        canAttack = true;
        isAttacking = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Player") && canAttack && !isAttacking)
        {
            isAttacking = true;
            collision.collider.gameObject.GetComponent<PlayerHealth>().TakeDamage(Damage);
            PlayHitSound();
            StartCoroutine(AttackDelay(KOTime));
        }
    }

    private IEnumerator AttackDelay(float Delay)
    {
        Speed = 0;
        canAttack = false;

        // Salvar a rotação inicial dos ombros
        startRotation = shoulders.localRotation;

        // Rodar o objeto a 45º para cima devagar no eixo x
        targetRotation = Quaternion.Euler(-45f, startRotation.eulerAngles.y, startRotation.eulerAngles.z);
        float rotationTime = 0f;
        float rotationDuration = 1f;

        while (rotationTime < rotationDuration)
        {
            rotationTime += Time.deltaTime;
            shoulders.localRotation = Quaternion.Lerp(startRotation, targetRotation, rotationTime / rotationDuration);
            yield return null;
        }

        // Rodar o objeto a 45º para baixo depressa no eixo x
        targetRotation = Quaternion.Euler(45f, startRotation.eulerAngles.y, startRotation.eulerAngles.z);
        rotationTime = 0f;
        rotationDuration = 0.5f;

        while (rotationTime < rotationDuration)
        {
            rotationTime += Time.deltaTime;
            shoulders.localRotation = Quaternion.Lerp(targetRotation, startRotation, rotationTime / rotationDuration);
            yield return null;
        }

        // Restaurar a rotação original
        shoulders.localRotation = startRotation;

        Speed = MaxSpeed;
        canAttack = true;
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        if (health <= 0f)
        {
            Die();
        }
    }

    private void Die()
    {
        if (deathSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(deathSound);
        }

        // Implemente o que acontece quando o zumbi morre, por exemplo, destruí-lo
        Destroy(gameObject);
    }

    private void PlayHitSound()
    {
        if (hitSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(hitSound);
        }
    }
}