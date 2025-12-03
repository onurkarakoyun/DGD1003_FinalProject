using UnityEngine;

public class StealablePainting : MonoBehaviour
{
    public string interactionMessage = "Q - Tabloyu Çal";
    public float messageDuration = 2f;
    private bool playerInRange;
    private bool stolen;
    private float messageUntil;
    private SpriteRenderer paintingRenderer;
    private Collider2D paintingCollider;
    private bool isPainting = true;

    void Awake()
    {
        paintingRenderer = GetComponent<SpriteRenderer>();
        paintingCollider = GetComponent<Collider2D>();
        isPainting = GetComponent<PlayerController>() == null;
    }

    void Update()
    {
        if (playerInRange && !stolen && Input.GetKeyDown(KeyCode.Q) && isPainting)
        {
            stolen = true;
            if (paintingRenderer != null) paintingRenderer.enabled = false;
            if (paintingCollider != null) paintingCollider.enabled = false;
            messageUntil = Time.time + messageDuration;
            if (GameManager.Instance != null) GameManager.Instance.AddStolen();
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }

    void OnGUI()
    {
        if (!stolen && playerInRange)
        {
            var style = new GUIStyle(GUI.skin.box);
            style.alignment = TextAnchor.MiddleCenter;
            style.fontSize = 18;
            var rect = new Rect(Screen.width * 0.5f - 120, Screen.height * 0.85f, 240, 40);
            GUI.Box(rect, interactionMessage, style);
        }
        else if (stolen && Time.time < messageUntil)
        {
            var style = new GUIStyle(GUI.skin.box);
            style.alignment = TextAnchor.MiddleCenter;
            style.fontSize = 18;
            var rect = new Rect(Screen.width * 0.5f - 120, Screen.height * 0.85f, 240, 40);
            GUI.Box(rect, "Tablo çalındı!", style);
        }
    }
}
