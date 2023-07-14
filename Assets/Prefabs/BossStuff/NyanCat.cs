using UnityEngine;

public class NyanCat : MonoBehaviour {
	public Vector2 roomSize, speedRange, restartTime;
	public SpriteRenderer sprRainbow;
	public float colorSpeed;

	float speed, time;

	void ResetCat() {
		transform.position = new Vector2(-roomSize.x / 2f, Random.Range(-roomSize.y / 2f, roomSize.y / 2f));
		speed = Random.Range(speedRange.x, speedRange.y);
		time = Random.Range(restartTime.x, restartTime.y);
		sprRainbow.color = Color.white;
	}

	private void Start() {
		ResetCat();
	}

	private void Update() {
		if (transform.position.x > roomSize.x / 2f) {
			sprRainbow.color -= new Color(0, 0, 0, colorSpeed) * Utils.Time.PowerDeltaTime();

			if (sprRainbow.color.a <= 0f) {
				time -= Time.deltaTime;
				
				if (time <= 0f) {
					ResetCat();
				}
			}
		}

		transform.position += new Vector3(speed, 0) * Utils.Time.PowerDeltaTime();
	}
}
