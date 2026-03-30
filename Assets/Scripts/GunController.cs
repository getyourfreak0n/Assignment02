using UnityEngine;
using UnityEngine.InputSystem;

public class GunController : MonoBehaviour
{
    [SerializeField] float fireRate = 0.25f;
    [SerializeField] AudioClip gunshotSound;
    [SerializeField] Transform firePoint;
    [SerializeField] GameObject bulletPrefab;

    float nextFireTime;
    AudioSource audioSource;
    NewInputActions inputActions;

    void Awake()
    {
        inputActions = new NewInputActions();
        
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    void OnEnable()
    {
        inputActions.Enable();
        inputActions.Player.Shoot.performed += OnShootPerformed;
    }

    void OnDisable()
    {
        inputActions.Player.Shoot.performed -= OnShootPerformed;
        inputActions.Disable();
    }

    void OnShootPerformed(InputAction.CallbackContext context)
    {
        if (Time.time >= nextFireTime)
        {
            Shoot();
            nextFireTime = Time.time + fireRate;
        }
    }

    void Shoot()
    {
        if (gunshotSound != null)
        {
            audioSource.PlayOneShot(gunshotSound);
        }
        
        if (bulletPrefab != null && firePoint != null)
        {
            Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        }
    }
}