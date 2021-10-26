using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenuController : MonoBehaviour
{
    public static PauseMenuController instance;

    [SerializeField]
    private GameObject pauseMenu;
    [SerializeField]
    private Slider masterVolumeSlider;

    void Awake() {
        // singleton setup
        if (instance == null) {
            instance = this;
        } else if (instance != this) {
            Destroy(gameObject);
        }
    }

    void Start() {
        pauseMenu.SetActive(false);
    }

    public void ShowPauseMenu(bool shouldShow = true) {
        InitializeSliderVolume();
        pauseMenu.SetActive(shouldShow);

    }

    void InitializeSliderVolume() {
        masterVolumeSlider.value = GameController.instance.GetMasterVolume();
    }

    public void SetMasterVolume(float value) {
        GameController.instance.SetMasterVolume(value);
    }

    public void Resume() {
        GameController.instance.TogglePause();
    }

    public void MainMenu() {
        GameController.instance.LoadTitle();
    }

    public void Quit() {
        GameController.instance.QuitGame();
    }
}
