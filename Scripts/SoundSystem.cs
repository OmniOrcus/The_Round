using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SoundSystem : MonoBehaviour {

    public AudioClip[] audioClips;
    AudioSource player;

	// Use this for initialization
	void Awake () {
        player = GetComponent<AudioSource>();
	}

    public void PlaySound(uint index, bool playOver = false) {
        Debug.Log(gameObject.name + " is playing sound " + index + " with play over as " + playOver);
        Debug.Assert(index < audioClips.Length, gameObject.name + " has no sound " + index);
        if (playOver || !player.isPlaying) {
            player.PlayOneShot(audioClips[index]);
        }
    }
	
}
