using UnityEngine;
using UnityEngine.SceneManagement;

public class Finishline : MonoBehaviour {
	private void OnTriggerEnter2D(Collider2D collision) {
		if (collision.CompareTag("Player")) {
			switch (SceneManager.GetActiveScene().name) {
				case "Stage1": GlobalVar.goTo = "Stage2"; break;
				case "Stage2": GlobalVar.goTo = "Stage3"; break;
				case "Stage3": GlobalVar.goTo = "Credits"; break;
				default: GlobalVar.goTo = "Stage1"; break;
			}
			SceneManager.LoadScene("StageTransition");
		}
	}
}
