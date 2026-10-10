using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Q17pD
{
    public class UIHighlight : MonoBehaviour
    {
        private Image _image;
        private TextMeshProUGUI _text;
        private bool _initialized = false;

        private void Awake() { Initialize(); }
        private void Initialize() { if (_initialized) return; TryGetComponent(out _image); TryGetComponent(out _text); _initialized = true; }
        public void HighlightImage(float value = 1f, float time = 0.2f) { if (!_initialized) Initialize(); if (_image) _image.DOFade(value, time); }
        public void UnHighlightImage(float time = 0.2f) { if (!_initialized) Initialize(); if (_image) _image.DOFade(0f, time); }
        public void HighlightImageFixedValues() { if (!_initialized) Initialize(); if (_image) _image.DOFade(0.7f, 0.2f); }
        public void UnHighlightImageFixedValues() { if (!_initialized) Initialize(); if (_image) _image.DOFade(0.25f, 0.2f); }
        public void HighlightTMP(float value = 1f, float time = 0.2f) { if (!_initialized) Initialize(); if (_text) _text.DOFade(value, time); }
        public void UnHighlightTMP(float time = 0.2f) { if (!_initialized) Initialize(); if (_text) _text.DOFade(0f, time); }
        public void HighlightTMPFixedValues() { if (!_initialized) Initialize(); if (_text) _text.DOFade(1f, 0.2f); }
        public void UnHighlightTMPFixedValues() { if (!_initialized) Initialize(); if (_text) _text.DOFade(0.25f, 0.2f); }
    }
}