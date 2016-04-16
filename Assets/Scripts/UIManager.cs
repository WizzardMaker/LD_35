using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIManager : MonoBehaviour {

	public static UIManager active;

	public Disguise playerDisguise;
	public Player player;

	public Text disguiseText;

	// Use this for initialization
	void Start () {
		active = this;

		player = FindObjectOfType<Player>();
		playerDisguise = player.disguise;
	}
	
	// Update is called once per frame
	void Update () {
		disguiseText.text = playerDisguise.isActive ? "Current Disguise:" + playerDisguise.disguise.name : "Not Disguised";
	}
}
