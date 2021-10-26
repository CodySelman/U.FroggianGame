using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public enum SoundName {
    AmbienceForest,
    AmbienceSky,
    AmbienceSpace,
    MusicMain,
    SfxButtonMouseClick,
    SfxButtonMouseOver,
    SfxBounce,
    SfxChargeUpFast,
    SfxChargeUpSlow,
    SfxFootstep1,
    SfxFootstep2,
    SfxFootstep3,
    SfxFootstep4,
    SfxGrapple,
    SfxJump1,
    SfxJump2,
    SfxLand1,
    SfxLand2
}

public class GameController : MonoBehaviour
{
    public static GameController instance;

    // State Machine
    private StateMachine sm;
    [System.NonSerialized]
    public GameNormalState normalState;
    [System.NonSerialized]
    public GamePauseState pauseState;
    [System.NonSerialized]
    public GameTitleState titleState;

    // components
    [SerializeField]
    AudioMixer audioMixer;
    [SerializeField]
    AudioMixerGroup audioMixerGroup;

    // variables
    public bool isPaused = false;
    public Sound[] sounds;

    void Awake() {
        // singleton setup
        if (instance == null) {
            instance = this;
            DontDestroyOnLoad(gameObject);
        } else if (instance != this) {
            Destroy(gameObject);
        }
    }

    void Start() {
        SetupAudioSources();

        sm = new StateMachine();
        normalState = new GameNormalState(this, sm);
        pauseState = new GamePauseState(this, sm);
        titleState = new GameTitleState(this, sm);
        sm.Initialize(titleState);
    }

    void Update() {
        sm.CurrentState.HandleInput();
        sm.CurrentState.LogicUpdate();
    }

    void FixedUpdate() {
        sm.CurrentState.PhysicsUpdate();
    }

    void LateUpdate()
    {
        sm.CurrentState.LateUpdate();
    }

    public void TogglePause() {
        Debug.Log("TogglePause");
        isPaused = !isPaused;
        if (isPaused) {
            sm.ChangeState(pauseState);
        } else {
            sm.ChangeState(normalState);
        }
    }

    public void ListenForPause() {
        Debug.Log("ListenForPause");
        if (Input.GetKeyDown(KeyCode.Escape)) {
            TogglePause();
        }
    }

    public void LoadTitle() {
        if (SceneManager.GetActiveScene().name != Constants.SCENE_TITLE) {
            sm.ChangeState(titleState);
            SceneManager.LoadScene(Constants.SCENE_TITLE);
        }
    }

    public void LoadGame() {
        if (SceneManager.GetActiveScene().name != Constants.SCENE_GAME) {
            sm.ChangeState(normalState);
            SceneManager.LoadScene(Constants.SCENE_GAME);
        }
    }

    public void QuitGame() {
        Application.Quit();
    }

    public float GetMasterVolume() {
        float volume = 0;
        audioMixer.GetFloat(Constants.AUDIO_MASTER_VOLUME, out volume);
        return GetSliderValueFromVolume(volume);
    }

    public void SetMasterVolume(float value) {
        audioMixer.SetFloat(Constants.AUDIO_MASTER_VOLUME, GetVolumeFromSliderValue(value));
    }

    float GetVolumeFromSliderValue(float value) {
        // -20 decibels is the min volume for our slider
        // mute (-80db) if the slider hits its lowest value (0)
        return value > 0 ? (value * 20) - 20 : -80;
    }

    float GetSliderValueFromVolume(float value) {
        if (value > 0) {
            // if volume is above 0db, return 1 (max slider value)
            return 1f;
        } else if (value < -20) {
            // if volume is below min volume (-20db) return 0
            return 0;
        } else {
            // calculate slider value
            return (value / 20) + 1;
        }
    }

    void SetupAudioSources() {
        foreach (Sound s in sounds) {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.loop = s.loop;
            s.source.playOnAwake = s.playOnAwake;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.outputAudioMixerGroup = audioMixerGroup;

            if (s.playOnAwake) {
                s.source.Play();
            }
        }
    }

    public void PlayAudio(SoundName soundName) {
        Sound s = Array.Find(sounds, sound => sound.name == soundName);
        if (s.randomPitch) {
            s.source.pitch = UnityEngine.Random.Range(s.pitchRange.x, s.pitchRange.y);
        }
        s.source.Play();
    }

    public void StopAudio(SoundName soundName) {
        GetAudioSource(soundName).Stop();
    }

    public void PauseAudio(SoundName soundName) {
        GetAudioSource(soundName).Pause();
    }

    public AudioSource GetAudioSource(SoundName soundName) {
        Sound s = Array.Find(sounds, sound => sound.name == soundName);
        return s.source;
    }

    public void PlayMusicAndAmbience(bool shouldPlay = true) {
        if (shouldPlay) {
            if (!GetAudioSource(SoundName.AmbienceForest).isPlaying) {
                PlayAudio(SoundName.AmbienceForest);
            }
            if (!GetAudioSource(SoundName.MusicMain).isPlaying) {
                PlayAudio(SoundName.MusicMain);
            }
        } else {
            if (GetAudioSource(SoundName.AmbienceForest).isPlaying) {
                StopAudio(SoundName.AmbienceForest);
            }
            if (GetAudioSource(SoundName.MusicMain).isPlaying) {
                StopAudio(SoundName.MusicMain);
            }
        }
    }
}
