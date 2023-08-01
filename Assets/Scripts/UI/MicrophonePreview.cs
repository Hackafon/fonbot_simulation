using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Fonbot.Common;
using Fonbot.UI;
using ROS2;
using UnityEngine;

namespace Fonbot.UI
{
    public class MicrophonePreview : MonoBehaviour
    {
        [SerializeField] private AudioSource _micSource;
        [SerializeField] private DropdownPanel _dropdownPanel;
        [SerializeField] private RectTransform _micBarPrefab;
        [SerializeField] private int _poolCapacity;
        [SerializeField] private int _barsCount;
        [SerializeField] private int _spectrumMultiplier;
        [SerializeField] private Vector2 _scaleRange;

        [SerializeField] private Vector3 _barStartingPosition;

        //distance between two bars
        [SerializeField] private float _barOffset;
        private ObjectPool _pool;
        private List<RectTransform> _activeBars;

        private ROS2UnityComponent _ros2Unity;
        private ROS2Node _ros2Node;
        [SerializeField] private Topic _microphoneSpectrumTopic;
        private float _spectrumAvg;

        private void Start()
        {
            _activeBars = new List<RectTransform>();
            _pool = gameObject.AddComponent<ObjectPool>();
            _pool.InitializePool(_micBarPrefab, _poolCapacity);

            _ros2Unity = GameObject.FindGameObjectWithTag("ROS2Manager").GetComponent<ROS2UnityComponent>();
            GetMicrophoneSpectrum();
        }

        private void GetMicrophoneSpectrum()
        {
            if (_ros2Node == null && _ros2Unity.Ok())
            {
                _ros2Node = _ros2Unity.CreateNode("FonbotMicrophone");

                _ = _ros2Node.CreateSubscription<std_msgs.msg.Float32>(
                    _microphoneSpectrumTopic.topicName, msg => _spectrumAvg = msg.Data);
            }
        }

        private void Update()
        {
            if (_dropdownPanel.Opened)
            {
                if (!_micSource.isPlaying)
                {
                    _micSource.time = Microphone.GetPosition(null) * 1.0f / 44100;
                    _micSource.Play();
                }

                ShowMicrophoneSpectrum();
            }
            else
            {
                if (_micSource.isPlaying)
                    _micSource.Stop();

                HideSpectrum();
            }
        }

        private void ShowMicrophoneSpectrum()
        {
            if (_activeBars.Count != _barsCount)
            {
                var _obj = _pool.GetPooledObject();
                if (_activeBars.Count == 0)
                {
                    _obj.anchoredPosition = _barStartingPosition;
                }
                else
                {
                    _obj.anchoredPosition =
                        new Vector3(
                            _activeBars[_activeBars.Count - 1].anchoredPosition.x + _micBarPrefab.rect.width +
                            _barOffset,
                            _barStartingPosition.y, _barStartingPosition.z);
                }

                _obj.transform.localScale = new Vector3(1,
                    Mathf.Clamp(_spectrumAvg * _spectrumMultiplier, _scaleRange.x, _scaleRange.y),
                    1);

                _activeBars.Add(_obj);
            }
            else
            {
                var _rect = _micBarPrefab.rect;

                for (int i = 0; i < _activeBars.Count - 1; i++)
                {
                    (_activeBars[i], _activeBars[i + 1]) = (_activeBars[i + 1], _activeBars[i]);
                    _activeBars[i].anchoredPosition3D -= new Vector3(_rect.width + _barOffset, 0, 0);
                }

                _activeBars[_activeBars.Count - 1].anchoredPosition =
                    _activeBars[_activeBars.Count - 2].anchoredPosition3D + new Vector3(_rect.width + _barOffset, 0, 0);
                _activeBars[_activeBars.Count - 1].transform.localScale = new Vector3(1,
                    Mathf.Clamp(_spectrumAvg * _spectrumMultiplier, _scaleRange.x, _scaleRange.y),
                    1);

            }
        }

        private void HideSpectrum()
        {
            if (_activeBars.Count == 0)
                return;

            foreach (var _rect in _activeBars)
            {
                _pool.ReturnObjectToPool(_rect);
            }

            _activeBars.RemoveAll((r) => true);
        }
    }
}