using UnityEngine;

namespace CardboardXR.Demos.Theater
{
    // The theater's screen: a web page shown on a surface in the room. Two implementations: the
    // browser one (VuplexScreen, compiled only when the paid Vuplex plugin is in the project and
    // VUPLEX_PRESENT is defined) and the placeholder (PlaceholderScreen), which keeps the demo
    // building and opening without it. See the folder's README.
    public interface IWebScreen
    {
        // True when a real browser is behind the screen.
        bool Available { get; }

        // Makes the screen's picture under anchor, sizeMeters across and up. Called once.
        void Build(Transform anchor, Vector2 sizeMeters);

        // True sends clicks to the page through TheaterClickRelay and ClickAt instead of the
        // screen's own pointer handling. Set before Build.
        void SetRelayClicks(bool relay);

        void LoadUrl(string url);
        void GoBack();
        void GoForward();
        void Reload();

        // A click on the surface, in texture coordinates: (0, 0) is its bottom-left.
        void ClickAt(Vector2 uv);

        void Dispose();
    }
}
