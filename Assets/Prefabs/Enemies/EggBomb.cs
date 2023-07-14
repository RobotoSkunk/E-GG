using UnityEngine;
using System.Collections;

public class EggBomb : MonoBehaviour {
	public Animator anim;
	public AudioSource aud;
	public CircleCollider2D col;
	public SpriteRenderer spr;
	public Rigidbody2D rb;

	bool gonnaBOOM = false;
	Collider2D _collider;

	private void Start() {
		_collider = GetComponent<Collider2D>();
	}

	private void OnCollisionEnter2D(Collision2D collision) {
		if ((collision.collider.CompareTag("Solid") || collision.collider.CompareTag("Player")) && !gonnaBOOM) {
			gonnaBOOM = true;
			StartCoroutine(BOOM_BOOM_MOTHERFUCKERS());
		} else if(collision.collider.CompareTag("Enemy")) {
			Physics2D.IgnoreCollision(_collider, collision.collider);
		}
	}

	IEnumerator BOOM_BOOM_MOTHERFUCKERS() {
		yield return new WaitForSeconds(0.5f);

		aud.Play();
		anim.SetBool("explode", true);
		col.enabled = true;
		spr.enabled = false;
		rb.velocity = Vector2.zero;
		rb.gravityScale = 0;
		rb.SetRotation(0f);

		yield return new WaitForSeconds(0.45f);

		Destroy(gameObject);
	}
}
