using UnityEngine;
using System.Collections;
using OVR; // OVRInput���g�p���邽�߂ɕK�v

public class RaycastMaterialChanger : MonoBehaviour {
    public float maxRayDistance = 10f; // ���C�L���X�g�̍ő勗��
    public LayerMask targetLayer; // ���C�L���X�g�̑ΏۂƂ��郌�C���[
    public OVRInput.Button activateButton = OVRInput.Button.Two; // ���C�L���X�g��Material�ύX���A�N�e�B�x�[�g����{�^��
    public Material highlightMaterial; // Material��ύX����ۂɎg�p����V����Material

    private Material originalMaterial; // ����Material��ێ�����ϐ�
    private Renderer lastHitRenderer; // �Ō�Ƀq�b�g�����I�u�W�F�N�g��Renderer

    void Update() {
        // ����̃{�^����������Ă��邩�m�F
        if (OVRInput.Get(activateButton)) {
            PerformRaycastAndChangeMaterial();
        } else {
            // �{�^���������ꂽ��A�ȑO�ύX����Material�����ɖ߂�
            ResetMaterial();
        }
    }

    void PerformRaycastAndChangeMaterial() {
        RaycastHit hit;
        // �R���g���[���[�̈ʒu����O���փ��C�L���X�g
        if (Physics.Raycast(transform.position, transform.forward, out hit, maxRayDistance, targetLayer)) {
            Renderer hitRenderer = hit.collider.GetComponent<Renderer>();
            if (hitRenderer != null && hitRenderer != lastHitRenderer) {
                // �ȑO�Ƀq�b�g�����I�u�W�F�N�g��Material�����ɖ߂�
                ResetMaterial();

                // �V�����I�u�W�F�N�g�Ƀq�b�g�����ꍇ
                lastHitRenderer = hitRenderer;
                originalMaterial = hitRenderer.material; // ����Material��ۑ�
                hitRenderer.material = highlightMaterial; // Material��ύX
                Debug.Log("Hit object: " + hit.collider.name + " and changed material.");
            }
        } else {
            // �����q�b�g���Ȃ������ꍇ�A�ȑO�ύX����Material�����ɖ߂�
            ResetMaterial();
        }
    }

    void ResetMaterial() {
        if (lastHitRenderer != null && originalMaterial != null) {
            lastHitRenderer.material = originalMaterial;
            Debug.Log("Reset material for: " + lastHitRenderer.name);
            lastHitRenderer = null; // ���Z�b�g��A�Q�Ƃ��N���A
            originalMaterial = null; // ���Z�b�g��A�Q�Ƃ��N���A
        }
    }
}