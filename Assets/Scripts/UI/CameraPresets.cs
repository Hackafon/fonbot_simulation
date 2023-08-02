using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;

namespace Fonbot.UI
{
    public class CameraPresets : MonoBehaviour
    {
        [SerializeField] private Camera _mainCam;
        [SerializeField] private TextMeshProUGUI _presetText;
        [SerializeField] private List<Vector3> _cameraPositionPresets;
        [SerializeField] private List<Vector3> _cameraRotationPresets;
        [SerializeField] private int _defaultPreset;
        private int _currentPreset;

        private void Start()
        {
            _currentPreset = _defaultPreset;
            TweenToCurrentPreset();
        }

        public void TweenCamera(Vector3 position, Vector3 rotation)
        {
            _presetText.SetText((_currentPreset + 1).ToString());

            _mainCam.transform.DOMove(position, 1f).SetEase(Ease.InOutSine);
            _mainCam.transform.DORotate(rotation, 1f).SetEase(Ease.InOutSine);
        }

        public void TweenToCurrentPreset()
        {
            TweenCamera(_cameraPositionPresets[_currentPreset], _cameraRotationPresets[_currentPreset]);
        }

        public void NextPreset()
        {
            if (_currentPreset + 1 < _cameraPositionPresets.Count)
                _currentPreset++;
            else
                _currentPreset = 0;

            TweenToCurrentPreset();
        }
    }
}