using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HidingSpot : MonoBehaviour
{
    [Header("Ayarlar")]
    private TextMeshProUGUI interactionText; // Ekranda görünecek Text objesini buraya sürükle
    public string hideMessage = "E - Saklan";
    public string exitMessage = "E - Çık";

    private bool isPlayerInRange = false; // Oyuncu kolonun yanında mı?
    private bool isPlayerHiding = false;  // Oyuncu şu an bu kolonda saklanıyor mu?
    
    private PlayerController playerScript; // Oyuncunun scriptine erişim
    private SpriteRenderer playerRenderer;
    private Rigidbody2D playerRb;
    private float originalGravity;
    private string originalTag; // Oyuncunun orijinal etiketi (Player)

    void Start()
    {
        interactionText = GetComponentInChildren<TextMeshProUGUI>();
        // Başlangıçta yazıyı gizle
        if(interactionText != null)
            interactionText.gameObject.SetActive(false);
    }

    void Update()
    {
        // Eğer oyuncu alandaysa VE E tuşuna basarsa
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.E))
        {
            if (isPlayerHiding)
            {
                ExitHiding(); // Zaten saklanıyorsa çık
            }
            else
            {
                EnterHiding(); // Saklanmıyorsa saklan
            }
        }
    }

    void EnterHiding()
    {
        isPlayerHiding = true;
        
        // 1. Oyuncu hareketini kilitle
        playerScript.isHidden = true;

        // 2. Tag değiştir (Düşmanlar göremez)
        playerScript.gameObject.tag = "Untagged"; 

        // 3. Yerçekimini ve Hızı Sıfırla (Yere batmayı engeller)
        playerRb.linearVelocity = Vector2.zero;
        playerRb.gravityScale = 0f; 

        // 4. Görseli Saydamlaştır
        Color hiddenColor = playerRenderer.color;
        hiddenColor.a = 0.5f; 
        playerRenderer.color = hiddenColor;
        playerRenderer.sortingOrder = -1;

        // NOT: Collider'ı KAPATMIYORUZ. Yanıp sönme sorunu buydu.
        
        UpdateUIText(exitMessage);
    }

    void ExitHiding()
    {
        isPlayerHiding = false;

        // 1. Hareketi aç
        playerScript.isHidden = false;

        // 2. Etiketi geri yükle
        playerScript.gameObject.tag = "Player";

        // 3. Yerçekimini eski haline getir
        playerRb.gravityScale = originalGravity;

        // 4. Görünümü düzelt
        Color normalColor = playerRenderer.color;
        normalColor.a = 1f; 
        playerRenderer.color = normalColor;
        playerRenderer.sortingOrder = 0; 

        UpdateUIText(hideMessage);
    }

    // Oyuncu Kolonun Yanına Geldiğinde
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerInRange = true;
            playerScript = collision.GetComponent<PlayerController>();
            playerRenderer = collision.GetComponent<SpriteRenderer>();
            playerRb = collision.GetComponent<Rigidbody2D>();
            
            // Orjinal yerçekimi değerini kaydet
            originalGravity = playerRb.gravityScale;

            UpdateUIText(hideMessage);
            if(interactionText != null) interactionText.gameObject.SetActive(true);
        }
    }

    // Oyuncu Kolonun Yanından Ayrıldığında
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<PlayerController>() != null)
        {
            isPlayerInRange = false;
            if(interactionText != null) interactionText.gameObject.SetActive(false);
            
            // Eğer saklanırken oyuncu bir şekilde çıkarsa resetle
            if (isPlayerHiding) ExitHiding();
        }
    }

    void UpdateUIText(string message)
    {
        if (interactionText != null)
            interactionText.text = message;
    }
}
