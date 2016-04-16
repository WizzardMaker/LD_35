using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System.Collections.Generic;

public class UIManager : MonoBehaviour {

	public static UIManager active;

	public Disguise playerDisguise;
	public Player player;

	int _activeDisguise;
	public int activeDisguise {
		get {
			return _activeDisguise;
		}
		set {
			if (value > 9)
				value = 0;
			if (value < 0)
				value = 9;

			_activeDisguise = value;
		}
	}



	GameObject oldActive;

	public Image ChargeBar;

	public Text disguiseText;
	public GameObject disguisesList;
	public GameObject disguiseTemplate;

	GameObject[] GetChildren(GameObject parent) {
		List<GameObject> temp = new List<GameObject>();

		for (int i = 0; i < parent.transform.childCount; i++)
			temp.Add(parent.transform.GetChild(i).gameObject);

		temp.Reverse();

		return temp.ToArray();
	}

	// Use this for initialization
	void Start () {
		active = this;

		player = FindObjectOfType<Player>();
		playerDisguise = player.disguise;

		UpdateDisguises(new GameObject[10]);

		activeDisguise = 1;
	}

	void SetFocus(int id) {
		GameObject go = disguisesList.transform.GetChild(id).gameObject;

		Image[] ima = (from im in go.GetComponentsInChildren<Image>(true)
					   where im.tag == "Highlight"
					   select im).ToArray();

		if (oldActive != null) oldActive.SetActive(false);
		ima[0].gameObject.SetActive(true);
		oldActive = ima[0].gameObject;
	}

	public void UpdateDisguises(GameObject[] dis) {
		int it = 1;

		for(int i = 0; i < disguisesList.transform.childCount; i++) {
			Destroy(disguisesList.transform.GetChild(i).gameObject);
		}

		foreach(GameObject go in dis) {

			GameObject temp = Instantiate(disguiseTemplate);
			Text text = temp.GetComponentInChildren<Text>();
			temp.transform.SetParent(disguisesList.transform, false);
			if (go == null)
				text.text = it + ".";
			else
				text.text = it + ". " + go.name;

			temp.SetActive(true);

			it++;
		}

		
	}

	// Update is called once per frame
	void LateUpdate () {
		disguiseText.text = playerDisguise.isActive ? "Current Disguise:" + playerDisguise.disguise.name : "Not Disguised";

		SetFocus(activeDisguise);

		activeDisguise -= Mathf.CeilToInt(Input.mouseScrollDelta.y);
		for (int i = 0; i < 10; i++)
			activeDisguise = Input.GetKey((i).ToString()) ? i-1 : activeDisguise;

		ChargeBar.fillAmount = playerDisguise.progress / playerDisguise.neededProgress;
	}
}
