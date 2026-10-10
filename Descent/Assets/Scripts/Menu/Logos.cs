using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Q17pD.Menu
{
    public class Logos : MonoBehaviour
    {
        [SerializeField] private bool _skip;
        //[SerializeField] CursorHandler _cursorHandler;
        [SerializeField] private List<Logo> _logos;
        [SerializeField] private Image _logosBg;
        [SerializeField] private Image _logosDarkeningPanel;
        [SerializeField] private float _darkeningTime = 1f;
        [SerializeField] private List<GameObject> _disableList;
        [SerializeField] private UIHighlight _mainDarkeningPanel;

        private IEnumerator Start()
        {
            if(_skip) { foreach (GameObject obj in _disableList) obj.SetActive(false); _logosDarkeningPanel.enabled = false; _logosBg.enabled = false; StopAllCoroutines(); }
            //_cursorHandler.LockCursorToggle(true);
            yield return new WaitForSeconds(_darkeningTime);
            foreach(Logo logo in _logos)
            {
                logo._logoObj.SetActive(true);
                _logosDarkeningPanel.DOFade(0f, _darkeningTime);
                yield return new WaitForSeconds(_darkeningTime);
                yield return new WaitForSeconds(logo._time);
                _logosDarkeningPanel.DOFade(1f, _darkeningTime);
                yield return new WaitForSeconds(_darkeningTime);
                logo._logoObj.SetActive(false);
                
            }
            _mainDarkeningPanel.gameObject.SetActive(true);
            foreach (GameObject obj in _disableList) obj.SetActive(false);
            _logosDarkeningPanel.enabled = false;
            _logosBg.enabled = false;
            yield return new WaitForSeconds(_darkeningTime);
            _mainDarkeningPanel.UnHighlightImage(_darkeningTime);
            yield return new WaitForSeconds(_darkeningTime);
            _mainDarkeningPanel.gameObject.SetActive(false);
            //_cursorHandler.LockCursorToggle(false);
            gameObject.SetActive(false);
            
        }
    }
    [System.Serializable] public class Logo{ public GameObject _logoObj; [SerializeField] public float _time; }
}
