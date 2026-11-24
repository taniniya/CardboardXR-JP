using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace CardboardXR.Demos.Theater
{
    // Passes a gaze click on the screen to whoever listens, with where on the screen it landed in
    // texture coordinates: the head's ray through the screen's collider gives the point, and the
    // point in the collider's own space gives the coordinates. Modeled on the video demo's
    // ClickRelay, which the gaze's event system reaches the same way. TheaterScreen turns Taking
    // on only while the relay is the one taking the screen's clicks.
    public class TheaterClickRelay : MonoBehaviour, IPointerClickHandler
    {
        public bool Taking;

        public event Action<Vector2> ClickedAt;

        public void OnPointerClick(PointerEventData eventData)
        {
            if (!Taking)
                return;
            var core = CoreServices.Instance;
            var box = GetComponent<BoxCollider>();
            if (core == null || box == null)
                return;
            var head = core.Head;
            var ray = new Ray(head.position, head.forward);
            if (!box.Raycast(ray, out var hit, 30f))
                return;
            var local = transform.InverseTransformPoint(hit.point);
            ClickedAt?.Invoke(TheaterMath.UvFromLocal(
                new Vector2(local.x, local.y), new Vector2(box.size.x, box.size.y)));
        }
    }
}
