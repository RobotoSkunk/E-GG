using UnityEngine;
using UnityEngine.SceneManagement;

public class StageTranstition : MonoBehaviour {
	public float force, time;

	private void Start() {
		GlobalVar.Audio.clip = null;
	}

	private void Update() {
		GlobalVar.shakeForce = force;
		GlobalVar.shakeTime = 1f;

		time -= Time.deltaTime;
		if (time <= 0f) {
			SceneManager.LoadScene(GlobalVar.goTo);
		}
	}
}
