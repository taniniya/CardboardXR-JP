using CardboardXR.Demos;
using CardboardXR.Demos.Theater;
using UnityEditor;
using UnityEngine;

namespace ProjectBootstrap
{
    // Webシアター: its app library entry and its starter scene (see DemoSetup). The build adds the
    // entry and makes the scene once, then never overwrites either.
    class TheaterSetup : DemoSetup
    {
        public override string Name => "Theater";
        public override int Order => 60;

        public override DemoCatalog.Entry Entry => new()
        {
            Id = "theater",
            Title = "Webシアター",
            Description = "VRの中に浮かぶ大画面でYouTubeなどのWebページを見られます。画面はピンチでクリックでき、コントロールパネルからショートカットや戻る・進む・更新が使えます。",
            Tagline = "大画面でYouTubeやWebを見る",
            Icon = "movie",
            Tint = new Color(0.85f, 0.16f, 0.20f),
            Requirements = new DemoRequirements { SixDof = Need.Off, Passthrough = Need.Off, Hands = Need.Off },
        };

        public override void BuildScene(string folder)
        {
            // The screen: an anchor the demo places in front of the head at runtime. Its collider
            // (matching TheaterScreen's serialized size) is what the gaze clicks land on while the
            // relay is taking them; the relay beside it works out where they landed.
            var screenObject = new GameObject("Screen Anchor");
            var screen = screenObject.AddComponent<TheaterScreen>();
            var collider = screenObject.AddComponent<BoxCollider>();
            collider.size = new Vector3(3.2f, 1.8f, 0.1f);
            screenObject.AddComponent<TheaterClickRelay>();

            var demo = new GameObject("Theater Demo").AddComponent<TheaterDemo>();
            var serializedDemo = new SerializedObject(demo);
            serializedDemo.FindProperty("m_Screen").objectReferenceValue = screen;
            serializedDemo.ApplyModifiedPropertiesWithoutUndo();

            // What the demo shows in VR, which passthrough hides: a dark floor with a faint grid every
            // half meter, for a sense of space and scale.
            var environment = DemoAssets.Environment();
            var grid = StereoBuilder.SaveAsset(DemoAssets.CreateGridTexture(), $"{folder}/Grid.asset");
            const float floorSize = 30f;
            DemoAssets.Floor(environment.transform,
                StereoBuilder.CreateMaterial("Theater Floor", DemoAssets.Unlit, Color.white, grid, floorSize / 0.5f, folder), floorSize);
        }
    }
}
