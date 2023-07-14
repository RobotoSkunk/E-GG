using UnityEngine;
using System.Collections;

public class Chicharo : Enemy {
	public float lifeTime = 1f;

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
		if (collision.CompareTag("Solid") || collision.CompareTag("Player")) {
			StartCoroutine(EndLife());
		}
		ExecuteTask(collision);
	}

	IEnumerator EndLife() {
		yield return new WaitForSeconds(0.05f);

		Destroy(gameObject);
	}
}
