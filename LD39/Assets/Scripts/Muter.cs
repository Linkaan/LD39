using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Muter : MonoBehaviour {

	public AudioSource toMute;
	public Image image;

	public Sprite unmuted;
	public Sprite muted;

	public KeyCode keyToMute;

	public bool controlsSfx;

	private static bool isMutedSfx;
	private static bool isMutedMusic;

	private Player player;

	void Start () {
		player = GameObject.FindObjectOfType<Player> ();
	}

	void Update () {
		if (Input.GetKeyDown (keyToMute) && !player.ball.dead) {
			this.ToggleMute ();
		}
		if (toMute.mute != (controlsSfx ? isMutedSfx : isMutedMusic))
			ToggleMute ();
	}

	public void ToggleMute () {
		toMute.mute = !toMute.mute;
		if (controlsSfx)
			isMutedSfx = toMute.mute;
		else
			isMutedMusic = toMute.mute;
		image.overrideSprite = toMute.mute ? muted : unmuted;
	}
}
