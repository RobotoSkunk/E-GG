using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Init : MonoBehaviour {
	public AudioSource aud;
	public float splashTime, soundTime;
	public string menuScene;

	private void Start() {
		StartCoroutine(BoomSound());
		StartCoroutine(GoToScene());
	}

	IEnumerator BoomSound() {
		yield return new WaitForSeconds(soundTime);

		aud.Play();
	}

	IEnumerator GoToScene() {
		yield return new WaitForSeconds(splashTime);

		SceneManager.LoadScene(menuScene);
	}
}
