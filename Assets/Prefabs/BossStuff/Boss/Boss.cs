using UnityEngine;

public class Boss : MonoBehaviour {
	[Header("Components")]
	public Rigidbody2D rb;
	public AudioSource aud;
	public Animator anim;
	public Enemy data;

	[Header("Properties")]
	public float speed;
	public AudioClip[] audios;

	bool died;

	private void FixedUpdate() {
		rb.velocity = new Vector2(-0.5f, rb.velocity.y);
		died = data.health <= 0f;

		if (died) {
			data.spriteRenderer.color = (Utils.Time.FixedFrameCount() % 2 == 0) ? Color.red : Color.white;
		}

		anim.SetFloat("SpeedX", Mathf.Abs(rb.velocity.x));
		anim.SetFloat("SpeedY", rb.velocity.y);
		anim.SetBool("Died", died);
	}
}
