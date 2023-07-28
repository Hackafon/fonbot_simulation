using System;
using System.Collections;
using System.Collections.Generic;
using ROS2;
using UnityEngine;
using UnityEngine.UI;

public class CameraData : MonoBehaviour
{
    [SerializeField] private RawImage _cameraOutputImage;
    private WebCamTexture _webCamTexture;

    private ROS2UnityComponent _ros2Unity;
    private IPublisher<sensor_msgs.msg.Image> _driverPub;
    [SerializeField] private Topic _topic;

    private void Start()
    {
        _webCamTexture = new WebCamTexture();
        _cameraOutputImage.texture = _webCamTexture;
        _webCamTexture.Play();

        _ros2Unity = GameObject.FindGameObjectWithTag("ROS2Manager").GetComponent<ROS2UnityComponent>();
    }

    private void Update()
    {
        PublishToTopic();
    }

    void PublishToTopic()
    {
        if (!_ros2Unity.Ok())
        {
            return;
        }

        if (SensorManager.Instance.ros2Node == null)
        {
            Debug.LogError("ROS2 node not created!");
            return;
        }

        _driverPub = SensorManager.Instance.ros2Node.CreatePublisher<sensor_msgs.msg.Image>(_topic.topicName);

        sensor_msgs.msg.Image _cameraImage = new sensor_msgs.msg.Image();

        //color array of webcam capture
        Color32[] _image = _webCamTexture.GetPixels32();
        //ros2 image array with size width*height*3 (3 because of 3 color channels)
        _cameraImage.Data = new byte[_cameraImage.Width * _cameraImage.Height * 3];
        //which second of recording does the capture correspond to
        _cameraImage.Header.Stamp.Sec = Mathf.RoundToInt(Time.timeSinceLevelLoad);
        //frame id of the capture
        _cameraImage.Header.Frame_id = $"Frame#{Time.frameCount}";
        //number of elements in a row
        _cameraImage.Step = 3 * _cameraImage.Width;
        //rgb8 encoding where each channel takes 1 byte
        _cameraImage.Encoding = "rgb8";
        _cameraImage.Width = (uint)_webCamTexture.width;
        _cameraImage.Height = (uint)_webCamTexture.height;

        for (int i = 0; i < _image.Length; i++)
        {
            _cameraImage.Data[3 * i] = _image[i].r;
            _cameraImage.Data[3 * i + 1] = _image[i].g;
            _cameraImage.Data[3 * i + 2] = _image[i].b;
        }

        _driverPub.Publish(_cameraImage);
        Debug.Log($"Published to topic {_topic.topicName}");
    }
}