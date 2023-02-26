using DG.Tweening;
using TMPro;
using UnityEngine;

public class UIPulsingText : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _text;
    [SerializeField] private float _pulseCycleDuration = 1f;
    [SerializeField] private Ease _ease = Ease.Linear;

    private Tween _pulseTween;
    private Color _defaultMaterialColor;
    private Color _zeroAlphaMatrialColor;

    private Material _targetMaterial;
    
    private static readonly int FaceColor = Shader.PropertyToID("_FaceColor");

    private void OnValidate()
    {
        if (_text == null)
            _text = GetComponentInChildren<TextMeshProUGUI>();
    }

    private void Awake()
    {
        Init();
    }

    private void OnEnable()
    {
        StartPulse();
    }

    private void OnDisable()
    {
        StopPulse();
    }

    private void Init()
    {
        _targetMaterial = _text.fontMaterial;
        _defaultMaterialColor = _targetMaterial.GetColor(FaceColor);
        _zeroAlphaMatrialColor = _defaultMaterialColor;
        _zeroAlphaMatrialColor.a = 0;
    }

    private void StartPulse()
    {
        _pulseTween = _targetMaterial
            .DOColor(_zeroAlphaMatrialColor,FaceColor, _pulseCycleDuration * 0.5f)
            .SetEase(_ease)
            .SetLoops(-1, LoopType.Yoyo);

        _pulseTween.Play();
    }

    private void StopPulse()
    {
        _pulseTween?.Kill();
        _targetMaterial.SetColor(FaceColor, _defaultMaterialColor);

    }
}
