using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
[RequireComponent(typeof(Renderer))]
public class BottleLiquid : MonoBehaviour {
    /// <summary>
    /// BottleLiquid�V�F�[�_�֘A�̃v���p�e�BID��`
    /// </summary>
    private static class ShaderPropertyId {
        public static readonly int BottleLiquidWaveCenter = Shader.PropertyToID("_WaveCenter");
        public static readonly int BottleLiquidWaveParams = Shader.PropertyToID("_WaveParams");
        public static readonly int BottleLiquidColorForward = Shader.PropertyToID("_LiquidColorForward");
        public static readonly int BottleLiquidColorBack = Shader.PropertyToID("_LiquidColorBack");
    }

    /// <summary>�t�ʃJ���[�I�t�Z�b�g�l</summary>
    private static readonly Color LiquidColorTopOffset = new Color(0.15f, 0.15f, 0.15f, 0.0f);

    /// <summary>�t�̃J���[</summary>
    [SerializeField] private Color liquidColor;

    /// <summary>�r�`��̊T�v��\���I�t�Z�b�g�|�C���g���X�g</summary>
    [SerializeField] private Vector3[] bottleSizeOffsetPoints;

    /// <summary>�[�U��</summary>
    [Range(0.0f, 1.0f)][SerializeField] private float fillingRate = 0.5f;

    /// <summary>�ʒu�����ɂ�铮���̉e����</summary>
    [Range(0.0f, 2.0f)][SerializeField] private float positionInfluenceRate = 0.7f;

    /// <summary>��]�����ɂ�铮���̉e����</summary>
    [Range(0.0f, 2.0f)][SerializeField] private float rotationInfluenceRate = 0.4f;

    /// <summary>�g�̑傫���̌�����</summary>
    [Range(0.0f, 1.0f)][SerializeField] private float sizeAttenuationRate = 0.92f;

    /// <summary>�g�̎����̌�����</summary>
    [Range(0.0f, 1.0f)][SerializeField] private float cycleAttenuationRate = 0.97f;

    /// <summary>���Ԃɂ��ʑ��ω��W��</summary>
    [SerializeField] private float cycleOffsetCoef = 12.0f;

    /// <summary>�����ɂ��ω��ʍő�i�g�̑傫���j</summary>
    [SerializeField] private float deltaSizeMax = 0.15f;

    /// <summary>�����ɂ��ω��ʍő�i�g�̎����j</summary>
    [SerializeField] private float deltaCycleMax = 10.0f;

    /// <summary>����ΏۂƂȂ�}�e���A��</summary>
    private Material[] targetMaterials;

    /// <summary>�O��Q�ƈʒu</summary>
    private Vector3 prevPosition;

    /// <summary>�O��Q�ƃI�C���[�p�x</summary>
    private Vector3 prevEulerAngles;

    /// <summary>���݂̉t�̔g�p�����[�^</summary>
    private Vector4 waveCurrentParams;

    /// <summary>
    /// �J�n������
    /// </summary>
    private void Start() {
        Renderer targetRenderer = GetComponent<Renderer>();
        if (targetRenderer == null) {
            return;
        }

        if (targetMaterials == null || targetMaterials.Length <= 0) {
            List<Material> targetMaterialList = new List<Material>();
            for (int index = 0; index < targetRenderer.sharedMaterials.Length; index++) {
                Material material = targetRenderer.sharedMaterials[index];
                if (material.shader.name.Contains("BottleLiquid")) {
                    targetMaterialList.Add(material);
                }
            }

            targetMaterials = targetMaterialList.ToArray();
        }

        waveCurrentParams = Vector4.zero;

        BackupTransform();
    }

    /// <summary>
    /// �X�V����
    /// </summary>
    private void Update() {
        if (targetMaterials == null || targetMaterials.Length <= 0) {
            return;
        }

        CalculateWaveParams();
        SetupMaterials();

        BackupTransform();
    }

    /// <summary>
    /// �g�p�����[�^�Z�o
    /// </summary>
    private void CalculateWaveParams() {
        // waveParams�ɂ��̂܂܃x�N�g�����Z
        // x�͐U���Ay�͎���
        Vector4 attenuationRateVec = new Vector4(sizeAttenuationRate, cycleAttenuationRate, 0.0f, 0.0f);
        Vector4 deltaMaxVec = new Vector4(deltaSizeMax, deltaCycleMax, 0.0f, 0.0f);

        // ��������
        waveCurrentParams = Vector4.Scale(waveCurrentParams, attenuationRateVec);

        // �ʒu�Ɖ�]�̍�������ω��l�Y�o
        Transform thisTransform = transform;
        Vector3 currentRotation = thisTransform.eulerAngles;
        Vector3 diffPos = thisTransform.position - prevPosition;
        Vector3 diffRot = new Vector3(
            Mathf.DeltaAngle(currentRotation.x, prevEulerAngles.x),
            Mathf.DeltaAngle(currentRotation.y, prevEulerAngles.y),
            Mathf.DeltaAngle(currentRotation.z, prevEulerAngles.z));

        waveCurrentParams += deltaMaxVec * (diffPos.magnitude * positionInfluenceRate);
        waveCurrentParams += deltaMaxVec * (diffRot.magnitude * rotationInfluenceRate * 0.01f); // �I�C���[�p�����͌��̒l���傫���̂ŕ␳

        waveCurrentParams = Vector4.Min(waveCurrentParams, deltaMaxVec);

        // ���Ԃɂ��ʑ��ω��͌����ΏۊO
        waveCurrentParams.z = cycleOffsetCoef;
    }

    /// <summary>
    /// �g�̒��S�ʒu�Z�o
    /// </summary>
    private Vector4 CalculateWaveCenter() {
        // xz�̓I�u�W�F�N�g�̒��S�i���[���h���W�n�j
        // y�͕r�`��Ə[�U������t�ʂ̍�����ݒ�i���[���h���W�n�j
        (float min, float max) liquidSurfaceHeight = GetLiquidSurfaceHeight();
        return transform.position +
               Vector3.up * Mathf.Lerp(liquidSurfaceHeight.min, liquidSurfaceHeight.max, fillingRate);
        //Vector3.up* Mathf.Lerp(liquidSurfaceHeight.min, liquidSurfaceHeight.max, 0.8f);
    }

    /// <summary>
    /// �}�e���A���ݒ�
    /// </summary>
    private void SetupMaterials() {
        Vector4 waveCenter = CalculateWaveCenter();

        for (int index = 0; index < targetMaterials.Length; index++) {
            Material material = targetMaterials[index];
            material.SetVector(ShaderPropertyId.BottleLiquidWaveCenter, waveCenter);
            material.SetVector(ShaderPropertyId.BottleLiquidWaveParams, waveCurrentParams);
            material.SetColor(ShaderPropertyId.BottleLiquidColorForward, liquidColor);
            material.SetColor(ShaderPropertyId.BottleLiquidColorBack, liquidColor + LiquidColorTopOffset);
        }
    }

    /// <summary>
    /// �p�����̕ۑ�
    /// </summary>
    private void BackupTransform() {
        prevPosition = transform.position;
        prevEulerAngles = transform.eulerAngles;
    }

    /// <summary>
    /// �I�u�W�F�N�g���[�J���ɂ�����t�ʂ̍����i�ŏ�/�ő�j���擾
    /// </summary>
    private (float min, float max) GetLiquidSurfaceHeight() {
        if (bottleSizeOffsetPoints == null || bottleSizeOffsetPoints.Length <= 0) {
            return (0.0f, 0.0f);
        }

        Transform thisTransform = transform;
        (float min, float max) ret = (float.MaxValue, float.MinValue);
        for (int index = 0; index < bottleSizeOffsetPoints.Length; index++) {
            Vector3 localPoint = thisTransform.TransformPoint(bottleSizeOffsetPoints[index]) - thisTransform.position;
            ret.min = Mathf.Min(ret.min, localPoint.y);
            ret.max = Mathf.Max(ret.max, localPoint.y);
        }

        return ret;
    }

    public void SetLiquidColor(Color color) {
        liquidColor = color;
        SetupMaterials();
    }

    public void SetFillingRate(float rate) {
        fillingRate = Mathf.Clamp01(rate);
        //SetupMaterials();
    }

# if UNITY_EDITOR
    /// <summary>
    /// �I�����̃M�Y���\��
    /// </summary>
    private void OnDrawGizmosSelected() {
        if (bottleSizeOffsetPoints == null || bottleSizeOffsetPoints.Length <= 0) {
            return;
        }

        // �r�`��I�t�Z�b�g�|�C���g�̕\��
        Gizmos.color = Color.yellow;
        for (int index = 0; index < bottleSizeOffsetPoints.Length; index++) {
            Vector3 point = bottleSizeOffsetPoints[index];
            Gizmos.DrawSphere(transform.TransformPoint(point), 0.05f);
        }
    }
# endif
}
