 using UnityEngine;

public class LaserTrap : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Oyuncu lazere yakalandı!");
        }
    }
}
