using System.Linq;
using Fonbot.Common;
using ROS2;
using std_msgs.msg;
using UnityEngine;
using UnityEngine.Serialization;

namespace Fonbot.Sensors
{
    public class MicrophoneData : MonoBehaviour, ISensorData
    {
        [SerializeField] private AudioSource _audioSource;
        private AudioClip _micClip;
        private int _lastPosition = 0;
        private float[] _data;
        private float[] _spectrumData;

        private ROS2UnityComponent _ros2Unity;
        private IPublisher<Float32MultiArray> _samplesDriverPub;
        private IPublisher<Float32> _spectrumDriverPub;

        [FormerlySerializedAs("_topic")] [SerializeField] private Topic _samplesTopic;
        [SerializeField] private Topic _spectrumTopic;

        void Start()
        {
            Application.RequestUserAuthorization(UserAuthorization.Microphone);
            _micClip = Microphone.Start(null, true, 10, 44100);
            _data = new float[_micClip.samples * _micClip.channels];
            _spectrumData = new float[64];
            _audioSource.clip = _micClip;

            _ros2Unity = GameObject.FindGameObjectWithTag("ROS2Manager").GetComponent<ROS2UnityComponent>();
        }

        private void Update()
        {
            PublishToTopic();
        }
        
        public float GetSpectrumData()
        {
            _audioSource.GetSpectrumData(_spectrumData, 0, FFTWindow.BlackmanHarris);
            float _avgSample = 0;
            foreach (var sample in _spectrumData)
            {
                _avgSample += sample;
            }

            return _avgSample / _spectrumData.Length;
        }

        float[] GetMicData()
        {
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

            _samplesDriverPub =
                SensorManager.Instance.ros2Node.CreatePublisher<Float32MultiArray>(_samplesTopic.topicName);

            Float32MultiArray _samples = new Float32MultiArray();
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

            _samplesDriverPub.Publish(_samples);
            Debug.Log($"Published to topic {_samplesTopic.topicName}");

            _spectrumDriverPub = SensorManager.Instance.ros2Node.CreatePublisher<Float32>(_spectrumTopic.topicName);
            Float32 _spectrumAvg = new Float32();    
            _spectrumAvg.Data = GetSpectrumData();
            _spectrumDriverPub.Publish(_spectrumAvg);
            Debug.Log($"Published to topic {_spectrumTopic.topicName}");
        }

        
    }
}