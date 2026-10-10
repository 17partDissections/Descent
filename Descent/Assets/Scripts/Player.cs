using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Q17pD.Descent
{
    public class Player : MonoBehaviour
    {
        [SerializeField] private float _speed, _rotation;
        [SerializeField] private List<AudioClip> _footsteps;
        [SerializeField] private List<Image> _itemIcons;
        private AudioHandler _audioHandler;
        private WaitForSeconds _sleep = new WaitForSeconds(0.5f);
        private List<string> _items = new List<string>();

        public void AddItem(string key, Sprite sprite)
        {
            _items.Add(key);
            Image freeIcon = _itemIcons.FirstOrDefault(x => !x.transform.parent.gameObject.activeSelf);
            freeIcon.sprite = sprite;
            freeIcon.transform.parent.gameObject.SetActive(true);
        }
        public void RemoveItem(string key)
        {
            _items.Remove(key);
            Image neededIcon = _itemIcons.FirstOrDefault(x => x.transform.parent.gameObject.activeSelf && x.sprite.name == key);
            neededIcon.transform.parent.gameObject.SetActive(false);
        }
        private void Start() { _audioHandler = GetComponent<AudioHandlerHolder>().AudioHandler; StartCoroutine(FootstepCoroutine());  }
        private void Update()
        {
            float move = Input.GetAxis("Vertical"); float turn = Input.GetAxis("Horizontal");
            transform.Translate(0, 0, move * _speed * Time.deltaTime);
            transform.Rotate(0, turn * _rotation * Time.deltaTime, 0);
        }
        private IEnumerator FootstepCoroutine()
        {
            while (true)
            {
                float move = Input.GetAxis("Vertical");
                if (move != 0) _audioHandler.PlaySound(SoundType.SFX, _footsteps[Random.Range(0, _footsteps.Count)]);
                yield return _sleep;
            }
        }
    }
}