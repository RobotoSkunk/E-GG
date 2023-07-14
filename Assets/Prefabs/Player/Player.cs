using UnityEngine;
using System.Linq;
using System.Collections.Generic;

public class Player : MonoBehaviour {
	// Variables públicas (modificar desde Unity)
	[Header("Components")]
	public Rigidbody2D rb;
	public Animator anim, armsAnim;
	public SpriteRenderer sprRnd, armsSpr;
	public GameObject bullet;

	[Header("Sounds")]
	public AudioSource aud;
	public AudioSource bulletsAudio;
	public AudioClip[] jump, hurt, death;

	[Header("Properties")]
	public LayerMask groundLayer;
	public float bulletSpeed, immortalTimeReset;
	public float shootAngle, threshold;
	public Vector2 knockback;

	// Variables privadas
	bool onGround, hasJump = false, ignoreKnockback = false;
	float speedX = 0f, shootTime = 0f, actualLive, armsAngle = 0.5f;
	int jumpCount = 0, lastJumpCount = 0;
	Vector3 lastPos, initialPos;
	Vector2 armsVector = Vector2.right, axis;
	new Collider2D collider;

	private void Start() {
		initialPos = transform.position;
		actualLive = GlobalVar.Player.health;
		collider = GetComponent<Collider2D>();
	}

	private void OnCollisionEnter2D(Collision2D collision) {
		if (collision.collider.CompareTag("Enemy")) {
			Physics2D.IgnoreCollision(collision.collider, collider);
		} else if(collision.relativeVelocity.magnitude >= threshold && GlobalVar.immortalTime <= 0f) {
			GlobalVar.Player.health -= (collision.relativeVelocity.magnitude - threshold) / threshold;
			ignoreKnockback = true;
		}
	}

	private void Update() {
		axis = new Vector2(
			Input.GetAxisRaw("Horizontal") < 0f ? -1f : (Input.GetAxisRaw("Horizontal") > 0f ? 1f : 0f),
			Input.GetAxisRaw("Vertical") < 0f ? -1f : (Input.GetAxisRaw("Vertical") > 0f ? 1f : 0f)
		);

		// Movimiento horizontal del jugador
		speedX = axis.x * GlobalVar.Player.speed;

		#region Calcular salto
		if (Input.GetAxisRaw("Jump") > 0f && !hasJump) {
			jumpCount--;
			hasJump = true;
		} else if (Input.GetAxisRaw("Jump") == 0f) 
			hasJump = false;
		#endregion

		#region Dirección de los brazos
		if (axis.x != 0 || axis.y != 0) {
			armsVector = new Vector2(axis.x, axis.y);

			armsAngle = 90f - Mathf.Atan2(armsVector.y, Mathf.Abs(armsVector.x)) * Mathf.Rad2Deg;
		}

		armsSpr.flipX = sprRnd.flipX;
		armsAnim.SetFloat("Direction", armsAngle / 180f);
		#endregion

		#region Disparar
		if (Input.GetAxisRaw("Shoot") != 0f) {
			if (shootTime <= 0f) {
				List<double> angles = Utils.Math.LinSpace(shootAngle, GlobalVar.Player.bulletsNum);

				if (angles.Count() == 1) {
					GameObject _newInst = Instantiate(bullet);
					_newInst.transform.position = new Vector2(transform.position.x, transform.position.y) + armsVector * 0.5f + new Vector2(0f, 0.45f);
					_newInst.transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, Utils.Math.Direction(Vector2.zero, armsVector) * Mathf.Rad2Deg));
					_newInst.GetComponent<Rigidbody2D>().velocity = armsVector * bulletSpeed;
				} else {
					foreach (var a in angles) {
						float angle = Utils.Math.Direction(Vector2.zero, armsVector) * Mathf.Rad2Deg + (float)a;
						Vector2 dir = Utils.Math.GetDirVector2D(angle * Mathf.Deg2Rad);

						GameObject _newInst = Instantiate(bullet);
						_newInst.transform.position = new Vector2(transform.position.x, transform.position.y) + armsVector * 0.5f + new Vector2(0f, 0.45f);
						_newInst.transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, angle));
						_newInst.GetComponent<Rigidbody2D>().velocity = dir * bulletSpeed;
					}
				}
				bulletsAudio.Play();

				shootTime = GlobalVar.Player.shootInterval;
			}

			shootTime -= Time.deltaTime;
		} else shootTime = 0f;
		#endregion
	}

	// Todo lo que tenga que ver con físicas está en esta función para evitar bugs de FPS
 	private void FixedUpdate() {
		// Detectar colisión con el suelo
		onGround = Physics2D.OverlapBox(new Vector2(transform.position.x, transform.position.y), new Vector2(0.2f, 0.05f), 0f, groundLayer);

		#region Daño al jugador
		if (transform.position.y < GlobalVar.roomBottomLeft.y - 0.5f)
			GlobalVar.Player.health = 0f;

		if (GlobalVar.Player.health < actualLive) GlobalVar.immortalTime = immortalTimeReset;

		if (GlobalVar.immortalTime > 0f) {
			if (GlobalVar.immortalTime == immortalTimeReset) {

				if (GlobalVar.Player.health <= 0f) {
					GlobalVar.Player.health = 1f;
					GlobalVar.deaths++;

					aud.clip = death[Random.Range(0, death.Length)];
					aud.Play();

					transform.position = initialPos;
				} else {
					aud.clip = hurt[Random.Range(0, hurt.Length)];
					aud.Play();
				}

				if (!ignoreKnockback)
					rb.velocity = new Vector2((sprRnd.flipX ? 1f : -1f) * knockback.x, knockback.y);
			}

			ignoreKnockback = false;

			GlobalVar.immortalTime -= Time.fixedDeltaTime;
			sprRnd.color = armsSpr.color = (Utils.Time.FixedFrameCount() % 3 == 0 || Utils.Time.FixedFrameCount() % 4 == 0) ? Color.clear : Color.white;
		} else if (sprRnd.color != Color.white)
			sprRnd.color = armsSpr.color = Color.white;
		#endregion

		#region Físicas del salto del jugador y movimiento horizontal
		if (lastJumpCount != jumpCount && jumpCount >= 0) {
			rb.velocity = Vector2.up * GlobalVar.Player.jumpForce;

			aud.clip = jump[Random.Range(0, jump.Length)];
			aud.Play();
		}

		if (onGround && !hasJump) jumpCount = lastJumpCount = GlobalVar.Player.jumpMax;

		lastJumpCount = jumpCount;

		rb.velocity = new Vector2(Mathf.Lerp(rb.velocity.x, speedX, 0.3f), rb.velocity.y);
		#endregion

		#region Animaciones del jugador
		float _speedX = transform.position.x - lastPos.x;

		if (_speedX > 0f) sprRnd.flipX = false;
		else if (_speedX < 0f) sprRnd.flipX = true;

		anim.SetFloat("SpeedX", Mathf.Abs(_speedX));
		anim.SetFloat("SpeedY", rb.velocity.y);
		anim.SetFloat("Magnitude", Vector3.Magnitude(transform.position - lastPos));
		#endregion

		// Variables auxiliares
		lastPos = transform.position;
		actualLive = GlobalVar.Player.health;
	}
}
