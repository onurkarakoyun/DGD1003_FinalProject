using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class HidingSpot : MonoBehaviour
{
    [Header("Gerekli Bileşenler")]
    public SpriteRenderer columnRenderer; 
    public TextMeshProUGUI interactionText; 

    [Header("Mesajlar")]
    public string hideMessage = "E - Saklan";
    public string exitMessage = "E - Çık";
    private bool isPlayerInRange = false; 
    private bool isPlayerHiding = false;  
    private bool canInteract = true; 
    private PlayerController playerScript; 
    private SpriteRenderer playerRenderer;
    private Rigidbody2D playerRb;
    private float originalGravity;
    private int originalSortingOrder;
    void Start()
    {
        if (columnRenderer == null) columnRenderer = GetComponent<SpriteRenderer>();
        if (interactionText == null) interactionText = GetComponentInChildren<TextMeshProUGUI>();
        if(interactionText != null) interactionText.gameObject.SetActive(false);
    }

    void Update()
    {
        if (isPlayerInRange && canInteract && Input.GetKeyDown(KeyCode.E))
        {
            if (isPlayerHiding)
                ExitHiding();
            else
                EnterHiding();
        }
        if (isPlayerHiding && playerRb != null)
        {
            playerRb.linearVelocity = Vector2.zero;
        }
    }

    void EnterHiding()
    {
        if (playerScript == null) return;

        isPlayerHiding = true;
        playerRb.linearVelocity = Vector2.zero;
        playerRb.gravityScale = 0f;
        playerRb.constraints = RigidbodyConstraints2D.FreezeRotation; 
        playerScript.transform.position = new Vector3(transform.position.x, playerScript.transform.position.y, playerScript.transform.position.z);
        playerScript.isHidden = true;
        playerScript.gameObject.tag = "Untagged"; 
        originalSortingOrder = playerRenderer.sortingOrder;
        
        if (columnRenderer != null)
        {
            playerRenderer.sortingOrder = columnRenderer.sortingOrder - 1; 
            Color colColor = columnRenderer.color;
            colColor.a = 0.5f; 
            columnRenderer.color = colColor;
        }

        UpdateUIText(exitMessage);
        StartCoroutine(InteractionCooldown());
    }

    void ExitHiding()
    {
        isPlayerHiding = false;
        playerScript.isHidden = false;
        playerScript.gameObject.tag = "Player";
        playerRb.gravityScale = originalGravity;
        playerRenderer.sortingOrder = originalSortingOrder; 

        if (columnRenderer != null)
        {
            Color colColor = columnRenderer.color;
            colColor.a = 1f; 
            columnRenderer.color = colColor;
        }

        UpdateUIText(hideMessage);
        StartCoroutine(InteractionCooldown());
    }
    IEnumerator InteractionCooldown()
    {
        canInteract = false;
        yield return new WaitForSeconds(0.2f);
        canInteract = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerInRange = true;
            playerScript = collision.GetComponent<PlayerController>();
            playerRenderer = collision.GetComponent<SpriteRenderer>();
            playerRb = collision.GetComponent<Rigidbody2D>();
            
            if(playerRb != null) originalGravity = playerRb.gravityScale;

            UpdateUIText(hideMessage);
            if(interactionText != null) interactionText.gameObject.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (isPlayerHiding) return; 

        if (collision.GetComponent<PlayerController>() != null)
        {
            isPlayerInRange = false;
            if(interactionText != null) interactionText.gameObject.SetActive(false);
        }
    }

    void UpdateUIText(string message)
    {
        if (interactionText != null) interactionText.text = message;
    }
}