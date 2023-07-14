using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

[System.Serializable]
public struct ShopArticle {
	[System.Serializable]
	public enum ArticleType {
		BULLETS_NUMBER,
		DAMAGE,
		SPEED,
		JUMP_FORCE,
		JUMPS_MAX,
		HEALTH,
		SHOOT_INTERVAL
	}

	public string name;
	public Sprite sprite;
	public ArticleType type;
	public float value;
	public int price;
}

[System.Serializable]
public struct ItemsContainer {
	public RectTransform rectTransform;
	public Image image;
	public ShopArticle article;

	Vector2 startPos;
	bool bought, selected, alreadyStarted;
	Sprite itemHover, itemNormal;
	Image selfImage;

	public void Start(ShopArticle article, Sprite itemHover, Sprite itemNormal) {
		if (!alreadyStarted) {
			startPos = rectTransform.localPosition;
			selfImage = rectTransform.gameObject.GetComponent<Image>();
			alreadyStarted = true;
			this.itemHover = itemHover;
			this.itemNormal = itemNormal;
		}

		this.article = article;
		image.sprite = article.sprite;
		image.SetNativeSize();
		bought = false;
	}

	public void Update() {
		image.enabled = !bought;

		rectTransform.localPosition = startPos + (bought ? Vector2.zero : Random.insideUnitCircle * 2f);
	}

	public void SetSelected(bool trigger) {
		selected = trigger;
		selfImage.sprite = selected ? itemHover : itemNormal;
		selfImage.SetNativeSize();
	}

	public void Buy() {
		bought = true;

		switch (article.type) {
			case ShopArticle.ArticleType.BULLETS_NUMBER: GlobalVar.Player.bulletsNum += (int)article.value; break;
			case ShopArticle.ArticleType.DAMAGE: GlobalVar.Player.bulletDamage += article.value; break;
			case ShopArticle.ArticleType.HEALTH: GlobalVar.Player.health = 1; break;
			case ShopArticle.ArticleType.JUMPS_MAX: GlobalVar.Player.jumpMax += (int)article.value; break;
			case ShopArticle.ArticleType.JUMP_FORCE: GlobalVar.Player.jumpForce += article.value; break;
			case ShopArticle.ArticleType.SHOOT_INTERVAL: GlobalVar.Player.shootInterval += article.value; break;
			case ShopArticle.ArticleType.SPEED: GlobalVar.Player.speed += article.value; break;
		}

		GlobalVar.coins -= (uint)article.price;
	}

	public bool IsBought() {
		return bought;
	}
}

public class Shop : MonoBehaviour {
	[Header("Components")]
	public AudioSource aud;
	public AudioClip showAudio, hideAudio, buyAudio, moveAudio;
	public RectTransform displayContainer;
	public Sprite itemNormal, itemHover;
	public TextMeshProUGUI articleText;

	[Header("Shop data")]
	public ShopArticle[] articles;
	public ItemsContainer[] items;

	int selected = 0;
	bool wasMoved, display = false;
	float posY;

	void GenerateShop() {
		List<ShopArticle> artList = new List<ShopArticle>();

		for (int i = 0; i < items.Length; i++) {
			ShopArticle selectedArticle;

			do {
				selectedArticle = articles[Random.Range(0, articles.Length)];
			} while (artList.Contains(selectedArticle));

			artList.Add(selectedArticle);

			items[i].Start(selectedArticle, itemHover, itemNormal);
		}
	}

	private void Start() {
		GenerateShop();
	}

	private void Update() {
		posY = Mathf.Lerp(posY, (display ? 240f : 480f * 2f), 0.45f * Utils.Time.PowerDeltaTime());
		displayContainer.position = new Vector2(displayContainer.position.x, posY);

		if (display) {
			if (Input.GetAxisRaw("Horizontal") != 0f && !wasMoved) {
				wasMoved = true;
				if (Input.GetAxisRaw("Horizontal") > 0f) selected++;
				else selected--;

				if (selected < 0) selected = items.Length - 1;
				else if (selected >= items.Length) selected = 0;

				aud.clip = moveAudio;
				aud.Play();

			} else if (Input.GetAxisRaw("Horizontal") == 0f) {
				wasMoved = false;
			}

			if (Input.GetAxisRaw("Submit") > 0f && !items[selected].IsBought() && items[selected].article.price <= GlobalVar.coins) {
				items[selected].Buy();

				aud.clip = buyAudio;
				aud.Play();
			}
		} else wasMoved = true;

		for (int i = 0; i < items.Length; i++) {
			items[i].Update(); 
			items[i].SetSelected(i == selected);
			if (i == selected)
				articleText.text = $"{items[i].article.name} (${items[i].article.price})";
		}

		if (GlobalVar.Player.health <= 0f) {
			GenerateShop();
		}
	}

	private void OnTriggerEnter2D(Collider2D collision) {
		if (collision.CompareTag("Player")) {
			display = true;
			aud.clip = showAudio;
			aud.Play();
		}
	}

	private void OnTriggerExit2D(Collider2D collision) {
		if (collision.CompareTag("Player")) {
			display = false;
			aud.clip = hideAudio;
			aud.Play();
		}
	}
}
