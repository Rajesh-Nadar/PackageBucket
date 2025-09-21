using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UnityTools.UIExtension
{
    [AddComponentMenu("UI/Rounded Image")]
    public class RoundedImage : Image
    {
        [Header("Rounded Rectangle")]
        [Tooltip("Corner radius in pixels")]
        [Range(0f, 60f)]
        public float cornerRadius = 20f;

        [Tooltip("Number of segments to approximate each quarter circle")]
        [Range(1, 40)]
        public int cornerSegments = 6;

        protected override void OnValidate()
        {
            base.OnValidate();
            cornerSegments = Mathf.Max(1, cornerSegments);
            SetVerticesDirty();
        }

        protected override void OnPopulateMesh(VertexHelper vh)
        {
            vh.Clear();

            Rect rect = GetPixelAdjustedRect();
            float w = rect.width;
            float h = rect.height;

            float r = Mathf.Clamp(cornerRadius, 0f, Mathf.Min(w, h) * 0.5f);

            if (r <= 0.001f)
            {
                AddQuad(vh, rect);
                return;
            }

            float xMin = rect.xMin;
            float xMax = rect.xMax;
            float yMin = rect.yMin;
            float yMax = rect.yMax;

            // corner centers
            Vector2 cTL = new Vector2(xMin + r, yMax - r);
            Vector2 cTR = new Vector2(xMax - r, yMax - r);
            Vector2 cBR = new Vector2(xMax - r, yMin + r);
            Vector2 cBL = new Vector2(xMin + r, yMin + r);

            // perimeter
            List<Vector2> path = new List<Vector2>();
            // Top edge (left → right)
            path.Add(new Vector2(xMin + r, yMax));
            path.Add(new Vector2(xMax - r, yMax));
            AddArc(path, cTR, 90f, 0f, r, cornerSegments);   // top-right
            path.Add(new Vector2(xMax, yMin + r));
            AddArc(path, cBR, 0f, -90f, r, cornerSegments);  // bottom-right
            path.Add(new Vector2(xMin + r, yMin));
            AddArc(path, cBL, -90f, -180f, r, cornerSegments); // bottom-left
            path.Add(new Vector2(xMin, yMax - r));
            AddArc(path, cTL, -180f, -270f, r, cornerSegments); // top-left

            // remove dupes
            RemoveDuplicateConsecutive(path, 0.0001f);

            // center vertex
            Vector2 center = new Vector2((xMin + xMax) * 0.5f, (yMin + yMax) * 0.5f);
            int centerIndex = 0;
            vh.AddVert(MakeVertex(center, rect));

            // perimeter verts
            for (int i = 0; i < path.Count; i++)
            {
                vh.AddVert(MakeVertex(path[i], rect));
            }

            // triangles (fan)
            for (int i = 0; i < path.Count; i++)
            {
                int next = (i + 1) % path.Count;
                vh.AddTriangle(centerIndex, i + 1, next + 1);
            }
        }

        private UIVertex MakeVertex(Vector2 p, Rect rect)
        {
            UIVertex v = UIVertex.simpleVert;
            v.position = p;
            v.color = color;
            v.uv0 = new Vector2(
                (p.x - rect.xMin) / Mathf.Max(0.0001f, rect.width),
                (p.y - rect.yMin) / Mathf.Max(0.0001f, rect.height)
            );
            return v;
        }

        private void AddQuad(VertexHelper vh, Rect rect)
        {
            vh.AddVert(MakeVertex(new Vector2(rect.xMin, rect.yMin), rect));
            vh.AddVert(MakeVertex(new Vector2(rect.xMin, rect.yMax), rect));
            vh.AddVert(MakeVertex(new Vector2(rect.xMax, rect.yMax), rect));
            vh.AddVert(MakeVertex(new Vector2(rect.xMax, rect.yMin), rect));

            vh.AddTriangle(0, 1, 2);
            vh.AddTriangle(2, 3, 0);
        }

        private void AddArc(List<Vector2> path, Vector2 center, float startDeg, float endDeg, float radius, int segments)
        {
            float total = endDeg - startDeg;
            for (int i = 0; i <= segments; i++)
            {
                float t = i / (float) segments;
                float angle = Mathf.Deg2Rad * (startDeg + total * t);
                Vector2 p = center + new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * radius;
                path.Add(p);
            }
        }

        private void RemoveDuplicateConsecutive(List<Vector2> list, float eps)
        {
            if (list.Count < 2) return;
            for (int i = list.Count - 1; i > 0; i--)
            {
                if ((list[i] - list[i - 1]).sqrMagnitude <= eps * eps)
                    list.RemoveAt(i);
            }
            if (list.Count > 1 && (list[0] - list[^1]).sqrMagnitude <= eps * eps)
                list.RemoveAt(list.Count - 1);
        }
    }
}
