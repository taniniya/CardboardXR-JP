using CardboardXR.UI;
using TMPro;
using UnityEngine;

namespace CardboardXR.Demos.Theater
{
    // Web Theater: a big screen in a dark room showing a web page, YouTube to start with, worked by
    // looking and pinching. The screen is placed in front of the head when the demo opens and stays
    // there. The page itself takes clicks and drags when the browser plugin is in the project; a
    // small opener below the screen brings up the control panel, with the shortcuts and the way
    // back, forward and to reload. Without the plugin the screen is a placeholder, a pinch on it
    // opens the panel instead, and the panel says why. 3DoF with no hands, like the video demo, so
    // the heat goes to the browser. Logs [Theater].
    public class TheaterDemo : Demo
    {
        const float k_ScreenDistance = 3.4f;
        const float k_PanelDistance = 2.2f;
        const float k_OpenerDistance = 2.0f;
        static readonly Vector2 k_PanelSize = new(560f, 316f);
        static readonly Vector2 k_OpenerSize = new(300f, 96f);

        [SerializeField] TheaterScreen m_Screen;

        CoreServices m_Core;
        RectTransform m_Panel;
        TMP_Text m_Status;
        TMP_Text m_Hint;
        bool m_PanelShown;

        public override void OnDemoEnter(CoreServices core)
        {
            m_Core = core;
            core.Placer.ObjectPrefab = null;
            if (m_Screen == null)
            {
                Debug.LogError("[Theater] No screen wired in the scene; nothing to show");
                return;
            }
            PlaceScreen();
            m_Screen.Build();
            m_Screen.ClickedAt += OnScreenClicked;
            BuildPanels();
            OpenLink(TheaterLinks.All[0]);
            Debug.Log($"[Theater] Ready: {(m_Screen.BrowserAvailable ? "browser" : "placeholder")} screen");
        }

        public override void OnDemoExit(CoreServices core)
        {
            if (m_Screen != null)
            {
                m_Screen.ClickedAt -= OnScreenClicked;
                m_Screen.Dispose();
            }
            Debug.Log("[Theater] Leaving");
        }

        // In front of where the head faces when the demo opens, at eye height, facing the viewer.
        // It stays there: turning the head looks around the room, not at a screen that follows.
        void PlaceScreen()
        {
            var head = m_Core.Head;
            var flat = Vector3.ProjectOnPlane(head.forward, Vector3.up);
            var forward = flat.sqrMagnitude > 1e-4f ? flat.normalized : Vector3.forward;
            var position = head.position + forward * k_ScreenDistance;
            position.y = head.position.y + 0.1f;
            m_Screen.transform.SetPositionAndRotation(position,
                Quaternion.LookRotation(head.position - position, Vector3.up));
        }

        void OnScreenClicked(Vector2 uv)
        {
            // The relay only takes clicks when the page isn't taking them itself: with a browser
            // (the relay fallback) the click goes to the page; on the placeholder it works the
            // panel, the way a tap on the video works that demo's controls.
            if (m_Screen.BrowserAvailable)
                m_Screen.ClickAt(uv);
            else
                TogglePanel();
        }

        void OpenLink(TheaterLinks.Link link)
        {
            m_Screen.LoadUrl(link.Url);
            SetStatus(link.Label);
            Debug.Log($"[Theater] Opening {link.Label}: {link.Url}");
        }

        void SetStatus(string page)
        {
            if (m_Status != null)
                m_Status.text = m_Screen.BrowserAvailable ? page : $"{page}（ダミー画面）";
        }

        void TogglePanel()
        {
            if (m_PanelShown)
                HidePanel();
            else
                ShowPanel();
        }

        void ShowPanel()
        {
            m_PanelShown = true;
            WorldUi.PlaceInFront(m_Panel, m_Core.Head, k_PanelDistance, -18f);
            m_Panel.sizeDelta = k_PanelSize;
            M3Fade.Show(m_Panel.gameObject);
        }

        void HidePanel()
        {
            m_PanelShown = false;
            M3Fade.Hide(m_Panel.gameObject);
        }

        void BuildPanels()
        {
            // The opener: always there below the screen, one button for the control panel.
            var opener = WorldUi.Panel("Theater Opener", transform, k_OpenerSize, k_OpenerDistance);
            M3.Button("Controls", opener, "コントロール", M3Button.Variant.Filled, Vector2.zero,
                TogglePanel, "settings", 240f);
            WorldUi.PlaceInFront(opener, m_Core.Head, k_OpenerDistance, -32f);
            opener.sizeDelta = k_OpenerSize;

            m_Panel = WorldUi.Panel("Theater Panel", transform, k_PanelSize, k_PanelDistance);
            m_Status = M3.Text("Status", m_Panel, "", M3Theme.Type.BodyMedium, new Vector2(-20f, 126f),
                new Vector2(480f, 24f), M3Theme.OnSurfaceVariant);
            M3.IconButton("Close", m_Panel, "close", M3Button.Variant.Text, new Vector2(252f, 126f), HidePanel);

            // The shortcuts.
            const float width = 168f;
            var youtube = TheaterLinks.Find("youtube");
            var subscriptions = TheaterLinks.Find("subscriptions");
            var music = TheaterLinks.Find("music");
            M3.Button("YouTube", m_Panel, youtube.Label, M3Button.Variant.Tonal, new Vector2(-176f, 62f),
                () => OpenLink(youtube), "movie", width);
            M3.Button("Subscriptions", m_Panel, subscriptions.Label, M3Button.Variant.Tonal, new Vector2(0f, 62f),
                () => OpenLink(subscriptions), "video_library", width);
            M3.Button("Music", m_Panel, music.Label, M3Button.Variant.Tonal, new Vector2(176f, 62f),
                () => OpenLink(music), null, width);

            // The way around the page.
            M3.IconButton("Back", m_Panel, "chevron_left", M3Button.Variant.Tonal, new Vector2(-150f, -30f), () =>
            {
                m_Screen.GoBack();
                Debug.Log("[Theater] Back");
            });
            M3.IconButton("Forward", m_Panel, "chevron_right", M3Button.Variant.Tonal, new Vector2(-50f, -30f), () =>
            {
                m_Screen.GoForward();
                Debug.Log("[Theater] Forward");
            });
            M3.IconButton("Reload", m_Panel, "refresh", M3Button.Variant.Tonal, new Vector2(50f, -30f), () =>
            {
                m_Screen.Reload();
                Debug.Log("[Theater] Reload");
            });
            M3.IconButton("Home", m_Panel, "home", M3Button.Variant.Tonal, new Vector2(150f, -30f),
                () => OpenLink(TheaterLinks.All[0]));

            m_Hint = M3.Text("Hint", m_Panel, "", M3Theme.Type.BodyMedium, new Vector2(0f, -116f),
                new Vector2(520f, 44f), M3Theme.OnSurfaceVariant);
            m_Hint.text = m_Screen.BrowserAvailable
                ? "画面はピンチでクリック、押しながらの視線移動でスクロールできます"
                : "ブラウザ（Vuplex 3D WebView）が未導入のため、ダミー画面です。導入手順はフォルダの README を見てください";

            HidePanel();
        }
    }
}
