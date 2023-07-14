using UnityEngine;
using UnityEngine.SceneManagement;

public class Storyboard : MonoBehaviour {
	public SpriteRenderer sprRnd;
	public AudioSource aud;
	public Camera cam;

	[Header("Panels")]
	public Sprite[] panels;
	public Color[] colors;

	int panel = 0;

	private void Update() {
		if (Input.anyKeyDown) {
			panel++;
			aud.Play();

			if (panel < colors.Length) cam.backgroundColor = colors[panel];
			if (panel < panels.Length) sprRnd.sprite = panels[panel];
			else SceneManager.LoadScene("Tutorial");
		}

		if (panel == 3) transform.position = Random.insideUnitCircle * 0.05f;
		else transform.position = Vector2.zero;
	}
}
