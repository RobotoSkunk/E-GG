using UnityEngine;

public class BossStar : MonoBehaviour {
	public Vector2 roomSize, speedRange;
	public SpriteRenderer spr;
	public Animator anim;
	public float minAlpha;

	float speed;

	private void ResetStar() {
		float tmp = Random.Range(0f, 1f);

		speed = speedRange.x + (speedRange.y - speedRange.x) * tmp;
		spr.color = new Color(1f, 1f, 1f, minAlpha + Random.Range(0f, tmp * (1 - minAlpha)));
		anim.SetFloat("speed", Random.Range(0.5f, 1f));
	}

	private void Start() {
		ResetStar();
		transform.position = new Vector2(
			Random.Range(-roomSize.x / 2f, roomSize.x / 2f),
			Random.Range(-roomSize.y / 2f, roomSize.y / 2f)
		);
	}

	private void Update() {
		if (transform.position.x < -roomSize.x / 2f) {
			ResetStar();

			transform.position = new Vector2(
				roomSize.x / 2f,
				Random.Range(-roomSize.y / 2f, roomSize.y / 2f)
			);
		}

		transform.position -= new Vector3(speed, 0) * Utils.Time.PowerDeltaTime();
	}
}
