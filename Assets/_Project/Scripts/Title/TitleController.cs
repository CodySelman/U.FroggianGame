using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitleController : MonoBehaviour
{
    private bool showSettings = false;

    [SerializeField]
    GameObject mainOptions;
    [SerializeField]
    GameObject settingsOptions;
    [SerializeField]
    Slider masterVolumeSlider;

    void Start() {
        mainOptions.SetActive(true);
        settingsOptions.SetActive(false);
    }

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
        if (showSettings) {
            InitializeSliderVolume();
        }
    }

    public void SetMasterVolume(float value) {
        GameController.instance.SetMasterVolume(value);
    }

    void InitializeSliderVolume() {
        masterVolumeSlider.value = GameController.instance.GetMasterVolume();
    }
}
