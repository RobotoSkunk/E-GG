using UnityEngine;

public class Killzone : MonoBehaviour {
	public Vector2 damage;

	private void OnTriggerStay2D(Collider2D collision) {
		if (collision.CompareTag("Player") && GlobalVar.immortalTime <= 0f) {
			GlobalVar.Player.health -= Random.Range(damage.x, damage.y);
		}
	}
}

