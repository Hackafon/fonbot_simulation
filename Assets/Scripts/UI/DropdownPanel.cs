using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace Fonbot.UI
{
    public class DropdownPanel : MonoBehaviour
    {
        [SerializeField] private RectTransform _panelTransform;
        [SerializeField] private RectTransform _arrowTransform;
        [SerializeField] private Vector2 _yBounds;
        [SerializeField] private float _tweenDuration;
        [SerializeField] private bool _invertArrow;
        public bool Opened { get; private set; }

        public void TogglePanel()
        {
            if (Opened)
            {
                HidePanel();
            }
            else
            {
                ShowPanel();
            }

            Opened = !Opened;
        }

        void ShowPanel()
        {
            _panelTransform.DOAnchorPosY(_yBounds.y, _tweenDuration).SetEase(Ease.InOutSine);
            _arrowTransform.DORotate(_invertArrow ? new Vector3(0, 0, 180f) : Vector3.zero, _tweenDuration)
                .SetEase(Ease.InOutSine);
        }

        void HidePanel()
        {
            _panelTransform.DOAnchorPosY(_yBounds.x, _tweenDuration).SetEase(Ease.InOutSine);
            _arrowTransform.DORotate(_invertArrow ? Vector3.zero : new Vector3(0, 0, 180f), _tweenDuration)
                .SetEase(Ease.InOutSine);
        }
    }
}