using UnityEngine;
using System.Collections;

public class GatelingGun : MonoBehaviour {
	public bool gunFiring;
	public bool startGunFiring;
	public GameObject barrel;
	public GameObject flash;

	public float spinSpeed;
	public float fireSpeed;

	void Awake()
	{
		StartCoroutine (Flashing ());
	}

	void Update () {
		if (gunFiring) {
			barrel.transform.Rotate (0, 0, spinSpeed * Time.deltaTime);
			if(startGunFiring)
			{
				StartCoroutine (Flashing ());
				startGunFiring = false;
			}
		} else 
		{
			startGunFiring = true;
			gunFiring = false;
		}
	}

	IEnumerator Flashing()
	{
		if (gunFiring) {
			flash.transform.Rotate (0, 0, -200);
			flash.SetActive (true);
			yield return new WaitForSeconds (0.05f);
			flash.SetActive (false);
			yield return new WaitForSeconds (fireSpeed);
			StartCoroutine (Flashing ());
		}
	}
}
