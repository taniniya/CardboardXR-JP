#if VUPLEX_PRESENT
using UnityEngine;
using Vuplex.WebView;

namespace CardboardXR.Demos.Theater
{
    // The screen with the browser: Vuplex's 3D WebView prefab under the anchor, taking clicks and
    // drags itself through the same event system the panels use. Compiled only when VUPLEX_PRESENT
    // is defined, which the folder README's steps do after the plugin is imported; without it, the
    // placeholder takes this class's place and nothing here is compiled.
    public class VuplexScreen : IWebScreen
    {
        // Pixels per meter of screen. 800 keeps a 3.2 m screen near 2560 px across: sharp enough
        // to read, light enough for a phone that is also drawing the room twice.
        const float k_Resolution = 800f;

        WebViewPrefab m_Prefab;
        string m_PendingUrl;
        bool m_Relay;
        bool m_Ready;

        public bool Available => true;

        public void Build(Transform anchor, Vector2 sizeMeters)
        {
            m_Prefab = WebViewPrefab.Instantiate(sizeMeters.x, sizeMeters.y);
            m_Prefab.transform.SetParent(anchor, false);
            // A breath in front of the anchor's plane, so the gaze meets this screen's collider
            // before the anchor's when the page is taking clicks itself.
            m_Prefab.transform.localPosition = new Vector3(0f, 0f, 0.002f);
            m_Prefab.Resolution = k_Resolution;
            ApplyInputMode();
            InitAsync();
        }

        public void SetRelayClicks(bool relay)
        {
            m_Relay = relay;
            ApplyInputMode();
        }

        // Relay clicks: the prefab neither takes pointer input nor stops the gaze, so the anchor's
        // collider behind it gets the click and TheaterClickRelay forwards it to ClickAt.
        void ApplyInputMode()
        {
            if (m_Prefab == null)
                return;
            m_Prefab.ClickingEnabled = !m_Relay;
            if (m_Prefab.Collider != null)
                m_Prefab.Collider.enabled = !m_Relay;
        }

        async void InitAsync()
        {
            try
            {
                await m_Prefab.WaitUntilInitialized();
                m_Ready = true;
                ApplyInputMode();
                var url = string.IsNullOrEmpty(m_PendingUrl) ? TheaterLinks.HomeUrl : m_PendingUrl;
                m_PendingUrl = null;
                m_Prefab.WebView.LoadUrl(url);
                Debug.Log($"[Theater] Browser ready, loading {url}");
            }
            catch (System.Exception e)
            {
                Debug.LogWarning($"[Theater] Browser failed to start: {e.Message}");
            }
        }

        public void LoadUrl(string url)
        {
            if (!TheaterLinks.IsWebUrl(url))
            {
                Debug.LogWarning($"[Theater] Not a web page, not loading: {url}");
                return;
            }
            if (m_Ready)
                m_Prefab.WebView.LoadUrl(url);
            else
                m_PendingUrl = url;
        }

        public void GoBack()
        {
            if (m_Ready)
                m_Prefab.WebView.GoBack();
        }

        public void GoForward()
        {
            if (m_Ready)
                m_Prefab.WebView.GoForward();
        }

        public void Reload()
        {
            if (m_Ready)
                m_Prefab.WebView.Reload();
        }

        public void ClickAt(Vector2 uv)
        {
            if (m_Ready)
                m_Prefab.WebView.Click(TheaterMath.ToWebPoint(uv));
        }

        public void Dispose()
        {
            if (m_Prefab != null)
            {
                m_Prefab.Destroy();
                m_Prefab = null;
            }
            m_Ready = false;
        }
    }
}
#endif
