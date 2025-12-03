using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public int stolenCount;
    public bool gameOver;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AddStolen()
    {
        stolenCount++;
    }

    public void FinishLevel()
    {
        gameOver = true;
        var pc = FindAnyObjectByType<PlayerController>();
        if (pc != null) pc.isHidden = true;
    }
}
