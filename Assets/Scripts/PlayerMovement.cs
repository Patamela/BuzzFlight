using System;
using UnityEngine;
using UnityEngine.InputSystem;
using DG.Tweening;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{
    //1º Interfaz, 2º Componentes, 3º Variables Internas

    [Header("Input Actions")]
    [SerializeField] private InputAction thrust;
    [SerializeField] private InputAction rotation;
    [SerializeField] private InputAction shoot;
    [SerializeField] private InputAction dash;

    [Header("Movement Settings")]
    [SerializeField] private float moveForce = 10f;
    [SerializeField] private float rotationForce = 2.5f;

    [Header("Dash Settings")]
    [SerializeField] float dashDistance = 10f;
    [SerializeField] float dashDuration = 1f;
    [SerializeField] float maxCooldown = 10f;

    [Header("Particle Systems")]
    [SerializeField] private ParticleSystem thrustParticleSystem;
    
    [Header("Audio Settings")]
    [SerializeField] private AudioClip thrustAudioClip;
    
    Rigidbody rb;
    AudioSource audioSource;
    PlayerShooting playerShooting;
    PlayerHealth playerHealth;
    Tween dashTween;    
    float cooldownTimer;
    bool isDashing = false;
    bool dashInterrupted = false;

    public bool DashInterrupted{get {return dashInterrupted;} set {dashInterrupted = value; }}
    public bool IsDashing{ get{ return isDashing; } set{ isDashing = value; }}
    public float CoolDownTimer{ get{ return cooldownTimer; } set{ cooldownTimer = value < 0 ? 0 : value; }}

    void OnEnable()
    {
        thrust.Enable();
        rotation.Enable();
        shoot.Enable();
        dash.Enable();
    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        //audioSource = GetComponent<AudioSource>();
        playerShooting = GetComponent<PlayerShooting>();
        playerHealth = GetComponent<PlayerHealth>();
        CoolDownTimer = maxCooldown;
    }

    void FixedUpdate()
    {
        ProcessThrust();
        ProcessRotation();
        ProcessShooting();
        ProcessDash();
        if (dashInterrupted)
        {
            dashTween.Kill();
            StopCoroutine(DoDash());
        }
    }

    private void ProcessDash()
    {   
        CoolDownTimer -= Time.deltaTime;

        if(dash.IsPressed() && CoolDownTimer <= 0 && !isDashing)
        {   
            isDashing = true;
            StartCoroutine(DoDash());
        }
    }

    private IEnumerator DoDash()
    {      
        Vector3 targetPosition = rb.position + Vector3.right * dashDistance;

        playerHealth.IsVulnerable = false;
        dashTween = rb
            .DOMove(targetPosition, dashDuration)
            .SetEase(Ease.Linear);

        CoolDownTimer = maxCooldown;
        yield return new WaitForSeconds(dashDuration);
        playerHealth.IsVulnerable = true; 
        isDashing = false;
    }

    private void ProcessShooting()
    {
        if (shoot.IsPressed()) playerShooting.Shoot();
    
    }

    private void ProcessThrust()
    {
        if (thrust.IsPressed())
        {
            StartThrusting();
        }
        else
        {
            StopThrusting();
        }
    }

    private void ProcessRotation()
    {
        rb.freezeRotation = true; //freezinfn rotation so we can manually rotate

        float rotationValue = rotation.ReadValue<float>();

        if(rotationValue < 0)
        {
            transform.Rotate(Vector3.forward * rotationForce);
        }
        else if (rotationValue > 0)
        {
            transform.Rotate(Vector3.back * rotationForce);
        }
    }


    private void StopThrusting()
    {
        thrustParticleSystem.Stop();
        if(audioSource!=null)audioSource.Stop();
    }

    private void StartThrusting()
    {
        rb.AddRelativeForce(Vector3.up * moveForce);

        if (!thrustParticleSystem.isPlaying)
        {
            thrustParticleSystem.Play();
        }

        /*if (!audioSource.isPlaying)
        {   
            if(thrustAudioClip!=null)  audioSource.PlayOneShot(thrustAudioClip);
        }*/
    }
}
