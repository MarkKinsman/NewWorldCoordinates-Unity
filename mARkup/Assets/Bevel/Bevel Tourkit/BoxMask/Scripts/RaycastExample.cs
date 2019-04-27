using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hyperreal
{
    /// <summary>
    /// The key to doing a proper raycast against masked material objects is to first
    /// do a raycast with a LayerMask that ignores a box mask layer and then to
    /// do a second verification raycast that ignores everything except for the box mask layer.
    /// </summary>
    public class RaycastExample : MonoBehaviour
    {
        [SerializeField]
        LayerMask mainLayerMask;

        [SerializeField]
        LayerMask verificationLayerMask;

        Color hitColor = Color.green;
        Color noHitColor = Color.red;

        float initialAngleY;
        const float turnAngleY = 45.0f;

        const float maxRaycastDistance = 10.0f;

        void Start()
        {
            Init();
        }

        void Update()
        {
            Raycast();
            Turn();
        }

        void Init()
        {
            initialAngleY = transform.eulerAngles.y;
        }

        void Raycast()
        {
            bool isVerifiedHit = false;

            RaycastHit hit;
            bool isHit = Physics.Raycast(transform.position, transform.forward, out hit, maxRaycastDistance, mainLayerMask);
            if (isHit)
            {
                isVerifiedHit = GetIsVerifiedRaycast();
            }

            Vector3 startPoint = transform.position;
            Vector3 endPoint = isHit ? hit.point : startPoint + transform.forward * maxRaycastDistance;
            Color color = isVerifiedHit ? hitColor : noHitColor;

            Debug.DrawLine(startPoint, endPoint, color);
        }

        bool GetIsVerifiedRaycast()
        {
            RaycastHit hit;
            return Physics.Raycast(transform.position, transform.forward, out hit, maxRaycastDistance, verificationLayerMask);
        }

        void Turn()
        {
            float anglePercentage = Mathf.Sin(Time.time);
            Vector3 eulerAngles = transform.eulerAngles;
            eulerAngles.y = initialAngleY + (anglePercentage * turnAngleY);
            transform.eulerAngles = eulerAngles;
        }
    }
}

