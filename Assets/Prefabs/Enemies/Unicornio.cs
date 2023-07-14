using UnityEngine;

public class Unicornio : Enemy {
	[Header("Properties")]
	public float rangeOfPatrolling = 10.0f;
	public float velocidad = 0.1f;
	public float maxBombCooldown = 3f;
	public int direction = 1;
	public float sight = 5.0f;
	public GameObject shitObject;
	public Vector2 damageDirection;


	private Vector3 currentPos;
	private float bombCooldown;
	private float max;
	private float min;

	private void Start() {
		currentPos = transform.position;
		max = (rangeOfPatrolling / 2f) + currentPos.x;
		min = -(rangeOfPatrolling / 2f) + currentPos.x;
	}

	private void FixedUpdate() {
		#region movimiento
		Vector3 pasito = transform.position + new Vector3(direction * velocidad, 0.0f);
		if (pasito[0] <= min) {
			direction = 1;
		} else if (pasito[0] >= max) {
			direction = -1;
		}
		spriteRenderer.flipX = (direction > 0);
		transform.position += new Vector3(direction * velocidad, 0.0f);
		#endregion

		#region tirar bombas
		bombCooldown -= Time.fixedDeltaTime;

		if (Vector2.Distance(transform.position, _player.transform.position) < sight && bombCooldown <= 0f) {
			bombCooldown = maxBombCooldown;
			GameObject _newInstace = Instantiate(shitObject);
			_newInstace.transform.position = transform.position;
			_newInstace.GetComponent<Rigidbody2D>().AddForce(new Vector2(Random.Range(-0.35f, 0.35f), 0f));
		}
		#endregion

		if (health <= 0f) Destroy(gameObject);
	}
}

/*
using UnityEngine;

public class Unicornio : Enemy {
	[Header("Properties")]
	public float rangeOfPatrolling = 15f;
	public float velocidad = 0.1f;
	public int maxBombCooldown = 180;
	public int direction = 1;
	public float sight = 2f;
	public GameObject shitObject;
	public Vector2 damageDirection;

	private Vector3 currentPos;
	private int bombCooldown = 0;
	private float max;
	private float min;

	private void Start() {
		currentPos = this.transform.position;
		max = (rangeOfPatrolling / 2f) + currentPos[0];
		min = -(rangeOfPatrolling / 2f) + currentPos[0];
	}

	private void tirarUnaBomba() {
		bombCooldown = maxBombCooldown;
		GameObject _newInstace = Instantiate(shitObject);
		_newInstace.transform.position = this.transform.position;
	}

	private void FixedUpdate() {
		#region movimiento
		Vector3 pasito = this.transform.position + new Vector3(direction * velocidad, 0.0f);
		if (pasito[0] <= min)
		{
			direction = 1;
		}
		else if (pasito[0] >= max)
		{
			direction = -1;
		}
		spriteRenderer.flipX = (direction > 0);
		this.transform.position += new Vector3(direction * velocidad, 0.0f);
		#endregion

		#region tirar bombas
		bombCooldown = (bombCooldown <= 0) ? 0 : (bombCooldown - 1);
		if (Mathf.Abs(DistanceToPlayer()) < sight && bombCooldown == 0 && isAlive)
		{
			tirarUnaBomba();
		}
		#endregion
	}
}

 */

