using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.UI;

public class PassthroughAPIHelper : MonoBehaviour {
    private static readonly string[] PERMISSIONS =
    {
        "android.permission.CAMERA",
        "horizonos.permission.HEADSET_CAMERA"
    };

    [SerializeField] private RawImage output;
    [SerializeField] private Text debugMessage;

    private bool hasPermission = false;

    private void Awake() {
        debugMessage.text = "権限の確認をして、なければリクエストを出します";
        hasPermission = PERMISSIONS.All(Permission.HasUserAuthorizedPermission);
        if (!hasPermission) {
            var grantedPermissionCount = 0;
            var callbacks = new PermissionCallbacks();
            callbacks.PermissionGranted += (permissionName) => {
                grantedPermissionCount++;
                hasPermission = grantedPermissionCount >= PERMISSIONS.Length;
            };
            Permission.RequestUserPermissions(PERMISSIONS, callbacks);
        }
    }

    void Start() {
        StartCoroutine(InitializeWebCamTexture());
    }

    private IEnumerator InitializeWebCamTexture() {
        debugMessage.text = "初期化を開始します";

#if !UNITY_6000_OR_NEWER
        debugMessage.text = "Unity2022のバグのため、WebCamTextureの初期化を1フレーム待ち中";
        yield return new WaitForEndOfFrame();
#endif

        while (!hasPermission) {
            debugMessage.text = "権限の付与を待っています";
            yield return new WaitForEndOfFrame();
        }

        debugMessage.text = "パススルーをWebCam経由で設定します";
        var devices = WebCamTexture.devices;
        var deviceName = devices[0].name;
        var webCamTexture = new WebCamTexture(deviceName, 1280, 960);
        webCamTexture.Play();
        output.texture = webCamTexture;

        debugMessage.text = "初期化完了です";
    }
}

