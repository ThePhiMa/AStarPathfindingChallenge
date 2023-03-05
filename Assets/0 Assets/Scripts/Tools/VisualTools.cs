using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sleep0.Tools
{
    public static class VisualTools
    {
        public static TextMesh CreateWorldText(string text, Transform parent, Vector3 localPosition, int fontSize, Color color, TextAnchor textAnchor, TextAlignment textAlignment, int sortingOrder)
        {
            GameObject go = new GameObject(string.Format("WorldText({0:00},{1:00}", localPosition.x, localPosition.y), typeof(TextMesh));
            Transform transform = go.transform;
            transform.SetParent(parent, false);
            transform.localPosition = localPosition;
            transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
            TextMesh textMesh = go.GetComponent<TextMesh>();
            textMesh.anchor = textAnchor;
            textMesh.alignment = textAlignment;
            textMesh.text = text;
            textMesh.fontSize = fontSize;
            textMesh.color = color;
            textMesh.GetComponent<MeshRenderer>().sortingOrder = sortingOrder;
            return textMesh;
        }

        public static TextMesh CreateWorldText(Transform parent, string text, Vector3 localPosition = default(Vector3), int fontSize = 40, Color? color = null, TextAnchor textAnchor = TextAnchor.MiddleCenter, TextAlignment textAlignment = TextAlignment.Center, int sortingOrder = 5000)
        {
            return CreateWorldText(text, parent, localPosition, fontSize, color == null ? Color.white : (Color)color, textAnchor, textAlignment, sortingOrder);
        }
    }
}