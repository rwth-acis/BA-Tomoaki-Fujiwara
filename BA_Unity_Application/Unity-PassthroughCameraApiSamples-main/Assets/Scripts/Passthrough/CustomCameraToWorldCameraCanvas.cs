// Copyright (c) Meta Platforms, Inc. and affiliates.

using System.Collections;
using Meta.XR.Samples;
using UnityEngine;
using UnityEngine.UI;

namespace PassthroughCameraSamples.CameraToWorld
{
    [MetaCodeSample("PassthroughCameraApiSamples-CameraToWorld")]
    public class CustomCameraToWorldCameraCanvas : MonoBehaviour
    {
        [SerializeField] private WebCamTextureManager m_webCamTextureManager;
        [SerializeField] private Text m_debugText;
        //[SerializeField] private RawImage m_image; // RawImage も追加して表示を確認

        [SerializeField] private RenderTexture m_renderTexture; // private に変更
        private Texture2D m_cameraSnapshot;
        private Color32[] m_pixelsBuffer;

        // RenderTexture を初期化し、WebCamTextureから画像をコピーする
        private void InitializeRenderTexture()
        {
            var webCamTexture = m_webCamTextureManager.WebCamTexture;
            if (webCamTexture == null)
            {
                Debug.LogError("WebCamTexture is null during RenderTexture initialization.");
                return;
            }

            /*
            // 既存のRenderTextureを破棄（もしあれば）
            if (m_renderTexture != null)
            {
                m_renderTexture.Release();
                Destroy(m_renderTexture);
            }
            */
            // WebCamTexture と同じサイズとフォーマットでRenderTextureを新規作成
            //m_renderTexture = new RenderTexture(webCamTexture.width, webCamTexture.height, 0, RenderTextureFormat.ARGB32);
            //m_renderTexture.Create();

            /*
            // RawImage に RenderTexture を設定して表示
            if (m_image != null)
            {
                m_image.texture = m_renderTexture;
            }*/
        }

        public void MakeCameraSnapshot()
        {
            var webCamTexture = m_webCamTextureManager.WebCamTexture;
            if (webCamTexture == null || !webCamTexture.isPlaying)
            {
                m_debugText.text = "WebCamTexture not ready or not playing.";
                return;
            }

            /*
            // RenderTexture が初期化されていなければ初期化
            if (m_renderTexture == null || !m_renderTexture.IsCreated() ||
                m_renderTexture.width != webCamTexture.width ||
                m_renderTexture.height != webCamTexture.height)
            {
                InitializeRenderTexture();
            }*/

            // WebCamTexture を RenderTexture にコピー
            Graphics.Blit(webCamTexture, m_renderTexture);

            // Texture2D へのスナップショットは必要に応じて実行
            if (m_cameraSnapshot == null ||
                m_cameraSnapshot.width != webCamTexture.width ||
                m_cameraSnapshot.height != webCamTexture.height)
            {
                m_cameraSnapshot = new Texture2D(webCamTexture.width, webCamTexture.height, TextureFormat.RGBA32, false);
            }

            m_pixelsBuffer ??= new Color32[webCamTexture.width * webCamTexture.height];
            webCamTexture.GetPixels32(m_pixelsBuffer); // webCamTexture を直接使う
            m_cameraSnapshot.SetPixels32(m_pixelsBuffer);
            m_cameraSnapshot.Apply();

            m_debugText.text = "Camera Snapshot and RenderTexture updated.";
        }

        // カメラからのストリーミングを再開し、RenderTextureを更新する
        public void ResumeStreamingFromCamera()
        {
            MakeCameraSnapshot(); // 実際にはカメラ画像をRenderTextureに常に反映させたい場合に呼び出す
        }

        private IEnumerator Start()
        {
            // WebCamTexture が利用可能になるまで待機
            while (m_webCamTextureManager.WebCamTexture == null)
            {
                yield return null;
            }

            m_debugText.text = "WebCamTexture Object ready and playing.";

            // WebCamTexture の準備ができた後に RenderTexture を初期化
            InitializeRenderTexture();

            // 必要に応じてカメラ画像をRenderTextureにコピー開始
            // MakeCameraSnapshot() を毎フレーム呼び出すか、イベントドリブンで呼び出すか検討
            // ここでは初回のみ呼び出し
            Graphics.Blit(m_webCamTextureManager.WebCamTexture, m_renderTexture);
        }

        private void Update()
        {
            if (PassthroughCameraPermissions.HasCameraPermission != true)
            {
                m_debugText.text = "No permission granted.";
            }

            // Update WebCamTexture and RenderTexture every frame
            MakeCameraSnapshot();
        }
    }
}