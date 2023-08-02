using System;
using System.Collections.Generic;
using DG.Tweening;
using Fonbot.UI;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class Terminal : MonoBehaviour
{
    [FormerlySerializedAs("_commandLine")] [SerializeField]
    private RectTransform _commandInput;

    [SerializeField] private RectTransform _commandOutput;
    [SerializeField] private CommandInterpreter _interpreter;
    [SerializeField] private CameraPresets _presets;
    [SerializeField] private Canvas _canvas;
    [SerializeField] private Vector3 _tweenPosition;
    [SerializeField] private Vector3 _tweenRotation;
    [SerializeField] private Camera _mainCam;
    private List<GameObject> _terminalElements;
    private RectTransform _contentRect;
    private ScrollRect _scrollRect;
    private TMP_InputField _currentInput;
    private bool _cleared = false;

    private void Start()
    {
        _terminalElements = new List<GameObject>();
        _contentRect = GetComponent<RectTransform>();
        _scrollRect = GetComponentInParent<ScrollRect>();

        var _inputObj = Instantiate(_commandInput, gameObject.transform);
        var _input = _inputObj.GetComponentInChildren<TMP_InputField>();
        _input.ActivateInputField();
        _input.onSubmit.AddListener((cmd) =>
        {
            SubmitCommand(cmd);
            _input.interactable = false;
        });
        _currentInput = _input;
        _terminalElements.Add(_inputObj.gameObject);
    }

    private void Update()
    {
        if (!_currentInput.isFocused)
        {
            _currentInput.ActivateInputField();
        }
    }

    private void SubmitCommand(string cmd)
    {
        if (cmd.Trim().Length > 0)
        {
            string _intOutput = _interpreter.Interpret(cmd);
            if (!_cleared)
            {
                var _output = Instantiate(_commandOutput, gameObject.transform);
                var _outputTxt = _output.GetComponent<TextMeshProUGUI>();
                _outputTxt.SetText(_intOutput);
                _terminalElements.Add(_output.gameObject);
            }
        }

        var _inputObj = Instantiate(_commandInput, gameObject.transform);
        var _input = _inputObj.GetComponentInChildren<TMP_InputField>();
        _input.ActivateInputField();
        _input.onSubmit.AddListener((newCmd) =>
        {
            SubmitCommand(newCmd);
            _input.interactable = false;
        });

        //scroll to new input
        Canvas.ForceUpdateCanvases();
        _contentRect.anchoredPosition = new Vector2(_contentRect.anchoredPosition.x,
            ((Vector2)_scrollRect.transform.InverseTransformPoint(_contentRect.position)
             - (Vector2)_scrollRect.transform.InverseTransformPoint(_inputObj.position)).y);

        _currentInput = _input;
        _terminalElements.Add(_inputObj.gameObject);

        LayoutRebuilder.ForceRebuildLayoutImmediate(_contentRect);
        _cleared = false;
    }

    public void OpenTerminal()
    {
        _mainCam.transform.DOMove(_tweenPosition, 1f).SetEase(Ease.InOutSine).OnComplete((() =>
        {
            _mainCam.transform.DORotate(_tweenRotation, 0.5f).SetEase(Ease.InOutSine).OnComplete(
                () =>
                {
                    transform.parent.gameObject.SetActive(true);
                    transform.parent.DOScale(new Vector3(1, 1, 1), 0.5f);
                    _canvas.renderMode = RenderMode.ScreenSpaceCamera;
                    _canvas.worldCamera = Camera.main;
                    _canvas.planeDistance = 0.2f;
                });
        }));
        
    }
    
    public void ExitTerminal()
    {
        transform.parent.DOScale(new Vector3(0, 0, 0), 0.5f).OnComplete(() =>
        {
            transform.parent.gameObject.SetActive(false);
            _canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            _presets.TweenToCurrentPreset();
        });
    }

    public void ClearTerminal()
    {
        foreach (var _te in _terminalElements)
        {
            Destroy(_te);
        }

        _terminalElements.RemoveAll((el) => true);
        _cleared = true;
    }
}