using DG.Tweening;
using System;
using System.Collections;
using UnityEngine;

namespace Q17pD.Descent.Interactable
{
    public abstract class Interactable : MonoBehaviour
    {
        [SerializeField] private GameObject _tip;
        protected AudioHandler _audioHandler;
        protected bool _singleUse = true, _available = true;
        protected float _sleep = 0.5f;
        private void OnTriggerEnter(Collider other)
        {
            if (other.tag != "Player") return;
            if (_available)
            {
                
                _tip.SetActive(true);
                StartCoroutine(ListenKeyCoroutine());
            }
            if (_audioHandler == null) _audioHandler = other.GetComponent<AudioHandlerHolder>().AudioHandler;
            
        }
        private void OnTriggerExit(Collider other)
        {
            if (other.tag != "Player") return;
            StopAllCoroutines();
        }
        private IEnumerator ListenKeyCoroutine()
        {
            while (true)
            {
                yield return null;
                if (Input.GetKeyDown(KeyCode.E))
                {
                    _tip.SetActive(false);
                    Action();
                    if (_singleUse) { _available = false; StopAllCoroutines(); }
                    else yield return new WaitForSeconds(_sleep);
                    _tip.SetActive(true);
                }
            }
        }
        protected abstract void Action();
    }
}
