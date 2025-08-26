using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
[RequireComponent(typeof(Renderer))]
public class BeatedEggLiquid : MonoBehaviour
{
    /// <summary>
    /// Property ID definitions for the BottleLiquid shader
    /// </summary>
    private static class ShaderPropertyId
    {
        public static readonly int BottleLiquidWaveCenter = Shader.PropertyToID("_WaveCenter");
        public static readonly int BottleLiquidWaveParams = Shader.PropertyToID("_WaveParams");
        public static readonly int BottleLiquidColorForward = Shader.PropertyToID("_LiquidColorForward");
        public static readonly int BottleLiquidColorBack = Shader.PropertyToID("_LiquidColorBack");
    }

    /// <summary>Liquid surface color offset value</summary>
    private static readonly Color LiquidColorTopOffset = new Color(0.15f, 0.15f, 0.15f, 0.0f);

    /// <summary>Liquid color</summary>
    [SerializeField] private Color liquidColor;

    /// <summary>List of offset points representing the bottle shape outline</summary>
    [SerializeField] private Vector3[] bottleSizeOffsetPoints;

    /// <summary>Filling rate</summary>
    [Range(0.0f, 1.0f)][SerializeField] private float fillingRate = 0.5f;

    /// <summary>Influence rate of movement by position difference</summary>
    [Range(0.0f, 2.0f)][SerializeField] private float positionInfluenceRate = 0.7f;

    /// <summary>Influence rate of movement by rotation difference</summary>
    [Range(0.0f, 2.0f)][SerializeField] private float rotationInfluenceRate = 0.4f;

    /// <summary>Attenuation rate of wave size</summary>
    [Range(0.0f, 1.0f)][SerializeField] private float sizeAttenuationRate = 0.92f;

    /// <summary>Attenuation rate of wave cycle</summary>
    [Range(0.0f, 1.0f)][SerializeField] private float cycleAttenuationRate = 0.97f;

    /// <summary>Phase change coefficient over time</summary>
    [SerializeField] private float cycleOffsetCoef = 12.0f;

    /// <summary>Maximum change amount by difference (wave size)</summary>
    [SerializeField] private float deltaSizeMax = 0.15f;

    /// <summary>Maximum change amount by difference (wave cycle)</summary>
    [SerializeField] private float deltaCycleMax = 10.0f;

    /// <summary>Materials to be controlled</summary>
    private Material[] targetMaterials;

    /// <summary>Previous reference position</summary>
    private Vector3 prevPosition;

    /// <summary>Previous reference Euler angles</summary>
    private Vector3 prevEulerAngles;

    /// <summary>Current liquid wave parameters</summary>
    private Vector4 waveCurrentParams;

    /// <summary>
    /// Start processing
    /// </summary>
    private void Start()
    {
        Renderer targetRenderer = GetComponent<Renderer>();
        if (targetRenderer == null)
        {
            return;
        }

        if (targetMaterials == null || targetMaterials.Length <= 0)
        {
            List<Material> targetMaterialList = new List<Material>();
            for (int index = 0; index < targetRenderer.sharedMaterials.Length; index++)
            {
                Material material = targetRenderer.sharedMaterials[index];
                if (material.shader.name.Contains("BottleLiquid"))
                {
                    targetMaterialList.Add(material);
                }
            }

            targetMaterials = targetMaterialList.ToArray();
        }

        waveCurrentParams = Vector4.zero;

        BackupTransform();
    }

    /// <summary>
    /// Update processing
    /// </summary>
    private void Update()
    {
        if (targetMaterials == null || targetMaterials.Length <= 0)
        {
            return;
        }

        CalculateWaveParams();
        SetupMaterials();

        BackupTransform();
    }

    /// <summary>
    /// Calculate wave parameters
    /// </summary>
    private void CalculateWaveParams()
    {
        // Vector operation directly on waveParams
        // x is amplitude, y is cycle
        Vector4 attenuationRateVec = new Vector4(sizeAttenuationRate, cycleAttenuationRate, 0.0f, 0.0f);
        Vector4 deltaMaxVec = new Vector4(deltaSizeMax, deltaCycleMax, 0.0f, 0.0f);

        // Attenuation process
        waveCurrentParams = Vector4.Scale(waveCurrentParams, attenuationRateVec);

        // Calculate change value from position and rotation differences
        Transform thisTransform = transform;
        Vector3 currentRotation = thisTransform.eulerAngles;
        Vector3 diffPos = thisTransform.position - prevPosition;
        Vector3 diffRot = new Vector3(
            Mathf.DeltaAngle(currentRotation.x, prevEulerAngles.x),
            Mathf.DeltaAngle(currentRotation.y, prevEulerAngles.y),
            Mathf.DeltaAngle(currentRotation.z, prevEulerAngles.z));

        waveCurrentParams += deltaMaxVec * (diffPos.magnitude * positionInfluenceRate);
        waveCurrentParams += deltaMaxVec * (diffRot.magnitude * rotationInfluenceRate * 0.01f); // Correct the Euler angle difference as its original value is large

        waveCurrentParams = Vector4.Min(waveCurrentParams, deltaMaxVec);

        // Phase change over time is not subject to attenuation
        waveCurrentParams.z = cycleOffsetCoef;
    }

    /// <summary>
    /// Calculate wave center position
    /// </summary>
    private Vector4 CalculateWaveCenter()
    {
        // xz is the center of the object (world coordinate system)
        // y sets the liquid surface height from the bottle shape and filling rate (world coordinate system)
        (float min, float max) liquidSurfaceHeight = GetLiquidSurfaceHeight();
        return transform.position +
               Vector3.up * Mathf.Lerp(liquidSurfaceHeight.min, liquidSurfaceHeight.max, fillingRate);
    }

    /// <summary>
    /// Set up materials
    /// </summary>
    private void SetupMaterials()
    {
        Vector4 waveCenter = CalculateWaveCenter();

        for (int index = 0; index < targetMaterials.Length; index++)
        {
            Material material = targetMaterials[index];
            material.SetVector(ShaderPropertyId.BottleLiquidWaveCenter, waveCenter);
            material.SetVector(ShaderPropertyId.BottleLiquidWaveParams, waveCurrentParams);
            material.SetColor(ShaderPropertyId.BottleLiquidColorForward, liquidColor);
            material.SetColor(ShaderPropertyId.BottleLiquidColorBack, liquidColor + LiquidColorTopOffset);
        }
    }

    /// <summary>
    /// Save transform information
    /// </summary>
    private void BackupTransform()
    {
        prevPosition = transform.position;
        prevEulerAngles = transform.eulerAngles;
    }

    /// <summary>
    /// Get the liquid surface height (min/max) in object local space
    /// </summary>
    private (float min, float max) GetLiquidSurfaceHeight()
    {
        if (bottleSizeOffsetPoints == null || bottleSizeOffsetPoints.Length <= 0)
        {
            return (0.0f, 0.0f);
        }

        Transform thisTransform = transform;
        (float min, float max) ret = (float.MaxValue, float.MinValue);
        for (int index = 0; index < bottleSizeOffsetPoints.Length; index++)
        {
            Vector3 localPoint = thisTransform.TransformPoint(bottleSizeOffsetPoints[index]) - thisTransform.position;
            ret.min = Mathf.Min(ret.min, localPoint.y);
            ret.max = Mathf.Max(ret.max, localPoint.y);
        }

        return ret;
    }

    public void SetFillingRate(float rate)
    {
        fillingRate = Mathf.Clamp01(rate);
        //SetupMaterials();
    }

# if UNITY_EDITOR
    /// <summary>
    /// Display gizmos when selected
    /// </summary>
    private void OnDrawGizmosSelected()
    {
        if (bottleSizeOffsetPoints == null || bottleSizeOffsetPoints.Length <= 0)
        {
            return;
        }

        // Display bottle shape offset points
        Gizmos.color = Color.yellow;
        for (int index = 0; index < bottleSizeOffsetPoints.Length; index++)
        {
            Vector3 point = bottleSizeOffsetPoints[index];
            Gizmos.DrawSphere(transform.TransformPoint(point), 0.05f);
        }
    }
# endif
}
