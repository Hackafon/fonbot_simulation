using System;
using DG.Tweening;
using Fonbot.Common;
using Fonbot.Sensors;
using ROS2;
using UnityEngine;
using UnityEngine.UI;

namespace Fonbot.UI
{
    public class Minimap : MonoBehaviour
    {
        [SerializeField] private Transform _robotTransform;
        [SerializeField] private RectTransform _robotSymbolTransform;
        [SerializeField] private RectTransform _obstacleFrontTransform;
        [SerializeField] private Image _obstacleFrontImage;
        [SerializeField] private RectTransform _obstacleBackTransform;
        [SerializeField] private Image _obstacleBackImage;
        [SerializeField] private TMPro.TextMeshProUGUI _obstacleFrontDistanceText;
        [SerializeField] private TMPro.TextMeshProUGUI _obstacleBackDistanceText;

        //max distance on minimap
        [SerializeField] private float _maxMinimapDistance;
        [SerializeField] private Color _maxDistanceColor;
        [SerializeField] private Color _minDistanceColor;
        [SerializeField] private float _maxUltrasonicDistanceToShow;
        [SerializeField] private UltrasonicOptions _ultrasonicOptions;
        float _distanceFront;
        float _distanceBack;

        private ROS2UnityComponent _ros2Unity;
        private ROS2Node _ros2Node;
        [SerializeField] private Topic _ultrasonicFrontTopic;
        [SerializeField] private Topic _ultrasonicBackTopic;

        private void Start()
        {
            _distanceFront = _ultrasonicOptions.MaxDistance;
            _distanceBack = _ultrasonicOptions.MaxDistance;

            _ros2Unity = GameObject.FindGameObjectWithTag("ROS2Manager").GetComponent<ROS2UnityComponent>();

            GetObstacleDistances();
            PulseAnimation();
        }

        private void PulseAnimation()
        {
            DOTween.Sequence()
                .Append(_obstacleFrontTransform.DOScale(new Vector2(1.1f, 1.1f), 0.3f).SetDelay(0.3f))
                .Append(_obstacleFrontTransform.DOScale(Vector2.one, 0.3f))
                .SetLoops(-1, LoopType.Restart);

            DOTween.Sequence()
                .Append(_obstacleBackTransform.DOScale(new Vector2(1.1f, 1.1f), 0.3f).SetDelay(0.3f))
                .Append(_obstacleBackTransform.DOScale(Vector2.one, 0.3f))
                .SetLoops(-1, LoopType.Restart);
        }

        private void GetObstacleDistances()
        {
            if (_ros2Node == null && _ros2Unity.Ok())
            {
                _ros2Node = _ros2Unity.CreateNode("FonbotMinimap");

                _ = _ros2Node.CreateSubscription<std_msgs.msg.Float32>(
                    _ultrasonicFrontTopic.topicName, msg => _distanceFront = msg.Data);
                _ = _ros2Node.CreateSubscription<std_msgs.msg.Float32>(
                    _ultrasonicBackTopic.topicName, msg => _distanceBack = msg.Data);
            }
        }

        private void Update()
        {
            UpdateMinimap();
        }

        private void UpdateMinimap()
        {
            float _zAngle = 180 - _robotTransform.eulerAngles.y;
            _robotSymbolTransform.eulerAngles = new Vector3(0, 0, _zAngle);

            if (Math.Abs(_distanceFront - _ultrasonicOptions.MaxDistance) > Mathf.Epsilon)
            {
                _obstacleFrontTransform.gameObject.SetActive(true);
                Vector2 _frontPosition = PolarToCartesian(_zAngle - 90,
                    _distanceFront > _maxUltrasonicDistanceToShow ? _maxUltrasonicDistanceToShow : _distanceFront,
                    _maxUltrasonicDistanceToShow,
                    _maxMinimapDistance, -1);
                _obstacleFrontTransform.anchoredPosition = _frontPosition;
                _obstacleFrontDistanceText.SetText($"{Math.Round(_distanceFront, 2)}m");
                _obstacleFrontImage.color = Color.Lerp(_minDistanceColor, _maxDistanceColor,
                    _distanceFront > _maxUltrasonicDistanceToShow ? 1 : _distanceFront / _maxUltrasonicDistanceToShow);
            }
            else
            {
                _obstacleFrontTransform.gameObject.SetActive(false);
            }

            if (Math.Abs(_distanceBack - _ultrasonicOptions.MaxDistance) > Mathf.Epsilon)
            {
                _obstacleBackTransform.gameObject.SetActive(true);
                Vector2 _backPosition = PolarToCartesian(_zAngle - 90,
                    _distanceBack > _maxUltrasonicDistanceToShow ? _maxUltrasonicDistanceToShow : _distanceBack,
                    _maxUltrasonicDistanceToShow,
                    _maxMinimapDistance, 1);
                _obstacleBackTransform.anchoredPosition = _backPosition;
                _obstacleBackDistanceText.SetText($"{Math.Round(_distanceBack, 2)}m");
                _obstacleBackImage.color = Color.Lerp(_minDistanceColor, _maxDistanceColor,
                    _distanceBack > _maxUltrasonicDistanceToShow ? 1 : _distanceBack / _maxUltrasonicDistanceToShow);
            }
            else
            {
                _obstacleBackTransform.gameObject.SetActive(false);
            }
        }

        private Vector2 PolarToCartesian(float zAngle, float distance, float maxDistance, float rebasedMaxDistance,
            int sign)
        {
            float _rebasedDistance = distance / maxDistance * rebasedMaxDistance * sign;
            return new Vector2(_rebasedDistance * Mathf.Cos(zAngle * Mathf.Deg2Rad),
                _rebasedDistance * Mathf.Sin(zAngle * Mathf.Deg2Rad));
        }
    }
}