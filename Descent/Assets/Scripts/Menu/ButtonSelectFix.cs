using UnityEngine;
using UnityEngine.EventSystems;

namespace Q17pD.Menu
{
    public class ButtonSelectFix : MonoBehaviour { void OnEnable() => EventSystem.current.SetSelectedGameObject(gameObject); }
}
