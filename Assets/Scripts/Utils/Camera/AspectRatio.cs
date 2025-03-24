using UnityEngine;

public class AspectRatio : MonoBehaviour
{
    public float targetAspect = 16f / 9f; // 16:9 aspect ratio

    private void Start()
    {
        // Get the current screen's aspect ratio
        float windowAspect = (float)Screen.width / Screen.height;

        // Calculate the scaling factor to match the target aspect
        float scaleHeight = windowAspect / targetAspect;

        Camera camera = Camera.main;

        if (scaleHeight < 1.0f)
        {
            // Add letterboxing (black bars on top and bottom)
            Rect rect = camera.rect;

            rect.width = 1.0f;
            rect.height = scaleHeight;
            rect.x = 0;
            rect.y = (1.0f - scaleHeight) / 2.0f;

            camera.rect = rect;
        }
        else
        {
            // Add pillarboxing (black bars on the sides)
            float scaleWidth = 1.0f / scaleHeight;

            Rect rect = camera.rect;

            rect.width = scaleWidth;
            rect.height = 1.0f;
            rect.x = (1.0f - scaleWidth) / 2.0f;
            rect.y = 0;

            camera.rect = rect;
        }
    }
}
