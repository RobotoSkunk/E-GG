using UnityEngine;
using TMPro;

public class credits : MonoBehaviour {
	public TextMeshProUGUI creditsText;
	public float velocity;

	// Update is called once per frame
	void Update() {
		creditsText.transform.position += Vector3.up * velocity * Utils.Time.PowerDeltaTime();
	}
}
