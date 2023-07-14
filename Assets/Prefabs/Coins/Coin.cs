using UnityEngine;

public class Coin : MonoBehaviour {
	public Animator anim;

	private void Start() {
		anim.speed = Random.Range(0.5f, 1f);
	}

	private void OnTriggerEnter2D(Collider2D collision) {
		if (collision.CompareTag("Player")) {
			GlobalVar.coins++;
			Destroy(gameObject);
		}
	}
}
