using UnityEngine;

public class BossFight : MonoBehaviour {
	[Header("Necessary data")]
	public Enemy bossData;

	[Header("Decoration")]
	public int starsMin, starsMax;
	public GameObject star;

	private void Start() {
		int tmp = Random.Range(starsMin, starsMax);

		for (int i = 0; i < tmp; i++) Instantiate(star);
	}

	
}
