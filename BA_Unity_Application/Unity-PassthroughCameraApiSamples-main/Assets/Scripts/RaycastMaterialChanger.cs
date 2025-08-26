using UnityEngine;
using System.Collections;
using OVR; // OVRInputを使用するために必要

public class RaycastMaterialChanger : MonoBehaviour {
    public float maxRayDistance = 10f; // レイキャストの最大距離
    public LayerMask targetLayer; // レイキャストの対象とするレイヤー
    public OVRInput.Button activateButton = OVRInput.Button.Two; // レイキャストとMaterial変更をアクティベートするボタン
    public Material highlightMaterial; // Materialを変更する際に使用する新しいMaterial

    private Material originalMaterial; // 元のMaterialを保持する変数
    private Renderer lastHitRenderer; // 最後にヒットしたオブジェクトのRenderer

    void Update() {
        // 特定のボタンが押されているか確認
        if (OVRInput.Get(activateButton)) {
            PerformRaycastAndChangeMaterial();
        } else {
            // ボタンが離されたら、以前変更したMaterialを元に戻す
            ResetMaterial();
        }
    }

    void PerformRaycastAndChangeMaterial() {
        RaycastHit hit;
        // コントローラーの位置から前方へレイキャスト
        if (Physics.Raycast(transform.position, transform.forward, out hit, maxRayDistance, targetLayer)) {
            Renderer hitRenderer = hit.collider.GetComponent<Renderer>();
            if (hitRenderer != null && hitRenderer != lastHitRenderer) {
                // 以前にヒットしたオブジェクトのMaterialを元に戻す
                ResetMaterial();

                // 新しいオブジェクトにヒットした場合
                lastHitRenderer = hitRenderer;
                originalMaterial = hitRenderer.material; // 元のMaterialを保存
                hitRenderer.material = highlightMaterial; // Materialを変更
                Debug.Log("Hit object: " + hit.collider.name + " and changed material.");
            }
        } else {
            // 何もヒットしなかった場合、以前変更したMaterialを元に戻す
            ResetMaterial();
        }
    }

    void ResetMaterial() {
        if (lastHitRenderer != null && originalMaterial != null) {
            lastHitRenderer.material = originalMaterial;
            Debug.Log("Reset material for: " + lastHitRenderer.name);
            lastHitRenderer = null; // リセット後、参照をクリア
            originalMaterial = null; // リセット後、参照をクリア
        }
    }
}