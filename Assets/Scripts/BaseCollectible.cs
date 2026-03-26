using UnityEngine;

public class BaseCollectible : MonoBehaviour, ICollectible
{
    [SerializeField] float rotationSpeed = 100f;
    
    [SerializeField] GameObject collectParticle;
    [SerializeField] AudioClip collectSound;

    void Update()
    {
        transform.Rotate(Vector3.up * (rotationSpeed * Time.deltaTime));
    }

    public void Collect()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.AddScore();
        }

        if (collectSound != null)
        {
            AudioSource.PlayClipAtPoint(collectSound, transform.position);
        }

        if (collectParticle != null)
        {
            Instantiate(collectParticle, transform.position, Quaternion.identity);
        }

        Destroy(gameObject);
    }
}