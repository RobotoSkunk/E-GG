using UnityEngine;
using System.Collections;

public class BombitaCaminante : Enemy {
	[Header("Components")]
	public Rigidbody2D rb;
	public Animator anim;
	public AudioSource aud;
	public CircleCollider2D col;

	[Header("Properties")]
	public float velocidad = 0.04f;
	public float direction = -1f;
	public float margenBorde = 0.2f;
	public float sight = 1.5f;

	private float maxX = float.MaxValue;
	private float minX = float.MinValue;

	/*private void Start()
	{
		damage = 0.4f;
		explotionDelay = 60;
		explotionRadius = 0.5f;
		isGonnaExplode = false;
	}*/


	private void OnCollisionStay2D(Collision2D collision) {
		Vector3 sizePlatform;
		Vector3 posPlatform;
		if (collision.collider.CompareTag("Solid"))
		{
			sizePlatform = collision.transform.localScale;
			posPlatform = collision.transform.position;

			maxX = (sizePlatform[0] / 2.0f) + posPlatform[0];
			minX =-(sizePlatform[0] / 2.0f) + posPlatform[0];
		}
	}
	
	public bool HumanOnSight() {
		return (Vector2.Distance(transform.position, _player.transform.position) <= sight);
	}

	private void FixedUpdate() {
		if (Vector2.Distance(transform.position, _player.transform.position) <= 0.8f) {
			GetComponent<SpriteRenderer>().enabled = false;
			aud.Play();
			anim.SetBool("explode", true);
			col.enabled = true;

			StartCoroutine(EndLife());
			rb.velocity = Vector2.zero;
		} else {
			#region Movimiento de Enemigo
			spriteRenderer.flipX = (direction > 0);

			if (HumanOnSight()) {
				direction = Mathf.Sign(_player.transform.position.x - transform.position.x);
				rb.velocity = new Vector3(direction * velocidad, rb.velocity.y);
			} else {
				if ((transform.position.x - margenBorde) <= minX) direction = 1;
				else if ((transform.position.x + margenBorde) >= maxX) direction = -1;

				rb.velocity = new Vector3(direction * velocidad, rb.velocity.y);
			}
			#endregion
		}

		if (health <= 0f) {
			Destroy(gameObject);
		}
	}

	IEnumerator EndLife() {
		yield return new WaitForSeconds(0.45f);

		Destroy(gameObject);
	}
}
