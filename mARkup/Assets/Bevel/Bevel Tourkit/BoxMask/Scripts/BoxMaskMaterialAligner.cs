using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hyperreal
{
    /// <summary>
    /// This class is used to set the mask properties on a target object that uses the StandardBoxMask shader.
    /// Mask values are derived from the world position, rotation and scale of the transform values.
    /// </summary>
    [ExecuteInEditMode]
    public class BoxMaskMaterialAligner : MonoBehaviour
    {
        [SerializeField]
        GameObject targetGameObject;

        [SerializeField]
        bool isUsingLocalSpace;

        [SerializeField]
        bool isAligningOnStart;

        const string boxMaskOpaqueName = "Hyperreal/StandardBoxMaskOpaque";
        const string boxMaskTransparentName = "Hyperreal/StandardBoxMaskTransparent";
        const string offsetInLocalSpaceName = "_OffsetInLocalSpace";
        const string offsetPostionName = "_OffsetPosition";
        const string offsetScaleName = "_OffsetScale";
        const string normalizedRotationAxisName = "_NormalizedRotationAxis";
        const string pivotPointName = "_PivotPoint";
        const string rotationAngleName = "_RotationAngle";

        int offsetInLocalSpaceId;
        int offsetPositionId;
        int offsetScaleId;
        int normalizedRotationAxisId;
        //int pivotPointId; //(doesn't seem to be used. But wait to delete until understood
        int rotationAnlgeId;

        public GameObject TargetGameObject
        {
            get
            {
                return targetGameObject;
            }

            set
            {
                targetGameObject = value;
            }
        }

        void Awake()
        {
            Init();
        }

        void OnDisable()
        {
            StopAligning();
        }

        void Start()
        {
            AlignOnStart();
        }

        void Update()
        {
            EditorUpdate();
        }

        void Init()
        {
            offsetInLocalSpaceId = Shader.PropertyToID(offsetInLocalSpaceName);
            offsetPositionId = Shader.PropertyToID(offsetPostionName);
            offsetScaleId = Shader.PropertyToID(offsetScaleName);
            normalizedRotationAxisId = Shader.PropertyToID(normalizedRotationAxisName);
            //pivotPointId = Shader.PropertyToID(pivotPointName); //(doesn't seem to be used. But wait to delete until understood
            rotationAnlgeId = Shader.PropertyToID(rotationAngleName);
        }

        void AlignOnStart()
        {
            if (Application.isEditor && Application.isPlaying == false) return;

            if (isAligningOnStart)
            {
                StartAligning();
            }
        }

        void EditorUpdate()
        {
            if (Application.isEditor && Application.isPlaying == false && transform.hasChanged)
            {
                Align();
            }
        }

        [ContextMenu("Start Aligning")]
        public void StartAligning()
        {
            StopAligning();
            StartCoroutine(AlignCoroutine());
        }

        [ContextMenu("Stop Aligning")]
        public void StopAligning()
        {
            StopAllCoroutines();
        }

        IEnumerator AlignCoroutine()
        {
            while (isActiveAndEnabled)
            {
                Align();
                yield return null;
            }
        }

        [ContextMenu("Align Once")]
        public void Align()
        {
            if (targetGameObject == null) return;

            HashSet<Material> sharedMaterials = new HashSet<Material>();

            Renderer[] renderers = targetGameObject.GetComponentsInChildren<Renderer>();
            foreach (Renderer renderer in renderers)
            {
                foreach (Material material in renderer.sharedMaterials)
                {
                    sharedMaterials.Add(material);
                }
            }

            foreach (Material material in sharedMaterials)
            {
                AlignMaterial(material);
            }
        }

        void AlignMaterial(Material material)
        {
            bool isBoxClipShaderMaterial = GetIsBoxClipShader(material);
            if (isBoxClipShaderMaterial == false) return;

            SetSpace(material);

            if (isUsingLocalSpace)
            {
                SetPositionLocally(material);
                SetRotationLocally(material);
                SetScaleLocally(material);
            }
            else
            {
                SetPosition(material);
                SetRotation(material);
                SetScale(material);
            }
        }

        bool GetIsBoxClipShader(Material material)
        {
            return (material != null && (material.shader.name == boxMaskOpaqueName || material.shader.name == boxMaskTransparentName));
        }

        void SetSpace(Material material)
        {
            float offsetInLocalSpace = isUsingLocalSpace ? 1.0f : 0.0f;
            material.SetFloat(offsetInLocalSpaceId, offsetInLocalSpace);
        }

        void SetPosition(Material material)
        {
            material.SetVector(offsetPostionName, transform.position);
        }

        void SetPositionLocally(Material material)
        {
            Vector3 offsetPosition = transform.position - targetGameObject.transform.position;
            offsetPosition = Quaternion.Inverse(targetGameObject.transform.rotation) * offsetPosition;

            offsetPosition.x /= targetGameObject.transform.lossyScale.x;
            offsetPosition.y /= targetGameObject.transform.lossyScale.y;
            offsetPosition.z /= targetGameObject.transform.lossyScale.z;

            material.SetVector(offsetPositionId, offsetPosition);
        }

        void SetRotation(Material material)
        {
            Quaternion offsetRotation = Quaternion.Inverse(transform.rotation);
            SetRotationAxisAndAngle(offsetRotation, material);
        }

        void SetRotationLocally(Material material)
        {
            Quaternion offsetRotation = Quaternion.Inverse(transform.rotation) * targetGameObject.transform.rotation;
            SetRotationAxisAndAngle(offsetRotation, material);
        }

        void SetRotationAxisAndAngle(Quaternion offsetRotation, Material material)
        {
            float rotationAngle = 0.0f;
            Vector3 normalizedRotationAxis = Vector3.zero;
            offsetRotation.ToAngleAxis(out rotationAngle, out normalizedRotationAxis);
            rotationAngle = rotationAngle * Mathf.Deg2Rad;

            material.SetVector(normalizedRotationAxisId, normalizedRotationAxis);
            material.SetFloat(rotationAnlgeId, rotationAngle);
        }

        void SetScale(Material material)
        {
            material.SetVector(offsetScaleId, transform.lossyScale);
        }

        void SetScaleLocally(Material material)
        {
            Vector3 offsetScale = Vector3.one;
            offsetScale.x = transform.lossyScale.x / targetGameObject.transform.lossyScale.x;
            offsetScale.y = transform.lossyScale.y / targetGameObject.transform.lossyScale.y;
            offsetScale.z = transform.lossyScale.z / targetGameObject.transform.lossyScale.z;
            material.SetVector(offsetScaleId, offsetScale);
        }

    }
}

