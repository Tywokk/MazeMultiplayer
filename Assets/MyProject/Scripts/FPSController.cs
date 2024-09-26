using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

[RequireComponent(typeof(CharacterController))]
public class FPSController : NetworkBehaviour
{
    public Camera playerCamera;
    public AudioListener audioListener;
    [SerializeField] private AudioClip _playSound;
    private AudioSource _audioSource;
    public float walkSpeed = 6f;
    public float runSpeed = 12f;
    public float jumpPower = 7f;
    public float gravity = 15f;
    [SerializeField] float maxEnergy = 100f;
    [SerializeField] float currentEnergy;

    private Coroutine recovery;
    public float recoveryRate;


    public float lookSpeed = 5f;
    public float lookXLimit = 45f;


    Vector3 moveDirection = Vector3.zero;
    float movementDirectionY;
    float rotationX = 0;

    [SerializeField] Canvas playerName;
    public StaminaBar staminaBar;
    public bool canMove = true;

    NetworkVariable<int> randomNumber = new NetworkVariable<int>(1, 
        NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    CharacterController characterController;

    public static Action onPlayerSpawn;

    void Start()
    {
        if (!IsOwner) { return; }
        characterController = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        characterController.enabled = false;
        transform.position = new Vector3(5.2f, 2, 3);
        characterController.enabled = true;
        playerName.enabled = false;
        currentEnergy = maxEnergy;
        staminaBar.SetMaxStamina(currentEnergy);
        _audioSource = GetComponent<AudioSource>();
    }
    public override void OnNetworkSpawn()
    {
        onPlayerSpawn?.Invoke();
        randomNumber.OnValueChanged += (int previousValue, int newValue) =>
        {
            Debug.Log($"{OwnerClientId} : {randomNumber.Value}");
        };
    }
    private void Update()
    {
        if (!IsOwner)
        {
            playerCamera.enabled = false;
            audioListener.enabled = false;
            return;
        }
        LookAround();
        if (Input.GetKeyDown(KeyCode.Q))
        {
            randomNumber.Value = UnityEngine.Random.Range(0, 50);
        }
    }
    void FixedUpdate()
    {
        if (!IsOwner)
        {
            return;
        }
        else
        {
            Move();
            Jump();
            characterController.Move(moveDirection * Time.fixedDeltaTime);
        }

    }
    private void Move()
    {
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);

        // Press Left Shift to run
        bool isRunning;
        if (currentEnergy > 0 && Input.GetKey(KeyCode.LeftShift))
        {
            WasteEnergy(10f);
            isRunning = true;
        }
        else
        {
            isRunning = false;
        }
        float curSpeedX = canMove ? (isRunning ? runSpeed : walkSpeed) 
            * Input.GetAxis("Vertical") : 0;
        float curSpeedY = canMove ? (isRunning ? runSpeed : walkSpeed) 
            * Input.GetAxis("Horizontal") : 0;
        movementDirectionY = moveDirection.y;
        moveDirection = (forward * curSpeedX) + (right * curSpeedY);
    }
    private void Jump()
    {
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
            moveDirection.y -= gravity * Time.fixedDeltaTime;
        }
    }
    private void LookAround()
    {
        if (canMove)
        {
            rotationX += -Input.GetAxis("Mouse Y") * lookSpeed;
            rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
            playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
            transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSpeed, 0);
        }
    }
    private void PlaySoundStep()
    {
        _audioSource.PlayOneShot(_playSound, 1f);
    }
    public void WasteEnergy(float energy)
    {
        //Debug.Log("Run WasteEnergy");
        currentEnergy -= energy * Time.fixedDeltaTime;
        if (currentEnergy < 0)
        {
            currentEnergy = 0;
        }
        staminaBar.SetStamina(currentEnergy);
        if (recovery != null) StopCoroutine(recovery);
        recovery = StartCoroutine(RecoveryEnergy());
    }
    private IEnumerator RecoveryEnergy()
    {
        yield return new WaitForSeconds(1f);
        while (currentEnergy < maxEnergy)
        {
            currentEnergy += recoveryRate / 10f;
            if (currentEnergy > maxEnergy)
            {
                currentEnergy = maxEnergy;
            }
            staminaBar.SetStamina(currentEnergy);
            yield return new WaitForSeconds(.1f);
        }
    }
    public void ChangeMaxEnergy(float energy)
    {
        ChangeMaxEnergyClientRpc(energy);
    }
    [ClientRpc]
    public void ChangeMaxEnergyClientRpc(float energy)
    {
        maxEnergy += energy;
        staminaBar.ChangeMaxStamina(energy);
        recovery = StartCoroutine(RecoveryEnergy());
    }
    public void AddEnergy(float energy)
    {
        AddEnergyClientRpc(energy);
    }
    [ClientRpc]
    public void AddEnergyClientRpc(float energy)
    {
        currentEnergy += energy;
        staminaBar.SetStamina(currentEnergy);
    }
}