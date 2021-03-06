﻿using UnityEngine;
using System.Collections.Generic;

public enum WalkingSurfaceTypes
{
    None = 0,
    Grass = 1,
    InteriorFloor = 2
}

public enum GameEvent
{
    None,
    Death,
    Win,
    CorrectItem,
    InCorrectItem
}

public enum GameState
{
    Playing,
    Victory,
    Loss
}

public class SoundManager : MonoBehaviour
{
    public List<AudioClip> WalkingOnGrassAudioClips;
    public List<AudioClip> WalkingOnInteriorFloorsAudioClips;
    public List<AudioClip> SniffAudioClips;
    public List<AudioClip> PantingAudioClips;
    public List<AudioClip> DeathAudioClips;
    public List<AudioClip> WinAudioClips;
    public List<AudioClip> CorrectAudioClip;
    public List<AudioClip> InCorrectAudioClip;
    public AudioSource outputWalkingSource;
    public AudioSource BreathingSource;
    public AudioSource IdlePantingSource;
    public AudioSource SingleEventSource;
    public AudioSource MusicAudioSource;

    public WalkingSurfaceTypes CurrentWalkingSurface = WalkingSurfaceTypes.None;

    public static SoundManager instance = null;     // Allows other scripts to call functions from SoundManager.             
    public float lowPitchRange = .95f;              // The lowest a sound effect will be randomly pitched.
    public float highPitchRange = 1.05f;            // The highest a sound effect will be randomly pitched.                 
    public float lowGrassPitchRange = .85f;              // The lowest a sound effect will be randomly pitched.
    public float highGrassPitchRange = .95f;            // The highest a sound effect will be randomly pitched.           
    public float lowVolumeRange = .90f;
    public float highVolumeRange = 1.0f;
    public float IdleBreathingVolume = 0.75f;
    public float WalkingBreathingVolume = 0.25f;
    public float SniffingVolume = 1.0f;
    public float DeathVolume = 1.0f;
    public float MusicVolume = 0.7f;

    private GameState CurrentGameState = GameState.Playing;
    private GameEvent NextGameEventToPlay = GameEvent.None;

    public int TotalGameProgression;
    public int CurrentGameProgression = 0;
    public List<AudioClip> GameMusicAudioClips;
    private int CurrentMusicIndex = 0;

    void Awake()
    {
        // Check if there is already an instance of SoundManager
        if (instance == null)
            // if not, set it to this.
            instance = this;
        // If instance already exists:
        else if (instance != this)
            // Destroy this, this enforces our singleton pattern so there can only be one instance of SoundManager.
            Destroy(gameObject);

        // Set SoundManager to DontDestroyOnLoad so that it won't be destroyed when reloading our scene.
        DontDestroyOnLoad(gameObject);
    }

    public void Update()
    {
        // TODO: figure out what the pace for the game music should be
        // ManageGameMusic();

        switch (CurrentWalkingSurface)
        {
            case WalkingSurfaceTypes.Grass:
                if (!outputWalkingSource.isPlaying && 
                    CurrentGameState == GameState.Playing)
                {
                    RandomizeSfx(ref outputWalkingSource, lowGrassPitchRange, highGrassPitchRange, WalkingOnGrassAudioClips.ToArray());
                    outputWalkingSource.Play();
                }
                PlayWalkingBreathingInLoop();
                break;
            case WalkingSurfaceTypes.InteriorFloor:
                if (!outputWalkingSource.isPlaying && 
                    CurrentGameState == GameState.Playing)
                {
                    RandomizeSfx(ref outputWalkingSource, lowPitchRange, highPitchRange, WalkingOnInteriorFloorsAudioClips.ToArray());
                    outputWalkingSource.Play();
                }
                PlayWalkingBreathingInLoop();
                break;
            default:
                // stop playing walking sound
                outputWalkingSource.Stop();
                // stop playing running breathing sound
                BreathingSource.Stop();

                // evaluate and play sounds related to single audio game events
                if (!IdlePantingSource.isPlaying &&
                    !SingleEventSource.isPlaying)
                {
                    ManageGameEventAudio();
                }
                break;
        }
    }

    private void ManageGameEventAudio()
    {
        switch (NextGameEventToPlay)
        {
            case GameEvent.Death:
                break;
            case GameEvent.Win:
                break;
            case GameEvent.CorrectItem:
                RandomizeSfx(ref SingleEventSource, 0.95f, 1.0f, CorrectAudioClip.ToArray());
                SingleEventSource.loop = false;
                SingleEventSource.volume = SniffingVolume;
                SingleEventSource.Play();
                break;
            case GameEvent.InCorrectItem:
                RandomizeSfx(ref SingleEventSource, 0.95f, 1.0f, InCorrectAudioClip.ToArray());
                SingleEventSource.loop = false;
                SingleEventSource.volume = SniffingVolume;
                SingleEventSource.Play();
                break;
            case GameEvent.None:
                if (CurrentGameState == GameState.Playing)
                {
                    RandomizeSfx(ref IdlePantingSource, 0.95f, 1.0f, PantingAudioClips.ToArray());
                    IdlePantingSource.volume = IdleBreathingVolume;
                    IdlePantingSource.Play();
                }
                break;
        }
        NextGameEventToPlay = GameEvent.None;
    }

    private void ManageGameMusic()
    {
        if (!MusicAudioSource.isPlaying && CurrentGameState == GameState.Playing)
        {
            if (CurrentMusicIndex <= CurrentGameProgression)
            {
                CurrentMusicIndex = CurrentGameProgression;
                if (CurrentMusicIndex >= GameMusicAudioClips.Count)
                {
                    CurrentMusicIndex = 0;
                    CurrentGameProgression = 0;
                }
            }
            MusicAudioSource.clip = GameMusicAudioClips[CurrentMusicIndex];
            MusicAudioSource.loop = false;
            MusicAudioSource.volume = MusicVolume;
            MusicAudioSource.Play();
        }
    }

    private void PlayWalkingBreathingInLoop()
    {
        // play the breathing loop while running
        if (!BreathingSource.isPlaying && 
            CurrentGameState == GameState.Playing)
        {
            BreathingSource.volume = WalkingBreathingVolume;
            BreathingSource.loop = true;
            BreathingSource.Play();
        }
    }

    public void SetCurrentWalkingSurface(WalkingSurfaceTypes value)
    {
        CurrentWalkingSurface = value;
    }

    // RandomizeSfx chooses randomly between various audio clips and slightly changes their pitch.
    public void RandomizeSfx(ref AudioSource outputAudioSource, float lowerLimit, float upperLimit, params AudioClip[] inputClips)
    {
        outputAudioSource.clip = RandomClip(inputClips);
        // Choose a random pitch to play back our clip at between our high and low pitch ranges.
        outputAudioSource.pitch = RandomFloat(lowerLimit, upperLimit);
        outputAudioSource.volume = RandomFloat(lowVolumeRange, highVolumeRange);
    }

    private AudioClip RandomClip(params AudioClip[] clips)
    {
        // Generate a random number between 0 and the length of our array of clips passed in.
        return clips[Random.Range(0, clips.Length)];
    }

    private float RandomFloat(float lower, float upper)
    { 
        return Random.Range(lower, upper);
    }

    public void Sniff()
    {
        // stop both breathing sounds
        BreathingSource.Stop();
        IdlePantingSource.Stop();

        // play the event sound
        RandomizeSfx(ref SingleEventSource, 0.98f, 1.0f, SniffAudioClips.ToArray());
        SingleEventSource.loop = false;
        SingleEventSource.volume = SniffingVolume;
        SingleEventSource.Play();
    }

    public void Death()
    {
        if (CurrentGameState != GameState.Playing)
            return;

        CurrentGameState = GameState.Loss;
        NextGameEventToPlay = GameEvent.Death;

        // stop all other dog sounds
        outputWalkingSource.Stop();
        BreathingSource.Stop();
        IdlePantingSource.Stop();
        MusicAudioSource.Stop();

        // play the event sound
        if (DeathAudioClips.Count != 0)
        {
            RandomizeSfx(ref SingleEventSource, 0.98f, 1.0f, DeathAudioClips.ToArray());
            SingleEventSource.loop = false;
            SingleEventSource.volume = DeathVolume;
            SingleEventSource.Play();
        }
    }

    public void Win()
    {
        if (CurrentGameState != GameState.Playing)
            return;

        CurrentGameState = GameState.Victory;
        NextGameEventToPlay = GameEvent.Win;

        // stop all other dog sounds
        outputWalkingSource.Stop();
        BreathingSource.Stop();
        IdlePantingSource.Stop();
        MusicAudioSource.Stop();

        // play the event sound
        if (WinAudioClips.Count != 0)
        {
            RandomizeSfx(ref SingleEventSource, 0.98f, 1.0f, WinAudioClips.ToArray());
            SingleEventSource.loop = false;
            SingleEventSource.volume = DeathVolume;
            SingleEventSource.Play();
        }
    }

    public void FoundCorrectItem()
    {
        CurrentGameProgression++;
        NextGameEventToPlay = GameEvent.CorrectItem;
    }

    public void FoundInCorrectItem()
    {
        NextGameEventToPlay = GameEvent.InCorrectItem;
    }
}