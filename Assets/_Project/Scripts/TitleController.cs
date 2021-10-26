using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleController : MonoBehaviour
{
    private bool showSettings = false;

    [SerializeField]
    GameObject mainOptions;
    [SerializeField]
    GameObject settingsOptions;

    public void LoadGame() {
        GameController.instance.PlayAudio(SoundName.SfxButtonMouseClick);
        GameController.instance.LoadGame();
    }

    public void QuitGame() {
        GameController.instance.PlayAudio(SoundName.SfxButtonMouseClick);
        GameController.instance.QuitGame();
    }

    public void ToggleSettings() {
        GameController.instance.PlayAudio(SoundName.SfxButtonMouseClick);
        showSettings = !showSettings;
        mainOptions.SetActive(!showSettings);
        settingsOptions.SetActive(showSettings);
    }

    public void SetMasterVolume(float value) {
        GameController.instance.SetMasterVolume(value);
    }
}
