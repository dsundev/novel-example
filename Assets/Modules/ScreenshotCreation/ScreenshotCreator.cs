using UnityEngine;

namespace Modules.ScreenshotCreation
{
    public class ScreenshotCreator
    {
        public byte[] CreateScreenshotBytes(Camera camera, int width = 1024, int height = 576)
        {
            var renderTexture = new RenderTexture(width, height, 24);
            var texture = new Texture2D(width, height, TextureFormat.RGB24, false);

            camera.targetTexture = renderTexture;
            RenderTexture.active = renderTexture;
            camera.Render();

            texture.ReadPixels(new Rect(0, 0, width, height), 0, 0);
            texture.Apply();

            camera.targetTexture = null;
            RenderTexture.active = null;
            Object.Destroy(renderTexture);

            var bytes = texture.EncodeToPNG();
            Object.Destroy(texture);
            return bytes;
        }
    }
}