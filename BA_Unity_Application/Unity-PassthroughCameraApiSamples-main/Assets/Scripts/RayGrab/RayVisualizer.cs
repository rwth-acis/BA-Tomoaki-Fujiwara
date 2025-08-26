using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayVisualizer : MonoBehaviour {
    // Line Renderer �R���|�[�l���g�ւ̎Q��
    [SerializeField]
    private LineRenderer lineRenderer;

    // Ray �̍ő咷
    [SerializeField]
    private float maxRayLength = 100f;

    // Ray �𔭎˂���R���g���[���[�̎�� (�E�܂��͍�)
    [SerializeField]
    private OVRInput.Controller controllerType = OVRInput.Controller.RTouch; // �f�t�H���g�͉E�R���g���[���[

    void Awake() {
        // Line Renderer ���A�^�b�`����Ă��邱�Ƃ��m�F
        if (lineRenderer == null) {
            lineRenderer = GetComponent<LineRenderer>();
            if (lineRenderer == null) {
                Debug.LogError("Line Renderer �R���|�[�l���g��������܂���B���̃X�N���v�g�Ɠ���GameObject��Line Renderer��ǉ����Ă��������B", this);
                enabled = false; // �X�N���v�g�𖳌��ɂ���
                return;
            }
        }

        // Line Renderer �̏����ݒ� (Optional: Unity�G�f�B�^�Őݒ�ς݂̏ꍇ�s�v)
        lineRenderer.positionCount = 2; // �n�_�ƏI�_��2�_
        lineRenderer.useWorldSpace = true; // ���[���h���W���g�p
    }

    void Update() {
        // �R���g���[���[�̈ʒu�ƌ������擾
        Vector3 controllerPosition = OVRInput.GetLocalControllerPosition(controllerType);
        Quaternion controllerRotation = OVRInput.GetLocalControllerRotation(controllerType);

        // Ray �̎n�_ (�R���g���[���[�̈ʒu)
        Vector3 rayOrigin = transform.position; // ���̃X�N���v�g���A�^�b�`����Ă���I�u�W�F�N�g�̈ʒu

        // Ray �̕��� (�R���g���[���[�̃t�H���[�h����)
        // OVRInput.GetLocalControllerRotation �̓��[�J����]��Ԃ����߁A
        // �O���[�o���ȃt�H���[�h�����𓾂�ɂ� transform.forward �𗘗p���邩�A
        // controllerRotation * Vector3.forward �� transform.TransformDirection �ŕϊ�����K�v������܂��B
        // �����ł́A�X�N���v�g���R���g���[���[�ɃA�^�b�`����Ă���O���transform.forward���g�p���܂��B
        Vector3 rayDirection = transform.forward;

        // Raycast �����s
        RaycastHit hit;
        Vector3 rayEndPoint;

        if (Physics.Raycast(rayOrigin, rayDirection, out hit, maxRayLength)) {
            // �����ɓ��������ꍇ�A���������_�܂�Ray��`��
            rayEndPoint = hit.point;

            // Debug.Log ���g����Ray�����������I�u�W�F�N�g�̖��O��\�� (Optional)
            // Debug.Log($"Ray Hit: {hit.collider.name}");
        } else {
            // ���ɂ�������Ȃ������ꍇ�A�ő咷�܂�Ray��`��
            rayEndPoint = rayOrigin + rayDirection * maxRayLength;
        }

        // Line Renderer �̓_���X�V
        lineRenderer.SetPosition(0, rayOrigin);    // �n�_
        lineRenderer.SetPosition(1, rayEndPoint);  // �I�_
    }
}