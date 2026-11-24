using CardboardXR.Demos;
using CardboardXR.Demos.Video;
using UnityEditor;
using UnityEngine;

namespace ProjectBootstrap
{
    // 360 Video: a sphere to draw on, the player and its panel (built at runtime).
    class VideoSetup : DemoSetup
    {
        public override string Name => "Video";
        public override int Order => 20;

        public override DemoCatalog.Entry Entry => new()
        {
            Id = "video",
            Title = "360度動画",
            Description = "360度動画とVR180動画を、2Dでも3Dでも周囲に映して見られます。動画をタップすると操作パネルが出ます。スマホにコピーした動画や、Google Driveで共有した動画を再生できます。",
            Tagline = "360度・VR180の動画を周囲に映す",
            Icon = "360",
            Tint = new Color(0.7f, 0.25f, 0.29f),
            Requirements = new DemoRequirements { SixDof = Need.Off, Passthrough = Need.Off, Hands = Need.Off },
        };

        public override void BuildScene(string folder)
        {
            // Double-sided in case the sphere is ever seen from outside, and white so the video's own
            // colors show.
            var material = StereoBuilder.CreateMaterial("Video Sphere", DemoAssets.Unlit, Color.white, null, 1f, folder);

            var sphere = new GameObject("Video Sphere").AddComponent<VideoSphere>();
            var serializedSphere = new SerializedObject(sphere);
            serializedSphere.FindProperty("m_Material").objectReferenceValue = material;
            serializedSphere.ApplyModifiedPropertiesWithoutUndo();

            var demo = new GameObject("Video Demo").AddComponent<VideoDemo>();
            var serializedDemo = new SerializedObject(demo);
            serializedDemo.FindProperty("m_Sphere").objectReferenceValue = sphere;
            serializedDemo.ApplyModifiedPropertiesWithoutUndo();
        }
    }
}
