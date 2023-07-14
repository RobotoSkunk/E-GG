using TMPro;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public struct Parallax {
	public GameObject victim;
	public Vector2 value;
	public Vector2 startPos;
}

public class GameCamera : MonoBehaviour {
	[Header("GUI elements")]
	public Slider liveSlider;
	public TextMeshProUGUI deaths, score, coins;
	public GameObject liveContainer;

	[Header("Components")]
	public Camera cam;
	public GameObject objective, roomSize;

	[Header("Properties")]
	public Vector2 speed;
	public float distanceFromCamera, distanceSpeed;

	[Header("Parallaxing components")]
	public Parallax[] parallax;

	Vector3 startPos, setPos;
	Vector2 nextPos, objectiveLastPos, liveContainerPos;
	float extraX = 0f, extraDelay = 0f;

	private void Start() {
		if (objective != null) 
			nextPos = objective.transform.position;

		for (int i = 0; i < parallax.Length; i++) 
			parallax[i].startPos = parallax[i].victim.transform.position;

		startPos = transform.position;
		liveContainerPos = liveContainer.transform.position;

		if (roomSize != null) {
			GlobalVar.roomBottomLeft = new Vector2(
				roomSize.transform.position.x - roomSize.transform.localScale.x / 2f,
				roomSize.transform.position.y - roomSize.transform.localScale.y / 2f
			);
			GlobalVar.roomTopRight = new Vector2(
				roomSize.transform.position.x + roomSize.transform.localScale.x / 2f,
				roomSize.transform.position.y + roomSize.transform.localScale.y / 2f
			);
		} else {
			GlobalVar.roomBottomLeft = new Vector2(-6f, -4f);
			GlobalVar.roomTopRight = new Vector2(6f, 4f);
		}
	}

	private void Update() {
		liveSlider.value = GlobalVar.Player.health;
		deaths.text = $"Deaths: {GlobalVar.deaths}";
		score.text = $"Score: {GlobalVar.score}";
		coins.text = $"Coins: {GlobalVar.coins}";

		liveContainer.transform.position = liveContainerPos + (GlobalVar.Player.health <= 0.35f ? Random.insideUnitCircle * 5f : Vector2.zero);
	}

	private void FixedUpdate() {
		Vector2 camSize = new Vector2(cam.orthographicSize * 2f * cam.aspect, cam.orthographicSize * 2f),
			camMiddle = new Vector2((camSize.x / 2f), (camSize.y / 2f)),
			shake = Vector2.zero;

		if (objective != null) {
			nextPos = objective.transform.position;


			extraX = nextPos.x - objectiveLastPos.x > 0f ? distanceFromCamera : (nextPos.x - objectiveLastPos.x < 0f ? -distanceFromCamera : extraX);
			extraDelay = Mathf.Lerp(extraDelay, extraX, distanceSpeed);
			nextPos += new Vector2(extraDelay, 0);

			if (roomSize != null) {
				Vector2 bottomLeft = new Vector2(
					roomSize.transform.position.x - roomSize.transform.localScale.x / 2f + camMiddle.x,
					roomSize.transform.position.y - roomSize.transform.localScale.y / 2f + camMiddle.y
				),
				topRight = new Vector2(
					roomSize.transform.position.x + roomSize.transform.localScale.x / 2f - camMiddle.x,
					roomSize.transform.position.y + roomSize.transform.localScale.y / 2f - camMiddle.y
				);

				nextPos = new Vector2(
					(topRight.x <= bottomLeft.x ? Mathf.Lerp(bottomLeft.x, topRight.x, 0.5f) : Mathf.Clamp(nextPos.x, bottomLeft.x, topRight.x)),
					(topRight.y <= bottomLeft.y ? Mathf.Lerp(bottomLeft.y, topRight.y, 0.5f) : Mathf.Clamp(nextPos.y, bottomLeft.y, topRight.y))
				);
			}

			objectiveLastPos = objective.transform.position;
		}

		if (GlobalVar.shakeTime > 0f) {
			shake = Random.insideUnitCircle * GlobalVar.shakeForce;
			GlobalVar.shakeTime -= Time.fixedDeltaTime;
		}

		setPos = new Vector3(
			Mathf.Lerp(setPos.x, nextPos.x, speed.x),
			Mathf.Lerp(setPos.y, nextPos.y, speed.y),
			-10f
		);

		transform.position = setPos + new Vector3(shake.x, shake.y);

		Vector2 distanceFromStart = transform.position - startPos;

		for (int i = 0; i < parallax.Length; i++) {
			parallax[i].victim.transform.position = parallax[i].startPos + distanceFromStart * parallax[i].value;
		}
	}
}
