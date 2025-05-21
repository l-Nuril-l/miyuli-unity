using UnityEngine;

public class FPSCounter : MonoBehaviour
{
    public float updateInterval = 0.5f; // Update FPS every 0.5 seconds
    private float accum = 0; // FPS accumulated over the interval
    private int frames = 0; // Frames drawn over the interval
    private float timeleft; // Left time for current interval

    private GUIStyle style;

    private void Start()
    {
        timeleft = updateInterval;
        style = new GUIStyle();
        style.normal.textColor = Color.white;
        style.fontSize = 24;
    }

    private void OnGUI()
    {
        // Display FPS
        float fps = accum / frames;
        string fpsText = string.Format("{0:F2} FPS", fps);
        GUI.Label(new Rect(30, 35, 150, 100), fpsText, style);
    }

    private void Update()
    {
        timeleft -= Time.deltaTime;
        accum += Time.timeScale / Time.deltaTime;
        frames++;

        // Interval ended - reset variables for the next interval
        if (timeleft <= 0.0)
        {
            timeleft = updateInterval;
            accum = 0.0f;
            frames = 0;
        }
    }
}