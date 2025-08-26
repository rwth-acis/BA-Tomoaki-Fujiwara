using UnityEngine;
using Oculus.Platform; // Oculus Inputを使用するために必要

public class CameraRigChanger : MonoBehaviour {
    [Header("Camera Rigs")]
    [Tooltip("部屋の配置モードで使用するOVRCameraRig")]
    public GameObject setupCameraRig;
    [Tooltip("料理モードで使用するOVRCameraRig")]
    public GameObject cookingCameraRig;

    private bool isSetupModeActive = true; // 現在アクティブなモードのフラグ

    [Header("Input Settings")]
    [Tooltip("カメラリグを切り替えるためのコントローラーボタン")]
    public OVRInput.Button switchButton = OVRInput.Button.One; // OculusコントローラーのAボタン (Right Touch) または Xボタン (Left Touch)

    // オプション：どのコントローラーからの入力を受け付けるか
    [Tooltip("カメラリグ切り替えに使うコントローラー (例: RightHand, LeftHand, または None で両方)")]
    public OVRInput.Controller inputController = OVRInput.Controller.All;

    void Start() {
        // 初期状態の設定を確実にする
        if (setupCameraRig != null) {
            setupCameraRig.SetActive(true);
        }
        if (cookingCameraRig != null) {
            cookingCameraRig.SetActive(false);
        }
        isSetupModeActive = true;

        if (setupCameraRig == null || cookingCameraRig == null) {
            Debug.LogError("CameraRigChanger: setupCameraRig または cookingCameraRig が割り当てられていません。");
            enabled = false; // スクリプトを無効にする
        }
    }

    void Update() {
        // 指定されたボタンが押された瞬間を検出
        if (OVRInput.GetDown(switchButton, inputController)) {
            ToggleCameraRigs();
        }
    }

    /// <summary>
    /// 現在アクティブなCameraRigを切り替えます。
    /// </summary>
    public void ToggleCameraRigs() {
        if (setupCameraRig == null || cookingCameraRig == null) {
            Debug.LogError("CameraRigChanger: CameraRigsが正しく設定されていないため切り替えできません。");
            return;
        }

        if (isSetupModeActive) {
            // 配置モード -> 料理モードへ切り替え
            Debug.Log("CameraRigChanger: 料理モードに切り替えます。");
            setupCameraRig.SetActive(false);
            cookingCameraRig.SetActive(true);
        } else {
            // 料理モード -> 配置モードへ切り替え
            Debug.Log("CameraRigChanger: 配置モードに切り替えます。");
            setupCameraRig.SetActive(true);
            cookingCameraRig.SetActive(false);
        }
        isSetupModeActive = !isSetupModeActive; // フラグを反転
    }
}