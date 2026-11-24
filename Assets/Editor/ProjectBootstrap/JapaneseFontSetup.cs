using CardboardXR.UI;
using TMPro;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;

namespace ProjectBootstrap
{
    // Puts the app's Japanese font into the build as TextMesh Pro's fallback, so Japanese text is drawn
    // with it rather than as boxes. The font is M PLUS 1p (SIL Open Font License 1.1) at
    // Assets/_Project/UI/Resources/MPLUS1p-Regular.ttf. Here it becomes Assets/_Project/UI/MPLUS1p
    // Regular SDF.asset, a dynamic multi-atlas font asset (CardboardXR.UI.JapaneseFont.Create), which is
    // added to TMP Settings' fallback list; TMP Settings then references it, so it and the .ttf are in
    // the player. Runs before every build (JapaneseFontBuild) and as a step of the project's own build
    // (MaterialAssets); CardboardXR.UI.JapaneseFont covers a build without either.
    public static class JapaneseFontSetup
    {
        const string ResourcesFolder = "Assets/_Project/UI/Resources";
        const string FontPath = ResourcesFolder + "/" + JapaneseFont.k_ResourcePath + ".ttf";
        const string FontAssetPath = "Assets/_Project/UI/" + JapaneseFont.k_FontAssetName + ".asset";
        const string TmpSettingsPath = "Assets/TextMesh Pro/Resources/TMP Settings.asset";
        const string FallbacksProperty = "m_fallbackFontAssets";

        [MenuItem("CardboardXR/Make Japanese Font Fallback")]
        public static void EnsureFromMenu() => Ensure();

        // Makes the font asset if it isn't there and lists it in TMP Settings. True when the fallback is
        // in place. Never throws: a build without it still has the runtime font.
        public static bool Ensure()
        {
            try
            {
                return SetUp();
            }
            catch (System.Exception e)
            {
                Debug.LogWarning($"[Font] Couldn't set up the Japanese fallback font: {e}");
                return false;
            }
        }

        static bool SetUp()
        {
            var font = AssetDatabase.LoadAssetAtPath<Font>(FontPath);
            if (font == null)
            {
                Debug.LogError($"[Font] Missing {FontPath}, so the build would have no Japanese text");
                return false;
            }

            var asset = AssetDatabase.LoadAssetAtPath<TMP_FontAsset>(FontAssetPath);
            if (asset == null)
            {
                asset = MakeAsset(font);
                if (asset == null)
                    return false;
            }
            if (!JapaneseFont.SupportsJapanese(asset))
            {
                Debug.LogError($"[Font] {FontAssetPath} can't add kana and kanji, so it isn't made the fallback");
                return false;
            }
            return Register(asset);
        }

        // A dynamic SDF font asset saved with its atlas and material, as MaterialAssets makes the UI's
        // fonts.
        static TMP_FontAsset MakeAsset(Font font)
        {
            var created = JapaneseFont.Create(font);
            if (created == null)
            {
                Debug.LogError($"[Font] Couldn't make a font asset from {FontPath}");
                return null;
            }
            StereoBuilder.EnsureFolder("Assets/_Project/UI");
            AssetDatabase.CreateAsset(created, FontAssetPath);
            if (created.atlasTexture != null)
            {
                created.atlasTexture.name = $"{JapaneseFont.k_FontAssetName} Atlas";
                AssetDatabase.AddObjectToAsset(created.atlasTexture, created);
            }
            if (created.material != null)
            {
                created.material.name = $"{JapaneseFont.k_FontAssetName} Material";
                AssetDatabase.AddObjectToAsset(created.material, created);
            }
            EditorUtility.SetDirty(created);
            AssetDatabase.SaveAssets();
            if (created.atlasTexture == null || created.material == null)
            {
                Debug.LogError($"[Font] {FontAssetPath} was saved without its atlas or material");
                return null;
            }
            Debug.Log($"[Font] Made {FontAssetPath} from {FontPath}");
            return created;
        }

        // Adds the font asset to TMP Settings' fallback list, after the ones already there.
        static bool Register(TMP_FontAsset asset)
        {
            var settings = AssetDatabase.LoadAssetAtPath<TMP_Settings>(TmpSettingsPath);
            if (settings == null)
            {
                Debug.LogError($"[Font] Missing {TmpSettingsPath}, so the Japanese font can't be a fallback");
                return false;
            }
            var serialized = new SerializedObject(settings);
            var fallbacks = serialized.FindProperty(FallbacksProperty);
            if (fallbacks == null)
            {
                Debug.LogError($"[Font] {TmpSettingsPath} has no {FallbacksProperty}; the Japanese font can't be registered");
                return false;
            }
            for (var i = 0; i < fallbacks.arraySize; i++)
            {
                if (fallbacks.GetArrayElementAtIndex(i).objectReferenceValue == asset)
                    return true;
            }
            fallbacks.arraySize++;
            fallbacks.GetArrayElementAtIndex(fallbacks.arraySize - 1).objectReferenceValue = asset;
            serialized.ApplyModifiedPropertiesWithoutUndo();
            EditorUtility.SetDirty(settings);
            AssetDatabase.SaveAssets();
            Debug.Log($"[Font] {asset.name} is TMP Settings' fallback, so Japanese text is drawn in it");
            return true;
        }
    }

    // Every build, whether it goes through StereoBuilder or not.
    class JapaneseFontBuild : IPreprocessBuildWithReport
    {
        // Before TMP's own build processing, which reads TMP Settings.
        public int callbackOrder => -1000;

        public void OnPreprocessBuild(BuildReport report) => JapaneseFontSetup.Ensure();
    }
}
