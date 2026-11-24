using UnityEngine;

namespace CardboardXR.Demos.Theater
{
    // The screen without the browser plugin: a dark slab in the real screen's place, so the demo
    // still builds, opens and answers its panel. Loads and clicks are only logged; the panel says
    // why there is no page. See the folder's README for how to turn the browser on.
    public class PlaceholderScreen : IWebScreen
    {
        GameObject m_Slab;
        Material m_Material;
        Texture2D m_Texture;

        public bool Available => false;

        public void Build(Transform anchor, Vector2 sizeMeters)
        {
            m_Slab = GameObject.CreatePrimitive(PrimitiveType.Quad);
            m_Slab.name = "Placeholder Screen";
            m_Slab.transform.SetParent(anchor, false);
            m_Slab.transform.localScale = new Vector3(sizeMeters.x, sizeMeters.y, 1f);
            // The gaze stops on the anchor's own collider, not on this slab.
            Object.Destroy(m_Slab.GetComponent<Collider>());
            m_Texture = new Texture2D(1, 1) { name = "Theater Placeholder" };
            m_Texture.SetPixel(0, 0, new Color(0.05f, 0.05f, 0.09f));
            m_Texture.Apply();
            m_Material = new Material(TheaterScreen.UnlitShader()) { name = "Theater Placeholder" };
            m_Material.mainTexture = m_Texture;
            m_Slab.GetComponent<MeshRenderer>().sharedMaterial = m_Material;
        }

        public void SetRelayClicks(bool relay) { }

        public void LoadUrl(string url) => Debug.Log($"[Theater] Placeholder screen: would load {url}");

        public void GoBack() => Debug.Log("[Theater] Placeholder screen: would go back");

        public void GoForward() => Debug.Log("[Theater] Placeholder screen: would go forward");

        public void Reload() => Debug.Log("[Theater] Placeholder screen: would reload");

        public void ClickAt(Vector2 uv) => Debug.Log($"[Theater] Placeholder screen: clicked at {uv}");

        public void Dispose()
        {
            if (m_Slab != null)
                Object.Destroy(m_Slab);
            if (m_Material != null)
                Object.Destroy(m_Material);
            if (m_Texture != null)
                Object.Destroy(m_Texture);
            m_Slab = null;
            m_Material = null;
            m_Texture = null;
        }
    }
}
