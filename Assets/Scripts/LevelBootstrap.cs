using UnityEngine;
using UnityEngine.Rendering.Universal;

public static class LevelBootstrap
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    static void Init()
    {
        var player = Object.FindAnyObjectByType<PlayerController>();
        if (player != null) player.gameObject.tag = "Player";

        var existingLight = Object.FindAnyObjectByType<Light2D>();
        if (existingLight == null)
        {
            var lightGo = new GameObject("Global Light 2D");
            var light = lightGo.AddComponent<Light2D>();
            light.lightType = Light2D.LightType.Global;
            light.intensity = 1f;
        }

        var go = new GameObject("Tablo");
        var sr = go.AddComponent<SpriteRenderer>();
        sr.sortingOrder = 100;

        var resTex = Resources.Load<Texture2D>("painting");
        if (resTex != null)
        {
            var sp = Sprite.Create(resTex, new Rect(0, 0, resTex.width, resTex.height), new Vector2(0.5f, 0.5f), 100f);
            sr.sprite = sp;
        }
        else
        {
            var w = 128;
            var h = 128;
            var tex = new Texture2D(w, h, TextureFormat.RGBA32, false);
            for (int y = 0; y < h; y++)
            {
                for (int x = 0; x < w; x++)
                {
                    var c = ((x / 16 + y / 16) % 2 == 0) ? new Color(0.85f, 0.85f, 0.95f, 1f) : new Color(0.15f, 0.5f, 0.9f, 1f);
                    tex.SetPixel(x, y, c);
                }
            }
            tex.Apply(false, true);
            var sp = Sprite.Create(tex, new Rect(0, 0, w, h), new Vector2(0.5f, 0.5f), 64f);
            sr.sprite = sp;
        }

        var cam = Camera.main;
        var pos = new Vector3(1.5f, -1f, 0f);
        if (cam != null)
        {
            pos = cam.transform.position + new Vector3(2f, -1f, 10f);
            pos.z = 0f;
        }
        go.transform.position = pos;
        if (sr.sprite != null)
        {
            var desiredWidth = 2.0f;
            var currentWidth = sr.sprite.bounds.size.x;
            var scale = desiredWidth / currentWidth;
            go.transform.localScale = new Vector3(scale, scale, 1f);
        }
        else
        {
            go.transform.localScale = new Vector3(1.0f, 1.0f, 1f);
        }
        var bc = go.AddComponent<BoxCollider2D>();
        bc.isTrigger = true;
        if (sr.sprite != null)
        {
            bc.size = sr.sprite.bounds.size;
            bc.offset = Vector2.zero;
        }
        go.AddComponent<StealablePainting>();

        var gm = Object.FindAnyObjectByType<GameManager>();
        if (gm == null)
        {
            var gmGo = new GameObject("GameManager");
            gm = gmGo.AddComponent<GameManager>();
        }
        var hud = Object.FindAnyObjectByType<HUD>();
        if (hud == null)
        {
            var hudGo = new GameObject("HUD");
            hudGo.AddComponent<HUD>();
        }

        var left = new GameObject("LeftBoundary");
        var leftCol = left.AddComponent<BoxCollider2D>();
        left.transform.position = new Vector3(-9f, 0f, 0f);
        leftCol.size = new Vector2(0.5f, 20f);

        var exit = new GameObject("ExitDoor");
        var exitSr = exit.AddComponent<SpriteRenderer>();
        exitSr.sortingOrder = 90;
        var dw = 64;
        var dh = 128;
        var dtex = new Texture2D(dw, dh, TextureFormat.RGBA32, false);
        for (int y = 0; y < dh; y++)
        {
            for (int x = 0; x < dw; x++)
            {
                var c = new Color(0.2f, 0.8f, 0.2f, 1f);
                dtex.SetPixel(x, y, c);
            }
        }
        dtex.Apply(false, true);
        var dsp = Sprite.Create(dtex, new Rect(0, 0, dw, dh), new Vector2(0.5f, 0.5f), 100f);
        exitSr.sprite = dsp;
        exit.transform.position = new Vector3(21.5f, -1.5f, 0f);
        var exitCol = exit.AddComponent<BoxCollider2D>();
        exitCol.isTrigger = true;
        if (exitSr.sprite != null)
        {
            var s = exitSr.sprite.bounds.size;
            exitCol.size = s;
        }
        exit.AddComponent<ExitDoor>();

        var right = new GameObject("RightBoundary");
        var rightCol = right.AddComponent<BoxCollider2D>();
        right.transform.position = new Vector3(22.5f, 0f, 0f);
        rightCol.size = new Vector2(0.5f, 20f);

        var srs = Object.FindObjectsByType<SpriteRenderer>(FindObjectsSortMode.None);
        var minX = float.MaxValue;
        var maxX = float.MinValue;
        foreach (var r in srs)
        {
            var b = r.bounds;
            if (b.min.x < minX) minX = b.min.x;
            if (b.max.x > maxX) maxX = b.max.x;
        }
        if (minX < float.MaxValue && maxX > float.MinValue)
        {
            left.transform.position = new Vector3(minX - 0.5f, 0f, 0f);
            right.transform.position = new Vector3(maxX + 0.5f, 0f, 0f);
            exit.transform.position = new Vector3(maxX - 0.5f, exit.transform.position.y, 0f);
        }

        var all = Object.FindObjectsByType<Transform>(FindObjectsSortMode.None);
        foreach (var t in all)
        {
            var n = t.gameObject.name.ToLowerInvariant();
            if (n.Contains("kolon"))
            {
                var s = t.localScale;
                t.localScale = new Vector3(s.x * 0.6f, s.y, s.z);
                var c = t.GetComponent<BoxCollider2D>();
                if (c != null)
                {
                    var cs = c.size;
                    c.size = new Vector2(cs.x * 0.6f, cs.y);
                }
            }
        }

        var columns = new System.Collections.Generic.List<Transform>();
        foreach (var t in all)
        {
            var n = t.gameObject.name.ToLowerInvariant();
            if (n.Contains("kolon")) columns.Add(t);
        }
        if (columns.Count > 0 && minX < float.MaxValue && maxX > float.MinValue)
        {
            for (int i = 0; i < columns.Count; i++)
            {
                var f = (i + 1f) / (columns.Count + 1f);
                var x = Mathf.Lerp(minX + 0.5f, maxX - 0.5f, f);
                var p = columns[i].position;
                columns[i].position = new Vector3(x, p.y, p.z);
            }
        }

        if (columns.Count > 0)
        {
            var p0 = columns[0].position;
            go.transform.position = new Vector3(p0.x, go.transform.position.y, 0f);
        }
        if (columns.Count > 1)
        {
            var go2 = new GameObject("Tablo2");
            var sr2 = go2.AddComponent<SpriteRenderer>();
            sr2.sortingOrder = 100;
            if (sr.sprite != null) sr2.sprite = sr.sprite;
            go2.transform.localScale = go.transform.localScale;
            var p1 = columns[1].position;
            go2.transform.position = new Vector3(p1.x, go.transform.position.y, 0f);
            var bc2 = go2.AddComponent<BoxCollider2D>();
            bc2.isTrigger = true;
            if (sr2.sprite != null)
            {
                bc2.size = sr2.sprite.bounds.size;
                bc2.offset = Vector2.zero;
            }
            go2.AddComponent<StealablePainting>();
        }
    }
}
