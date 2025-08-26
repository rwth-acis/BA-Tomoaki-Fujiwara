using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayVisualizer : MonoBehaviour {
    // Line Renderer コンポーネントへの参照
    [SerializeField]
    private LineRenderer lineRenderer;

    // Ray の最大長
    [SerializeField]
    private float maxRayLength = 100f;

    // Ray を発射するコントローラーの種類 (右または左)
    [SerializeField]
    private OVRInput.Controller controllerType = OVRInput.Controller.RTouch; // デフォルトは右コントローラー

    void Awake() {
        // Line Renderer がアタッチされていることを確認
        if (lineRenderer == null) {
            lineRenderer = GetComponent<LineRenderer>();
            if (lineRenderer == null) {
                Debug.LogError("Line Renderer コンポーネントが見つかりません。このスクリプトと同じGameObjectにLine Rendererを追加してください。", this);
                enabled = false; // スクリプトを無効にする
                return;
            }
        }

        // Line Renderer の初期設定 (Optional: Unityエディタで設定済みの場合不要)
        lineRenderer.positionCount = 2; // 始点と終点の2点
        lineRenderer.useWorldSpace = true; // ワールド座標を使用
    }

    void Update() {
        // コントローラーの位置と向きを取得
        Vector3 controllerPosition = OVRInput.GetLocalControllerPosition(controllerType);
        Quaternion controllerRotation = OVRInput.GetLocalControllerRotation(controllerType);

        // Ray の始点 (コントローラーの位置)
        Vector3 rayOrigin = transform.position; // このスクリプトがアタッチされているオブジェクトの位置

        // Ray の方向 (コントローラーのフォワード方向)
        // OVRInput.GetLocalControllerRotation はローカル回転を返すため、
        // グローバルなフォワード方向を得るには transform.forward を利用するか、
        // controllerRotation * Vector3.forward を transform.TransformDirection で変換する必要があります。
        // ここでは、スクリプトがコントローラーにアタッチされている前提でtransform.forwardを使用します。
        Vector3 rayDirection = transform.forward;

        // Raycast を実行
        RaycastHit hit;
        Vector3 rayEndPoint;

        if (Physics.Raycast(rayOrigin, rayDirection, out hit, maxRayLength)) {
            // 何かに当たった場合、当たった点までRayを描画
            rayEndPoint = hit.point;

            // Debug.Log を使ってRayが当たったオブジェクトの名前を表示 (Optional)
            // Debug.Log($"Ray Hit: {hit.collider.name}");
        } else {
            // 何にも当たらなかった場合、最大長までRayを描画
            rayEndPoint = rayOrigin + rayDirection * maxRayLength;
        }

        // Line Renderer の点を更新
        lineRenderer.SetPosition(0, rayOrigin);    // 始点
        lineRenderer.SetPosition(1, rayEndPoint);  // 終点
    }
}