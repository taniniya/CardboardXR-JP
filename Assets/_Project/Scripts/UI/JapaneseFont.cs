using TMPro;
using UnityEngine;
using UnityEngine.TextCore.LowLevel;

namespace CardboardXR.UI
{
    // Puts a Japanese font under every TextMesh Pro text in the app. TMP's default font (LiberationSans)
    // and the Material UI's Roboto both lack kana and kanji, so without a fallback the Japanese text
    // would draw as boxes. The font is M PLUS 1p (SIL Open Font License 1.1), a dynamic font asset made
    // from Assets/_Project/UI/Resources/MPLUS1p-Regular.ttf: it starts with an empty atlas and adds each
    // glyph the first time it's used, across several atlas textures, so the whole of Japanese is
    // available without a multi-megabyte baked atlas.
    //   ProjectBootstrap.JapaneseFontSetup registers the same font in TMP Settings before a build, so a
    //   build starts with it already there. This runs before the first scene, so it also covers a build
    //   made without that step (the font then comes from Resources), and a font asset that turned out
    //   unusable: it checks the fonts already registered really do have kana and kanji first.
    public static class JapaneseFont
    {
        // In Assets/_Project/UI/Resources, so the font is in the build even when nothing references it.
        public const string k_ResourcePath = "MPLUS1p-Regular";
        public const string k_FontAssetName = "MPLUS1p Regular SDF";
        // Kana and kanji both, so a Latin-only font that answers for one can't pass.
        const string k_Probe = "あア漢字";

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        static void Install() => Ensure();

        // Adds the Japanese font as a TMP fallback unless one that has Japanese is already there.
        public static void Ensure()
        {
            if (HasJapaneseFallback())
                return;
            var font = Resources.Load<Font>(k_ResourcePath);
            if (font == null)
            {
                Debug.LogError($"[Font] No {k_ResourcePath}.ttf in Resources, so Japanese text has no font");
                return;
            }
            var asset = Create(font);
            var fallbacks = TMP_Settings.fallbackFontAssets;
            if (fallbacks == null)
            {
                Debug.LogWarning("[Font] No TMP Settings, so the Japanese font has nowhere to go");
                return;
            }
            if (fallbacks.Contains(asset))
                return;
            fallbacks.Insert(0, asset);
            Debug.Log($"[Font] Japanese fallback font {asset.name} added from {k_ResourcePath}.ttf");
        }

        // A dynamic SDF font asset from the font: an empty 1024 square atlas to start with, filled as
        // glyphs are used, and more atlases as they fill (Japanese needs several).
        public static TMP_FontAsset Create(Font font)
        {
            var asset = TMP_FontAsset.CreateFontAsset(font, 64, 8, GlyphRenderMode.SDFAA, 1024, 1024,
                AtlasPopulationMode.Dynamic, enableMultiAtlasSupport: true);
            asset.name = k_FontAssetName;
            // The same shader the packed font assets use, which is certainly in the build.
            var shader = Shader.Find("TextMeshPro/Mobile/Distance Field");
            if (shader != null && asset.material != null)
                asset.material.shader = shader;
            return asset;
        }

        // Whether TMP Settings' own font or one of its fallbacks has kana and kanji, text the
        // Latin-only fonts can't draw.
        public static bool HasJapaneseFallback()
        {
            if (TMP_Settings.defaultFontAsset != null && SupportsJapanese(TMP_Settings.defaultFontAsset))
                return true;
            var fallbacks = TMP_Settings.fallbackFontAssets;
            if (fallbacks == null)
                return false;
            foreach (var fallback in fallbacks)
            {
                if (SupportsJapanese(fallback))
                    return true;
            }
            return false;
        }

        // Whether the font asset has these glyphs, adding them to a dynamic one first. A static atlas
        // font returns false for the ones it doesn't have. (The build's own check uses this too.)
        public static bool SupportsJapanese(TMP_FontAsset asset)
        {
            if (asset == null)
                return false;
            try
            {
                return asset.TryAddCharacters(k_Probe, out var missing) && string.IsNullOrEmpty(missing);
            }
            catch (System.Exception e)
            {
                Debug.LogWarning($"[Font] Couldn't check {asset.name} for Japanese: {e.Message}");
                return false;
            }
        }
    }
}
