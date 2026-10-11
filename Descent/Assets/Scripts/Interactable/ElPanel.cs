using DG.Tweening;
using UnityEngine;

namespace Q17pD.Descent.Interactable
{
    public class ElPanel : Interactable
    {
        [SerializeField] private Transform _door, _gate;
        [SerializeField] private AudioClip _doorClip, _gateClip;

        private void Start() { _singleUse = false; }
        protected override void Action()
        {
            if (_door != null)
            {
                _sleep = 1.5f;
                _audioHandler.PlaySound(SoundType.SFX, _doorClip);
                _door.DORotate(new Vector3(0, 0, -5), 0.5f, RotateMode.LocalAxisAdd)
                    .OnComplete(() => _door.DORotate(new Vector3(0, 0, -85), 1f, RotateMode.LocalAxisAdd)
                        .OnComplete(() => _door = null));
            }
            else
            {
                _audioHandler.PlaySound(SoundType.SFX, _gateClip);
                _gate.DOMoveY(9.5f, 10f);
                _available = false;
                StopAllCoroutines();
            }
        }
    }
}
