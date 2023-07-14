using UnityEngine;

public static class GlobalVar {
	public static float shakeForce, shakeTime, immortalTime = 0f;
	public static uint score = 0, deaths = 0, coins = 5;

	public struct Player {
		public static float speed, shootInterval, bulletDamage, jumpForce, health = 1f;
		public static int jumpMax, bulletsNum;
	}
	public struct Audio {
		public static AudioClip clip;
		public static bool loop;
	}

	public static Vector2 roomBottomLeft, roomTopRight;
	public static string goTo;
}

public class Main : MonoBehaviour {
	[Header("Components")]
	public AudioSource audioSource;
	public AudioSource[] stagesAudio;

	[Header("GlobalVars (Initialization only)")]
	public float speed;
	public float shootInterval, bulletDamage, jumpForce;
	public int jumpMax, bulletsNum;

	[Header("GlobalVars (Debug only)")]
	public AudioClip audioClip;
	public float shakeForce, shakeTime;

	float lastShakeForce, lastShakeTime;
	AudioClip lastAudioClip;

	private void Awake() {
		DontDestroyOnLoad(gameObject);
		GlobalVar.Player.speed = speed;
		GlobalVar.Player.shootInterval = shootInterval;
		GlobalVar.Player.bulletDamage = bulletDamage;
		GlobalVar.Player.jumpMax = jumpMax;
		GlobalVar.Player.jumpForce = jumpForce;
		GlobalVar.Player.bulletsNum = bulletsNum;
	}

	private void Update() {
		#region Debug
		// if (shakeForce != lastShakeForce) lastShakeForce = GlobalVar.shakeForce = shakeForce;
		// if (shakeTime != lastShakeTime) lastShakeTime = GlobalVar.shakeTime = shakeTime;
		// if (audioClip != lastAudioClip) lastAudioClip = GlobalVar.Audio.clip = audioClip;
		#endregion

		if (GlobalVar.Audio.clip != lastAudioClip) {
			if (GlobalVar.Audio.clip != null) {
				audioSource.clip = lastAudioClip = GlobalVar.Audio.clip;
				audioSource.loop = GlobalVar.Audio.loop;
				audioSource.Play();
			} else if (audioSource.isPlaying) audioSource.Stop();
		}
	}
}
