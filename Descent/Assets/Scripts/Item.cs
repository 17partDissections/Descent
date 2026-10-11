using DG.Tweening;
using System.Collections;
using UnityEngine;

namespace Q17pD.Descent
{
    public class Item : MonoBehaviour
    {
        [SerializeField] private string _key;
        [SerializeField] private Sprite _sprite;
        private float _sleep = 1f;

        private IEnumerator Start()
        {
            WaitForSeconds sleep = new WaitForSeconds(_sleep);
            transform.DOLocalMoveY(transform.localPosition.y - 0.15f, _sleep).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutSine);
            yield return sleep;
            while (true)
            {
                //transform.DORotate(Vector3.up * 360f, 2f, RotateMode.FastBeyond360).SetLoops(-1, LoopType.Restart).SetEase(Ease.Linear);
                transform.DOLocalMoveY(transform.localPosition.y + 0.3f, _sleep).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutSine);
                yield return sleep;
                transform.DOLocalMoveY(transform.localPosition.y - 0.3f, _sleep).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutSine);
                yield return sleep;
            }
        }
        private void OnTriggerEnter(Collider other)
        {
            if (other.tag != "Player") return;
            other.GetComponent<Player>().AddItem(_key, _sprite);
            transform.DOKill();
            Destroy(gameObject);
        }
    }
}
