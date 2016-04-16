using UnityEngine;
using System.Collections.Generic;

public class SniperAI : MonoBehaviour {

	public List<GameObject> suspects;

	public int suspectAmount;

	public float scanTarget;
	float time, timeAdded;
	public float lookSpeed;

	public Vector3 target;
	public Transform laser;

	// Use this for initialization
	void Start () {
		foreach(Unit u in FindObjectsOfType<Unit>()) {
			if (suspectAmount < suspects.Count)
				break;
				suspects.Add(u.gameObject);
			
		}

		suspects.Add(FindObjectOfType<Player>().gameObject);

		time = scanTarget + Time.time;
	}

	int i = 0;

	public static bool isInView(GameObject target) {
		Camera cam = GameObject.FindGameObjectWithTag("SniperCam").GetComponent<Camera>();
		Plane[] planes = GeometryUtility.CalculateFrustumPlanes(cam);

		return GeometryUtility.TestPlanesAABB(planes, target.GetComponent<Collider>().bounds);
	}

	// Update is called once per frame
	void Update () {
		timeAdded += Time.deltaTime;

		if(timeAdded > time) {
			time = scanTarget + Time.time;
			if (suspects[i].GetComponent<Player>() != null) {
				if (suspects[i].GetComponent<Disguise>().isActive)
					i++;
			}
			i++;
		}

		if (i > suspects.Count - 1)
			i = 0;

		if (suspects[i].GetComponent<Player>() != null) {
			if (suspects[i].GetComponent<Disguise>().isActive)
				i++;
		}
		if (i > suspects.Count - 1)
			i = 0;

		target = suspects[i].transform.position;

		Quaternion neededRotation = Quaternion.LookRotation(target - laser.position);
		laser.rotation = Quaternion.Slerp(laser.rotation, neededRotation, Time.deltaTime * lookSpeed);
	}
}
