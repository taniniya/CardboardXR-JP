using System;
using UnityEngine;

namespace CardboardXR.Demos.Theater
{
    // The theater's screen, on the scene's Screen Anchor: a dark bezel, the web page on it (or the
    // placeholder), and the collider the gaze clicks land on while the page isn't taking them
    // itself. TheaterDemo drives it; the page behind it is an IWebScreen.
    public class TheaterScreen : MonoBehaviour
    {
        [SerializeField] Vector2 m_Size = new(3.2f, 1.8f);
        [Tooltip("Send the screen's clicks through the relay (IWebScreen.ClickAt) instead of the web " +
                 "view's own pointer handling. Only needed if clicks don't reach the page.")]
        [SerializeField] bool m_ForceRelayClicks;

        IWebScreen m_Backend;
        BoxCollider m_Collider;
        TheaterClickRelay m_Relay;
        GameObject m_Bezel;
        Material m_BezelMaterial;
        Texture2D m_BezelTexture;

        public bool BrowserAvailable => m_Backend != null && m_Backend.Available;
        public Vector2 Size => m_Size;

        // A click on the screen's surface, in texture coordinates: (0, 0) is its bottom-left. Only
        // raised while the relay is taking clicks (the placeholder, or the relay fallback).
        public event Action<Vector2> ClickedAt;

        public void Build()
        {
            if (m_Backend != null)
                return;
            m_Collider = GetComponent<BoxCollider>();
            m_Relay = GetComponent<TheaterClickRelay>();
            MakeBezel();
            m_Backend = CreateBackend();
            m_Backend.SetRelayClicks(m_ForceRelayClicks);
            m_Backend.Build(transform, m_Size);
            // The page takes its own clicks when there is a browser and the relay isn't forced;
            // otherwise the anchor's collider stops the gaze and the relay forwards the click.
            var native = m_Backend.Available && !m_ForceRelayClicks;
            if (m_Collider != null)
                m_Collider.enabled = !native;
            if (m_Relay != null)
            {
                m_Relay.Taking = !native;
                m_Relay.ClickedAt += OnRelayClick;
            }
            Debug.Log($"[Theater] Screen built: {(m_Backend.Available ? "browser" : "placeholder")}, " +
                      $"clicks by {(native ? "the page itself" : "the relay")}");
        }

        static IWebScreen CreateBackend()
        {
#if VUPLEX_PRESENT
            return new VuplexScreen();
#else
            return new PlaceholderScreen();
#endif
        }

        void OnRelayClick(Vector2 uv) => ClickedAt?.Invoke(uv);

        public void LoadUrl(string url) => m_Backend?.LoadUrl(url);

        public void GoBack() => m_Backend?.GoBack();

        public void GoForward() => m_Backend?.GoForward();

        public void Reload() => m_Backend?.Reload();

        public void ClickAt(Vector2 uv) => m_Backend?.ClickAt(uv);

        public void Dispose()
        {
            if (m_Relay != null)
                m_Relay.ClickedAt -= OnRelayClick;
            m_Backend?.Dispose();
            m_Backend = null;
            if (m_Bezel != null)
                Destroy(m_Bezel);
            if (m_BezelMaterial != null)
                Destroy(m_BezelMaterial);
            if (m_BezelTexture != null)
                Destroy(m_BezelTexture);
            m_Bezel = null;
            m_BezelMaterial = null;
            m_BezelTexture = null;
        }

        // A frame a little larger than the screen, a breath behind it, so the page reads as a
        // screen on a dark slab from any angle.
        void MakeBezel()
        {
            m_Bezel = GameObject.CreatePrimitive(PrimitiveType.Quad);
            m_Bezel.name = "Bezel";
            m_Bezel.transform.SetParent(transform, false);
            m_Bezel.transform.localPosition = new Vector3(0f, 0f, -0.006f);
            m_Bezel.transform.localScale = new Vector3(m_Size.x + 0.14f, m_Size.y + 0.14f, 1f);
            Destroy(m_Bezel.GetComponent<Collider>());
            m_BezelTexture = new Texture2D(1, 1) { name = "Theater Bezel" };
            m_BezelTexture.SetPixel(0, 0, new Color(0.02f, 0.02f, 0.03f));
            m_BezelTexture.Apply();
            m_BezelMaterial = new Material(UnlitShader()) { name = "Theater Bezel" };
            m_BezelMaterial.mainTexture = m_BezelTexture;
            m_Bezel.GetComponent<MeshRenderer>().sharedMaterial = m_BezelMaterial;
        }

        // The first unlit shader the build has: the render pipeline's own, then the built-in ones.
        // A 1x1 texture carries the color, so no shader's color property is needed by name.
        public static Shader UnlitShader()
        {
            foreach (var name in new[]
                     {
                         "Universal Render Pipeline/Unlit",
                         "Unlit/Color",
                         "Sprites/Default",
                         "Standard",
                     })
            {
                var shader = Shader.Find(name);
                if (shader != null)
                    return shader;
            }
            return Shader.Find("Standard");
        }
    }
}
