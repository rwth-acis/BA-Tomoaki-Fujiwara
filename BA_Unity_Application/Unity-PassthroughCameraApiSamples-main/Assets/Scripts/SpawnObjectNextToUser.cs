using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnObjectNextToUser : MonoBehaviour
{

    public GameObject objectToSpawn;
    public GameObject user;

    [Tooltip("ユーザーの位置からのオフセット。X:右, Y:上, Z:前")]
    public Vector3 positionOffset = new Vector3(0.5f, 0f, 0.5f);

    // Start is called before the first frame update
    void Start()
    {
        SetObjectPositionNextToUser();
    }

    /// <summary>
    /// 指定されたオブジェクトをユーザーの隣に配置します。
    /// </summary>
    public void SetObjectPositionNextToUser()
    {
        // user と objectToSpawn がインスペクターで設定されているか確認します
        if (user == null)
        {
            Debug.LogError("Userオブジェクトが設定されていません。", this);
            return;
        }

        if (objectToSpawn == null)
        {
            Debug.LogError("対象のオブジェクト(objectToSpawn)が設定されていません。", this);
            return;
        }

        // ユーザーの位置と向きを基準に、配置したい位置を計算します


        // objectToSpawnの位置を計算された位置に設定します
        objectToSpawn.transform.position = user.transform.position;
        Debug.Log("Test");

    }
}
