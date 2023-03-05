using UnityEngine;

namespace Sleep0.Tools
{
    public class VectorTools : MonoBehaviour
    {
        public static Vector3 GetMouseWorldPosition(Camera relativeCamera, float zValue)
        {
            Vector2 mousePosition = Input.mousePosition;
            return relativeCamera.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, zValue));
        }
    }
}