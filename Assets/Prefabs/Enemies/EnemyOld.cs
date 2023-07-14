using UnityEngine;
using System.Threading.Tasks;

// Obsolete
public class EnemyOld : MonoBehaviour {
	[Header("Components")]
	public SpriteRenderer spriteRenderer;
	public BoxCollider2D boxCollider;

	[Header("Properties")]
	public Vector2 offense;
	public float defense;
	private int explodeTimer = 0;
	public int explotionDelay = 60;
	public float explotionRadius = 0.5f;
	public float damage = 0.4f;

	[Header("Shared variables")]
	public float health = 1f;
	public bool isGonnaExplode = false;
	public bool isAlive = true;


	public void reviveEnemy()
    {
		this.isAlive = true;
		this.boxCollider.enabled = true;
        this.spriteRenderer.enabled = true;
    }
	public void killEnemy()
    {
		this.isAlive = false;
        this.boxCollider.enabled = false;
        this.spriteRenderer.enabled = false;
		Destroy(gameObject);
    }

	public void Explode(bool destroy = false)
	{
		this.explodeTimer += 1;
		this.spriteRenderer.color = (this.explodeTimer % 2 == 0) ? Color.blue : Color.yellow;

		if (explodeTimer == explotionDelay)
		{
			if (Mathf.Abs(DistanceToPlayer(0)) <= explotionRadius &&
				Mathf.Abs(DistanceToPlayer(1)) <= 0.5f)
			{
				GlobalVar.Player.health -= damage;
			}

            if (destroy)
            {
				Destroy(gameObject);
            }
            else
            {
                this.killEnemy();
            }
			explodeTimer = 0;
			isGonnaExplode = false;
		}
	}

    public float DistanceToPlayer(int axis = 0)
    {
        //Te devuelve la distancia con signo del jugador en el eje "axis"
        //Si es negativa el jugador esta a la izquierda y viceversa
        Vector3 currentPos = this.transform.position;
        Vector3 playerPos = GameObject.Find("player").transform.position;
        return (playerPos[axis] - currentPos[axis]);
    }

    private async void OnTriggerEnter2D(Collider2D collision) {
		if (collision.CompareTag("PlayerBullet")) {
			spriteRenderer.color = Color.red;

			if (defense == 0f)
				health = 0f;
			else
				health -= GlobalVar.Player.bulletDamage / defense;

			await Task.Delay(50);

			spriteRenderer.color = Color.white;
		} else if (offense != Vector2.zero && collision.CompareTag("Player") && GlobalVar.immortalTime <= 0f) {
			GlobalVar.Player.health -= Random.Range(offense.x, offense.y);
		}

		if (health <= 0.0f)
        {
			killEnemy();
        }

	}
}
