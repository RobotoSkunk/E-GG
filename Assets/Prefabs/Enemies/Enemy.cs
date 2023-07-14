using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {
	[Header("Components")]
	public SpriteRenderer spriteRenderer;

	[Header("Properties")]
	public Vector2 offense;
	public float defense;

	// Estas variables no deben usarse en el editor ni deben modificarse, son para que los scripts las usen, no para modificar.
	[System.NonSerialized]
	public float health = 1f;
	[System.NonSerialized]
	public GameObject _player;

	private void Awake() {
		_player = GameObject.FindGameObjectWithTag("Player");
	}

	private void OnTriggerEnter2D(Collider2D collision) {
		ExecuteTask(collision);
	}

	public void ExecuteTask(Collider2D collision) {
		if (gameObject == null) return; // Evitar bugs estúpidos

		if (collision.CompareTag("PlayerBullet")) {
			if (spriteRenderer != null) spriteRenderer.color = Color.red;

			if (defense == 0f)
				health = 0f;
			else
				health -= GlobalVar.Player.bulletDamage / defense;

			StartCoroutine(ResetColor());
		} else if (offense != Vector2.zero && collision.CompareTag("Player") && GlobalVar.immortalTime <= 0f) {
			GlobalVar.Player.health -= Random.Range(offense.x, offense.y);
		}
	}

	IEnumerator ResetColor() {
		yield return new WaitForSeconds(0.05f);

		if (spriteRenderer != null) spriteRenderer.color = Color.white;
	}
}
