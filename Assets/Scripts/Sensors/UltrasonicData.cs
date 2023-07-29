using ROS2;
using System.Collections.Generic;
using UnityEngine;

namespace Fonbot.Sensors
{
    public class UltrasonicData : MonoBehaviour, ISensorData
    {
        private List<RaycastHit> _hitInfo;
        [SerializeField] private UltrasonicOptions _options;
        [SerializeField] private List<Transform> _rayOrigins;

        private ROS2UnityComponent _ros2Unity;
        private IPublisher<std_msgs.msg.Float32> _driverPub;
        [SerializeField] private Topic _topic;

        private void Start()
        {
            _hitInfo = new List<RaycastHit>();
            for (int i = 0; i < _rayOrigins.Count; i++)
            {
                _hitInfo.Add(new RaycastHit());
            }

            _ros2Unity = GameObject.FindGameObjectWithTag("ROS2Manager").GetComponent<ROS2UnityComponent>();
        }

        void CastRays()
        {
            int _index = 0;
            foreach (Transform _origin in _rayOrigins)
            {
                bool _hitDetect = Physics.Raycast(_origin.position, _origin.forward, out var hit, _options.MaxDistance, _options.WhatToHit);
                _hitInfo[_index] = hit;

                _index++;

                if (_index >= _rayOrigins.Count)
                {
                    _index = 0;
                }
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

            _driverPub = SensorManager.Instance.ros2Node.CreatePublisher<std_msgs.msg.Float32>(_topic.topicName);

            float _minDist = _options.MaxDistance;
            foreach (RaycastHit _hit in _hitInfo)
            {
                if (_hit.collider != null && _hit.distance < _minDist)
                {
                    _minDist = _hit.distance;
                }
            }

            std_msgs.msg.Float32 _distance = new std_msgs.msg.Float32();
            _distance.Data = _minDist;
            _driverPub.Publish(_distance);
            Debug.Log($"Published to topic {_topic.topicName}: {_distance.Data}");
        }

        private void Update()
        {
            CastRays();
            PublishToTopic();
        }

        void OnDrawGizmos()
        {
            int _index = 0;
            foreach (Transform _origin in _rayOrigins)
            {
                Gizmos.color = Color.red;
                if (_origin != null)
                {
                    Gizmos.DrawRay(_origin.position, _origin.forward * _options.MaxDistance);
                }

                Gizmos.color = Color.green;

                if (_hitInfo != null && _hitInfo[_index].collider != null)
                {
                    Gizmos.DrawRay(_origin.position, _origin.forward * _hitInfo[_index].distance);
                }

                _index++;

                if (_index >= _rayOrigins.Count)
                {
                    _index = 0;
                }
            }
        }
    }
}