using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasOrientationHandler : MonoBehaviour
{
    [SerializeField]
    private Canvas _canvas;
    private CanvasScaler _canvasScaler;

    void Start()
    {
        _canvasScaler = _canvas.GetComponent<CanvasScaler>();
    }

    void Update()
    {
        if (Input.deviceOrientation == DeviceOrientation.LandscapeLeft || Input.deviceOrientation == DeviceOrientation.LandscapeRight)
        {
            _canvasScaler.referenceResolution = new Vector2(1280, 720);
        } 
        else
        {
            _canvasScaler.referenceResolution = new Vector2(720, 1280);
        }
    }
}
