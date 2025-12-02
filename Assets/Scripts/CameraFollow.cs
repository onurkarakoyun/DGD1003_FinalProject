using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("Hedef Ayarları")]
    public Transform target; // Kameranın takip edeceği karakter
    public Vector3 offset = new Vector3(0f, 0f, -10f); // Mesafe (Z ekseni -10 olmalı ki 2D görünsün)
    
    [Header("Hız Ayarları")]
    public float smoothTime = 0.25f; // Kamera ne kadar gecikmeli gelsin? (Düşük = Hızlı, Yüksek = Yumuşak)
    
    private Vector3 velocity = Vector3.zero; // SmoothDamp için gerekli referans değişkeni

    // Kamera takipleri her zaman LateUpdate'te yapılır
    // Çünkü Update'te karakter hareket eder, LateUpdate'te kamera onun yeni yerine gider.
    void LateUpdate()
    {
        if (target == null) return; // Eğer hedef yoksa hiçbir şey yapma

        // Hedefin pozisyonu + ofset
        Vector3 targetPosition = target.position + offset;

        // Kamerayı mevcut konumundan hedef konuma yumuşakça taşı
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
    }

    // Karakter değiştiğinde bu fonksiyonu çağıracağız
    public void ChangeTarget(Transform newTarget)
    {
        target = newTarget;
    }
}
