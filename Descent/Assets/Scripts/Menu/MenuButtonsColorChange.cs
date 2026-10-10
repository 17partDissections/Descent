using TMPro;
using UnityEngine;

namespace Q17pD.Menu
{
    public class MenuButtonsColorChange : MonoBehaviour
    {
        private TextMeshProUGUI _text;
        [SerializeField] private Color _defaultColor;
        [SerializeField] private Color _highlightedColor;
        [SerializeField] private Color _pressedColor;
        private void Start() { _text = GetComponent<TextMeshProUGUI>(); }
        public void SetDefaultColor() { _text.color = _defaultColor; }
        public void SetHighlightedColor() {  _text.color = _highlightedColor; }
        public void SetPressedColor() { _text.color = _pressedColor; }
    }
}
