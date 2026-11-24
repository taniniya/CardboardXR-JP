using UnityEngine;

namespace CardboardXR.Demos.Theater
{
    // Coordinate conversions between the gaze and the web page. Pure, so the tests can pin them down.
    public static class TheaterMath
    {
        // Where a point on the screen's surface falls in texture coordinates: (0, 0) at the
        // surface's bottom-left, (1, 1) at its top-right. local is the point in the surface's own
        // space, whose middle is (0, 0); size is the surface's width and height.
        public static Vector2 UvFromLocal(Vector2 local, Vector2 size) =>
            new(local.x / size.x + 0.5f, local.y / size.y + 0.5f);

        // Texture coordinates have (0, 0) at the bottom-left, as textures are drawn. The web view's
        // Click takes a normalized point with (0, 0) at the page's top-left, as pages are laid out.
        // Across stays across; up and down swap.
        public static Vector2 ToWebPoint(Vector2 uv) =>
            new(Mathf.Clamp01(uv.x), 1f - Mathf.Clamp01(uv.y));
    }
}
