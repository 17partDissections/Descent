using UnityEngine;

namespace Q17pD.Descent
{
    public class PlayerPrefsInit : MonoBehaviour
    {
        //an init for playerprefs vars if player ran a game in the first time
        //and also list of all playerprefs vars in Descent namespace
        private void Start()
        {
            if(PlayerPrefs.GetInt("Inited") == 0)
            {
                PlayerPrefs.SetInt("MusicVolume", 1);
                PlayerPrefs.SetInt("SFXVolume", 1);
                //PlayerPrefs.SetInt("MusicToggleValue", 1);
                //PlayerPrefs.SetInt("SFXToggleValue", 1);
                PlayerPrefs.SetFloat("MusicSliderValue", 1);
                PlayerPrefs.SetFloat("SFXSliderValue", 1);
                /*PlayerPrefs.SetInt("ForcedRatio", 1);*/ Screen.SetResolution(640, 480, FullScreenMode.FullScreenWindow);
                PlayerPrefs.SetInt("Inited", 1);
            }
        }
    }
}
