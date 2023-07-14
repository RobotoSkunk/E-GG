using UnityEngine;
using UnityEngine.SceneManagement;

public class NGIntro : MonoBehaviour {
	public GameObject tankman, title;
	public GameObject[] parallaxObjects;
	public float[] parallaxValue;

	public float timer, tankmanTime, titleTime, splashTime;
	public string menuScene;

	private void FixedUpdate() {
		for (int i = 0; i < parallaxObjects.Length; i++) {
			if (i < parallaxValue.Length) {
				parallaxObjects[i].transform.position += new Vector3(parallaxValue[i] * Time.fixedDeltaTime, 0f, 0f);
			}
		}

		tankman.transform.position = new Vector3(Mathf.Lerp(tankman.transform.position.x, 0f, Time.fixedDeltaTime / tankmanTime), -2.03125f, 0f);

		if (timer <= 0f) title.transform.position = new Vector3(0f, Mathf.Lerp(title.transform.position.y, 0f, Time.fixedDeltaTime / titleTime), 0f);
		if (splashTime <= 0f) SceneManager.LoadScene(menuScene);

		timer -= Time.fixedDeltaTime;
		splashTime -= Time.fixedDeltaTime;
	}
}
