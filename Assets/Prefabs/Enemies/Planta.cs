using UnityEngine;

public class Planta : Enemy {
	public float sight = 5f;
	public int shootingCooldown = 60;
	public int shootingTimer = 0;
	public float bulletVelocity = 16f;
	public Vector2 damageDirection;
	public GameObject bullet;

	private void FixedUpdate() {
		if (shootingTimer > 0) shootingTimer--;

		if (Vector2.Distance(transform.position, _player.transform.position) <= sight) {
			float direction = Mathf.Sign(_player.transform.position.x - transform.position.x);
			spriteRenderer.flipX = (direction < 0);

			if (shootingTimer == 0) {
				shootingTimer = shootingCooldown;
				GameObject _newInst = Instantiate(bullet);
				_newInst.transform.position = transform.position + new Vector3(direction, 0.5f);
				_newInst.GetComponent<Rigidbody2D>().velocity = new Vector2(direction * bulletVelocity, 0f);
			}
		}

		if (health <= 0f) Destroy(gameObject);
	}
}
