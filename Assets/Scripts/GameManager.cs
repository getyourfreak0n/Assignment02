using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField] int totalCollectibles = 3;
    int currentCollected;

    public UnityEvent onAllCollected;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    public void AddScore()
    {
        currentCollected++;

        if (currentCollected >= totalCollectibles)
        {
            onAllCollected.Invoke();
        }
    }
}