using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hyperreal
{
    public class BoxGizmo : MonoBehaviour
    {
        [SerializeField]
        Color defaultMainColor = Color.clear;
        [SerializeField]
        Color defaultWireframeColor = Color.clear;

        [Space()]
        [SerializeField]
        Color selectedMainColor = new Color(0.0f, 1.0f, 0.0f, 0.25f);
        [SerializeField]
        Color selectedWireframeColor = Color.green;

        void OnDrawGizmos()
        {
            DrawGizmoCube(defaultMainColor, defaultWireframeColor);
        }

        void OnDrawGizmosSelected()
        {
            DrawGizmoCube(selectedMainColor, selectedWireframeColor);
        }

        void DrawGizmoCube(Color mainColor, Color wireframeColor)
        {
            Gizmos.matrix = transform.localToWorldMatrix;

            Vector3 position = Vector3.zero;
            Vector3 size = Vector3.one;

            if (mainColor != Color.clear)
            {
                Gizmos.color = mainColor;
                Gizmos.DrawCube(position, size);
            }

            if (wireframeColor != Color.clear)
            {
                Gizmos.color = wireframeColor;
                Gizmos.DrawWireCube(position, size);
            }
        }
    }
}
