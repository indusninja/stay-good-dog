using UnityEngine;
using System.Collections.Generic;

public enum WalkingSurfaceTypes
{
    None = 0,
    Grass = 1,
    InteriorFloor = 2
}

public class SoundManager : MonoBehaviour
{
    public List<AudioClip> WalkingOnGrassAudioClips;
    public List<AudioClip> WalkingOnInteriorFloorsAudioClips;
    public AudioSource outputWalkingSource;

    public WalkingSurfaceTypes CurrentWalkingSurface = WalkingSurfaceTypes.None;

    public static SoundManager instance = null;     // Allows other scripts to call functions from SoundManager.             
    public float lowPitchRange = .95f;              // The lowest a sound effect will be randomly pitched.
    public float highPitchRange = 1.05f;            // The highest a sound effect will be randomly pitched.                 
    public float lowGrassPitchRange = .85f;              // The lowest a sound effect will be randomly pitched.
    public float highGrassPitchRange = .95f;            // The highest a sound effect will be randomly pitched.           
    public float lowVolumeRange = .90f;
    public float highVolumeRange = 1.0f;

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
        if (outputWalkingSource.isPlaying)
            return;

        switch (CurrentWalkingSurface)
        {
            case WalkingSurfaceTypes.Grass:
                RandomizeSfx(ref outputWalkingSource, lowGrassPitchRange, highGrassPitchRange, WalkingOnGrassAudioClips.ToArray());
                break;
            case WalkingSurfaceTypes.InteriorFloor:
                RandomizeSfx(ref outputWalkingSource, lowPitchRange, highPitchRange, WalkingOnInteriorFloorsAudioClips.ToArray());
                break;
            default:
                // stop playing
                outputWalkingSource.Stop();
                break;
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
        outputWalkingSource.Play();
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
}