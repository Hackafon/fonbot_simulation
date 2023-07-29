using System;
using System.Collections;
using System.Collections.Generic;
using ROS2;
using std_msgs.msg;
using UnityEngine;

public class MicrophoneData : MonoBehaviour, ISensorData
{
    private AudioClip _micClip;
    private int _lastPosition = 0;
    
    private ROS2UnityComponent _ros2Unity;
    private IPublisher<std_msgs.msg.Float32MultiArray> _driverPub;
    [SerializeField] private Topic _topic;

    void Start()
    {
        _micClip = Microphone.Start(null, true, 10, 44100);
        
        _ros2Unity = GameObject.FindGameObjectWithTag("ROS2Manager").GetComponent<ROS2UnityComponent>();
    }

    private void Update()
    {
        PublishToTopic();
    }

    float[] GetMicData()
    {
        float[] _data = new float[_micClip.samples * _micClip.channels];
        var _currentPosition = Microphone.GetPosition(null);
        _micClip.GetData(_data, 0);
        
        if (_currentPosition > _lastPosition)
        {
            float[] _samples = new float[_currentPosition - _lastPosition];
            for (int i = _lastPosition; i < _currentPosition; i++)
            {
                _samples[i - _lastPosition] = _data[i];
            }

            _lastPosition = _currentPosition;

            return _samples;
        }
        else
        {
            float[] _samples = new float[_micClip.samples * _micClip.channels - _lastPosition + _currentPosition];
            for (int i = _lastPosition; i < _micClip.samples * _micClip.channels; i++)
            {
                _samples[i - _lastPosition] = _data[i];
            }

            for (int i = 0; i < _currentPosition; i++)
            {
                _samples[i + _micClip.samples * _micClip.channels - _lastPosition] = _data[i];
            }

            _lastPosition = _currentPosition;

            return _samples;
        }
    }
    
    public void PublishToTopic()
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

        _driverPub = SensorManager.Instance.ros2Node.CreatePublisher<std_msgs.msg.Float32MultiArray>(_topic.topicName);

        std_msgs.msg.Float32MultiArray _samples = new Float32MultiArray();
        float[] _micData = GetMicData();
        
        //our float array will have following form: [frequency, number of channels, samples length, samples data...]
        _samples.Data = new float[_micData.Length + 3];
        _samples.Data[0] = _micClip.frequency;
        _samples.Data[1] = _micClip.channels;
        _samples.Data[2] = _micData.Length;
        for (int i = 3; i < _samples.Data.Length; i++)
        {
            _samples.Data[i] = _micData[i - 3];
        }

        _driverPub.Publish(_samples);
        Debug.Log($"Published to topic {_topic.topicName}");
    }
    
}