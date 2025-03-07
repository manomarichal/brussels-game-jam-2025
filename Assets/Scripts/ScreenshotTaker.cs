using System.Collections.Generic;
using UnityEngine;

public class ScreenshotTaker : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            TakeScreenshot();
        }
    }

    static void TakeScreenshot(int index = 0)
    {
        if (Application.isPlaying)
        {
            string path = "Assets/Screenshots";
            var time = System.DateTime.Now;
            string timestamp = $"{time.Hour}{time.Minute}{time.Second}";
            string fileName = $"{path}/screenshot_{index}-{timestamp}.png";

            ScreenCapture.CaptureScreenshot(fileName);

            Debug.Log("Took screenshot: " + fileName);
        }
    }
}
