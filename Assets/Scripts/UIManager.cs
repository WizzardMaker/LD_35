using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using System.Linq;
using System.Collections.Generic;

public class UIManager : MonoBehaviour {

	public AudioSource music;

	public static UIManager active;

	public Disguise playerDisguise;
	public Player player;

	int _activeDisguise;
	public int activeDisguise {
		get {
			return _activeDisguise;
		}
		set {
			if (isFixed)
				return;

			if (value > 9)
				value = 0;
			if (value < 0)
				value = 9;

			_activeDisguise = value;
		}
	}

	public bool isFixed;

	public static Animator anim;

	public Color normalColor;
	public Color unableColor;
	GameObject oldActive;

	public Image chargeBar;
	public Image supicionBar;

	public Text targetName, targetWearables, targetsLeft;

	public Text disguiseText;
	public GameObject disguisesList;
	public GameObject disguiseTemplate;

	public float time;

	public GameObject[] GetChildren(GameObject parent) {
		List<GameObject> temp = new List<GameObject>();

		for (int i = 0; i < parent.transform.childCount; i++)
			temp.Add(parent.transform.GetChild(i).gameObject);

		temp.Reverse();

		return temp.ToArray();
	}

	// Use this for initialization
	void Start () {
		time = 0;

		active = this;

		player = FindObjectOfType<Player>();
		playerDisguise = player.disguise;

		UpdateDisguises(new GameObject[10]);

		activeDisguise = 1;

		anim = GetComponent<Animator>();
	}

	public Image GetHighlighter(GameObject parent) {
		Image[] ima = (from im in parent.GetComponentsInChildren<Image>(true)
					   where im.tag == "Highlight"
					   select im).ToArray();

		return ima[0];
	}

	void SetFocus(int id) {
		GameObject go = disguisesList.transform.GetChild(id).gameObject;

		Image ima = GetHighlighter(go);
        if (oldActive != null) oldActive.SetActive(false);
		ima.gameObject.SetActive(true);
		oldActive = ima.gameObject;
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
				text.text = it + ". " + go.GetComponent<DisguiseInfo>().name;

			temp.SetActive(true);

			it++;
		}

		
	}

	public void GoToMainMenu() {
		SceneManager.LoadScene(0);
	}
	public void Restart() {
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	}
	public void Exit() {
		Application.Quit();
	}

	public Canvas victoryC, gameOverC, pauseC;
	public Text supicionPoints, timePoints, timeNeeded, points;

	public bool gameOver = false, victory = false;
	public bool pause {
		get;
		set;
	}
	public static bool stopMovement;

	// Update is called once per frame
	void LateUpdate () {
		if (stopMovement) {
			Cursor.lockState = CursorLockMode.None;
		}

		if (gameOver || victory) {
			pauseC.gameObject.SetActive(false);
			stopMovement = true;
			Time.timeScale = 0;
			music.Stop();
		}
		if (Input.GetKeyDown(KeyCode.Escape))
			pause = !pause;

		if (pause && !(gameOver || victory)) {
			pauseC.gameObject.SetActive(true);
			stopMovement = true;
			Time.timeScale = 0;
		} else if(!(gameOver || victory)) {
			pauseC.gameObject.SetActive(false);
			stopMovement = false;
			Time.timeScale = 1;
		}

		if (victory) {
			victoryC.gameObject.SetActive(true);
			supicionPoints.text = (player.suspicion <= 31f ? "+100": "-"+ (int)player.suspicion) + " | Supicion of the Sniper";
			timePoints.text = (300 - (int)time > 0 ? "+" : "-") + (600 - (int)time) + " | Time needed to kill =";
			timeNeeded.text = (int)time + " seconds";
			points.text ="Points: " + ((600 - (int)time) - (int)(player.suspicion <= 31f ? -100 : player.suspicion));
		}
		if (gameOver) {
			gameOverC.gameObject.SetActive(true);

		}



		if (stopMovement)
			return;

		gameOver = player.suspicion + 1 > player.maxSupicion;
		victory = player.targetsLeft == 0;

		time += Time.deltaTime;

		if (Input.GetKeyDown(KeyCode.Tab))
			anim.SetBool("showList", !anim.GetBool("showList"));

		targetName.text = player.target.infos.name;

		string[] stuffs = (from name in player.target.infos.extras
						  where true
						  select name.name).ToArray();

		targetWearables.text = stuffs.Aggregate((current, next) => current + "\n" + next);
		if (player.targetsLeft != 1)
			targetsLeft.text = "People left: " + player.targetsLeft;
		else
			targetsLeft.text = "Last Target";

		disguiseText.text = playerDisguise.isActive ? "Current Disguise:" + playerDisguise.disguise.GetComponent<DisguiseInfo>().name : "Not Disguised";

		SetFocus(activeDisguise);

		activeDisguise -= Mathf.CeilToInt(Input.mouseScrollDelta.y);
		for (int i = 0; i < 10; i++)
			activeDisguise = Input.GetKey((i).ToString()) ? i-1 : activeDisguise;

		chargeBar.fillAmount = playerDisguise.progress / playerDisguise.neededProgress;
		supicionBar.fillAmount = player.suspicion / player.maxSupicion;

		if (!playerDisguise.canHide) {
			GetHighlighter(oldActive).color = unableColor;
		} else {
			GetHighlighter(oldActive).color = normalColor;
		}
	}
}
