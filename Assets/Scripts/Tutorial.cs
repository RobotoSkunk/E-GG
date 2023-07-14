using UnityEngine;
using UnityEngine.SceneManagement;

public class Tutorial : MonoBehaviour {
	private void Update() {
		if (Input.anyKeyDown) {
			SceneManager.LoadScene("StageTransition");
			GlobalVar.goTo = "Stage1";
		}
	}
}
