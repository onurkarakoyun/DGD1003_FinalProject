using UnityEngine;

public class HUD : MonoBehaviour
{
    void OnGUI()
    {
        var count = GameManager.Instance != null ? GameManager.Instance.stolenCount : 0;
        var style = new GUIStyle(GUI.skin.box);
        style.alignment = TextAnchor.UpperLeft;
        style.fontSize = 18;
        var rect = new Rect(10, 10, 160, 28);
        GUI.Box(rect, "Toplanan eser: " + count, style);

        if (GameManager.Instance != null && GameManager.Instance.gameOver)
        {
            var s2 = new GUIStyle(GUI.skin.box);
            s2.alignment = TextAnchor.MiddleCenter;
            s2.fontSize = 28;
            var r2 = new Rect(Screen.width * 0.5f - 150, Screen.height * 0.5f - 25, 300, 50);
            GUI.Box(r2, "Oyun bitti!", s2);
        }
    }
}
