using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.UI;

public class PassthroughAPIHelper : MonoBehaviour
{
    private static readonly string[] PERMISSIONS =
    {
        "android.permission.CAMERA",
        "horizonos.permission.HEADSET_CAMERA"
    };

    [SerializeField] private RawImage output;
    [SerializeField] private Text debugMessage;

    private bool hasPermission = false;

    private void Awake()
    {
        debugMessage.text = "send request";
        hasPermission = PERMISSIONS.All(Permission.HasUserAuthorizedPermission);
        if (!hasPermission)
        {
            var grantedPermissionCount = 0;
            var callbacks = new PermissionCallbacks();
            callbacks.PermissionGranted += (permissionName) =>
            {
                grantedPermissionCount++;
                hasPermission = grantedPermissionCount >= PERMISSIONS.Length;
            };
            Permission.RequestUserPermissions(PERMISSIONS, callbacks);
        }
    }

    void Start()
    {
        StartCoroutine(InitializeWebCamTexture());
    }

    private IEnumerator InitializeWebCamTexture()
    {
        debugMessage.text = "init";

#if !UNITY_6000_OR_NEWER
        debugMessage.text = "waiting 1 frame";
        yield return new WaitForEndOfFrame();
#endif

        while (!hasPermission)
        {
            debugMessage.text = "waiting";
            yield return new WaitForEndOfFrame();
        }

        debugMessage.text = "WebCam";
        var devices = WebCamTexture.devices;
        var deviceName = devices[0].name;
        var webCamTexture = new WebCamTexture(deviceName, 1280, 960);
        webCamTexture.Play();
        output.texture = webCamTexture;

        debugMessage.text = "ready";
    }
}

