using System.Collections;
using TMPro;
using UnityEngine;

namespace Q17pD
{
    public class Version : MonoBehaviour
    {
        private IEnumerator Start() { yield return new WaitForSeconds(1); GetComponent<TextMeshProUGUI>().text += (Application.version); }
    }
}
