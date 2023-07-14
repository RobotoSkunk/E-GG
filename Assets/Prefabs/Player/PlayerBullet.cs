using UnityEngine;

public class PlayerBullet : MonoBehaviour {
	public float lifeTime;

	private void Update() {
		if (transform.position.x < GlobalVar.roomBottomLeft.x - 0.5f
		|| transform.position.x > GlobalVar.roomTopRight.x + 0.5f
		|| transform.position.y < GlobalVar.roomBottomLeft.y - 0.5f
		|| transform.position.y > GlobalVar.roomTopRight.y + 0.5f
		|| lifeTime <= 0f)
			Destroy(gameObject);

		lifeTime -= Time.deltaTime;
	}

	private void OnTriggerEnter2D(Collider2D collision) {
		if (collision.CompareTag("Solid") || collision.CompareTag("Enemy")) Destroy(gameObject);
	}
}
