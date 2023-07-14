using UnityEngine;
using System.Collections;

public class UnicornShit : Enemy {
	private void FixedUpdate() {
		if (health <= 0f) Destroy(gameObject);
	}

	private void OnCollisionEnter2D(Collision2D collision) {
		if (collision.collider.CompareTag("Player") && GlobalVar.immortalTime <= 0f) {
			StartCoroutine(Ugh());
		}
		ExecuteTask(collision.collider);
	}

	IEnumerator Ugh() {
		yield return new WaitForSeconds(0.05f);

		Destroy(gameObject);
	}
}
