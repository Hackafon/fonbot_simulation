using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraData : MonoBehaviour
{
    [SerializeField] private RawImage _cameraOutputImage;
    private void Start()
    {
        WebCamTexture _webCamTexture = new WebCamTexture();
        _cameraOutputImage.texture = _webCamTexture;
        _webCamTexture.Play();
    }
}
