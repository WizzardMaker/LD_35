using UnityEngine;
using System.Collections.Generic;

public class Area : MonoBehaviour {

	public List<DisguiseType> unsuspiciousTypes;

	public int layer;

	// Use this for initialization
	void Start() {

	}

	// Update is called once per frame
	void Update() {

	}

	public void OnTriggerStay(Collider other) {

		if (other.transform.tag == "Player") {
			Player p = other.gameObject.GetComponent<Player>();

            if (!unsuspiciousTypes.Contains(p.disguise.type) && p.disguise.isActive) {
				p.AddSupicion(p.disguise.supicionLevel * Time.deltaTime, layer);
				//Debug.Log("What are you doing?");
			} else if(p.disguise.isActive) {
				p.AddSupicion(-p.disguise.supicionLevel * Time.deltaTime, layer);
				//Debug.Log("Nothing Supicious");
			}
		}
		if (other.transform.parent == null)
			return;

		if (other.transform.parent.tag == "Player") {
			Player p = other.transform.parent.gameObject.GetComponent<Player>();

			if (!unsuspiciousTypes.Contains(p.disguise.type) && p.disguise.isActive) {
				p.AddSupicion(p.disguise.supicionLevel * Time.deltaTime, layer);
				//Debug.Log("What are you doing?");
			} else if (p.disguise.isActive) {
				p.AddSupicion(-p.disguise.supicionLevel * Time.deltaTime, layer);
				//Debug.Log("Nothing Supicious");
			}
		}
	}
}
/*
	POI:
	-49 2
	17 98
	97 10
	119 -6
*/
