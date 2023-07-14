using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;


public class movingBackground : MonoBehaviour {
	public float min, max;
    public AudioClip clip;

	float speed;
    public float TextMultiply;

    public TextMeshProUGUI PressText;

    private void Start() {
		speed = Random.Range(min, max) * (Random.value > 0.5f ? 1f : -1f);
        GlobalVar.Audio.clip = clip;
        GlobalVar.Audio.loop = true;
    }

    private void FixedUpdate() {
		transform.Rotate(Vector3.forward, speed);
	}

    private void PressEnterToStart()
    {
        if (Input.anyKey) {
            SceneManager.LoadScene("Storyboard");
        }
    }

    private void Update()
    {
        PressText.transform.position = new Vector2(0,-1.75f) + Random.insideUnitCircle * TextMultiply;
        PressEnterToStart();
    }
}
