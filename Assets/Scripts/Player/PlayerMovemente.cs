using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovemente : MonoBehaviour
{
    public Camera playerCamera;
    public float walkSpeed = 6f;
    public float runSpeed = 12f;
    public float aimSpeedMultiplier = 0.5f; // Multiplicador de velocidade enquanto mira
    public float jumpPower = 7f;
    public float gravity = 10f;

    public float lookSpeed = 2f;
    public float lookXLimit = 45f;

    public AudioClip walkSound;
    public AudioClip runSound;

    private AudioSource audioSource;
    private bool isWalking = false;
    private bool isRunning = false;
    private bool isAiming = false; // Variável para controlar se o jogador está mirando

    Vector3 moveDirection = Vector3.zero;
    float rotationX = 0;

    public bool canMove = true;

    CharacterController characterController;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        // Verifica se o jogador está no menu
        if (SceneManager.GetActiveScene().name == "GameOverMenu")
        {
            // Habilita o cursor
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            // Desabilita o cursor
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }

        #region Handles Movement
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);

        // Press Shift to run
        bool wasRunning = isRunning;
        isRunning = Input.GetKey(KeyCode.LeftShift);

        // Verifica se o botão direito do mouse está pressionado
        isAiming = Input.GetMouseButton(1);

        // Calcula a velocidade de movimento com base na condição de mira
        float speedMultiplier = isAiming ? aimSpeedMultiplier : 1f;
        float curSpeedX = canMove ? (isRunning || isWalking ? runSpeed : walkSpeed) * speedMultiplier * Input.GetAxis("Vertical") : 0;
        float curSpeedY = canMove ? (isRunning || isWalking ? runSpeed : walkSpeed) * speedMultiplier * Input.GetAxis("Horizontal") : 0;

        float movementDirectionY = moveDirection.y;
        moveDirection = (forward * curSpeedX) + (right * curSpeedY);

        // Verifica se o jogador está andando
        isWalking = Mathf.Abs(curSpeedX) > 0 || Mathf.Abs(curSpeedY) > 0;

        #endregion

        #region Handles Jumping
        if (Input.GetButton("Jump") && canMove && characterController.isGrounded)
        {
            moveDirection.y = jumpPower;
        }
        else
        {
            moveDirection.y = movementDirectionY;
        }

        if (!characterController.isGrounded)
        {
            moveDirection.y -= gravity * Time.deltaTime;
        }

        #endregion

        #region Handles Rotation
        characterController.Move(moveDirection * Time.deltaTime);

        if (canMove)
        {
            rotationX += -Input.GetAxis("Mouse Y") * lookSpeed;
            rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
            playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
            transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSpeed, 0);
        }

        #endregion

        #region Handles Audio
        if (isWalking)
{
    audioSource.clip = walkSound;

    if (!audioSource.isPlaying)
    {
        audioSource.Play();
    }
}
    else
    {
        audioSource.Stop();
    }
        #endregion
    }
}

