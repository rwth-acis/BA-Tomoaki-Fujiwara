using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectObject : MonoBehaviour
{

    public GameObject testChangeColorObject;


    public bool isSelected = false;

    // Start is called before the first frame update
    void Start()
    {
        //HitLaserpointer();
    }

    // Update is called once per frame


    public void HitLaserpointer()
    {
        isSelected = !isSelected;
        if (isSelected)
        {
            testChangeColorObject.layer = LayerMask.NameToLayer("Selected");
            EnableEmissionOnAllMaterials();
        }
        else
        {
            testChangeColorObject.layer = LayerMask.NameToLayer("Selectable");
            DisableEmissionOnAllMaterials();
        }
    }


    /// <summary>
    /// Finds all materials on the target object and its children, then enables their emission.
    /// </summary>
    public void EnableEmissionOnAllMaterials()
    {
        if (testChangeColorObject == null)
        {
            Debug.LogError("testChangeColorObjectが設定されていません。");
            return;
        }

        // 親オブジェクトとすべての子オブジェクトからRendererコンポーネントを取得します
        Renderer[] renderers = testChangeColorObject.GetComponentsInChildren<Renderer>();
        if (renderers.Length == 0)
        {
            Debug.LogError("testChangeColorObjectとその子オブジェクトにRendererコンポーネントが見つかりません。");
            return;
        }

        // 見つかったすべてのRendererをループ処理します
        foreach (Renderer objectRenderer in renderers)
        {
            // オブジェクトに設定されているすべてのマテリアルをループ処理します
            foreach (Material mat in objectRenderer.materials)
            {
                // マテリアル名でのチェックを削除し、すべてのマテリアルを対象とします
                // Emissionを有効にする
                mat.EnableKeyword("_EMISSION");
                
                // 0-255スケールの25を、0-1スケールに変換
                float valueToAdd = 25.0f / 255.0f;

                Color newColor = new Color(
                    valueToAdd,
                    valueToAdd,
                    valueToAdd,
                    1
                );
                // 新しい発光色を設定
                mat.SetColor("_EmissionColor", newColor);

                Debug.Log($"オブジェクト '{objectRenderer.gameObject.name}' のマテリアル '{mat.name}' のEmissionを有効にしました。");
            }
        }
    }

    /// <summary>
    /// Finds all materials on the target object and its children, then disables their emission.
    /// </summary>
    public void DisableEmissionOnAllMaterials()
    {
        if (testChangeColorObject == null)
        {
            Debug.LogError("testChangeColorObjectが設定されていません。");
            return;
        }

        // 親オブジェクトとすべての子オブジェクトからRendererコンポーネントを取得します
        Renderer[] renderers = testChangeColorObject.GetComponentsInChildren<Renderer>();
        if (renderers.Length == 0)
        {
            Debug.LogError("testChangeColorObjectとその子オブジェクトにRendererコンポーネントが見つかりません。");
            return;
        }

        // 見つかったすべてのRendererをループ処理します
        foreach (Renderer objectRenderer in renderers)
        {
            // オブジェクトに設定されているすべてのマテリアルをループ処理します
            foreach (Material mat in objectRenderer.materials)
            {
                // マテリアル名でのチェックを削除し、すべてのマテリアルを対象とします
                // Emissionを無効にする
                mat.DisableKeyword("_EMISSION");

                Debug.Log($"オブジェクト '{objectRenderer.gameObject.name}' のマテリアル '{mat.name}' のEmissionを無効にしました。");
            }
        }
    }

}
