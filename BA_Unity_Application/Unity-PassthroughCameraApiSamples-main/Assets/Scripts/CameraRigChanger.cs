using UnityEngine;
using Oculus.Platform; // Oculus Input���g�p���邽�߂ɕK�v

public class CameraRigChanger : MonoBehaviour {
    [Header("Camera Rigs")]
    [Tooltip("�����̔z�u���[�h�Ŏg�p����OVRCameraRig")]
    public GameObject setupCameraRig;
    [Tooltip("�������[�h�Ŏg�p����OVRCameraRig")]
    public GameObject cookingCameraRig;

    private bool isSetupModeActive = true; // ���݃A�N�e�B�u�ȃ��[�h�̃t���O

    [Header("Input Settings")]
    [Tooltip("�J�������O��؂�ւ��邽�߂̃R���g���[���[�{�^��")]
    public OVRInput.Button switchButton = OVRInput.Button.One; // Oculus�R���g���[���[��A�{�^�� (Right Touch) �܂��� X�{�^�� (Left Touch)

    // �I�v�V�����F�ǂ̃R���g���[���[����̓��͂��󂯕t���邩
    [Tooltip("�J�������O�؂�ւ��Ɏg���R���g���[���[ (��: RightHand, LeftHand, �܂��� None �ŗ���)")]
    public OVRInput.Controller inputController = OVRInput.Controller.All;

    void Start() {
        // ������Ԃ̐ݒ���m���ɂ���
        if (setupCameraRig != null) {
            setupCameraRig.SetActive(true);
        }
        if (cookingCameraRig != null) {
            cookingCameraRig.SetActive(false);
        }
        isSetupModeActive = true;

        if (setupCameraRig == null || cookingCameraRig == null) {
            Debug.LogError("CameraRigChanger: setupCameraRig �܂��� cookingCameraRig �����蓖�Ă��Ă��܂���B");
            enabled = false; // �X�N���v�g�𖳌��ɂ���
        }
    }

    void Update() {
        // �w�肳�ꂽ�{�^���������ꂽ�u�Ԃ����o
        if (OVRInput.GetDown(switchButton, inputController)) {
            ToggleCameraRigs();
        }
    }

    /// <summary>
    /// ���݃A�N�e�B�u��CameraRig��؂�ւ��܂��B
    /// </summary>
    public void ToggleCameraRigs() {
        if (setupCameraRig == null || cookingCameraRig == null) {
            Debug.LogError("CameraRigChanger: CameraRigs���������ݒ肳��Ă��Ȃ����ߐ؂�ւ��ł��܂���B");
            return;
        }

        if (isSetupModeActive) {
            // �z�u���[�h -> �������[�h�֐؂�ւ�
            Debug.Log("CameraRigChanger: �������[�h�ɐ؂�ւ��܂��B");
            setupCameraRig.SetActive(false);
            cookingCameraRig.SetActive(true);
        } else {
            // �������[�h -> �z�u���[�h�֐؂�ւ�
            Debug.Log("CameraRigChanger: �z�u���[�h�ɐ؂�ւ��܂��B");
            setupCameraRig.SetActive(true);
            cookingCameraRig.SetActive(false);
        }
        isSetupModeActive = !isSetupModeActive; // �t���O�𔽓]
    }
}