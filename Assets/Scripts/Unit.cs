using UnityEngine;
using System.Collections;

public class Unit : MonoBehaviour {

	public Vector3[] poi;

	public float speed;

	NavMeshAgent navA;
	int goal;

	// Use this for initialization
	void Start () {
		navA = GetComponent<NavMeshAgent>();
		navA.speed = speed;
		goal = Random.Range(0, poi.Length);
		navA.SetDestination(poi[goal]);
		GetComponentsInChildren<MeshRenderer>()[1].material.color = new Color32((byte)Random.Range(0, 255), (byte)Random.Range(0, 255), (byte)Random.Range(0, 255), 1);
	}
	
	// Update is called once per frame
	void Update () {

		if (Vector3.Distance(transform.position, poi[goal]) < 2) {
			goal = Random.Range(0, poi.Length);
			navA.SetDestination(poi[goal]);
		}
		
	}
}
