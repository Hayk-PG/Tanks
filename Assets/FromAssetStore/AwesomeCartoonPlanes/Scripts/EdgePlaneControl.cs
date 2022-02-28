// Please feel to use this script freely, Noe that I am not a developer and this script does contain bugs. 

using UnityEngine;
using System.Collections;

public class EdgePlaneControl : MonoBehaviour 
	{
		public bool propRunning = true;
		public bool smokeIsOn = false;

		public GameObject propOff;
		public GameObject propOn;

		public GameObject smoke;

		public GameObject WingFR;
		public GameObject WingFL;
		public GameObject WingBR;
		public GameObject WingBL;
		public GameObject tail;

		public GameObject plane;


		public void Update () 
		{
		
		if(!propRunning)
		{
			propOff.SetActive(true);
			propOn.SetActive (false);
		}
		else
		{
			propOff.SetActive(false);
			propOn.SetActive (true);
			propOn.transform.Rotate (0, 5000 * Time.deltaTime, 0);
			plane.transform.Translate (0, 0, 40 * Time.deltaTime);
		}

		if (Input.GetKey ("up")) 
		{
			plane.transform.Rotate (200 * Time.deltaTime, 0, 0, Space.Self);
			print(WingBR.transform.localEulerAngles.x);
			if(WingBR.transform.localEulerAngles.x <= 40 || WingBR.transform.localEulerAngles.x >= 325)
			{
				WingBR.transform.Rotate (-100 * Time.deltaTime, 0, 0, Space.Self);
				WingBL.transform.Rotate (-100 * Time.deltaTime, 0, 0, Space.Self);
			}
		}
		else
		{
						if(WingBR.transform.localEulerAngles.x >= 322 || WingBR.transform.localEulerAngles.x <= 1){
								WingBR.transform.Rotate (100 * Time.deltaTime, 0, 0, Space.Self);
								WingBL.transform.Rotate (100 * Time.deltaTime, 0, 0, Space.Self);
						}

				}

				if (Input.GetKey ("down")) {
			plane.transform.Rotate (-200 * Time.deltaTime, 0, 0, Space.Self);
						print (WingBR.transform.localEulerAngles.x);
						if (WingBR.transform.localEulerAngles.x >= 322 || WingBR.transform.localEulerAngles.x <= 35) {
								WingBR.transform.Rotate (100 * Time.deltaTime, 0, 0, Space.Self);
								WingBL.transform.Rotate (100 * Time.deltaTime, 0, 0, Space.Self);
						} 
				}else {
								print ("down");
								if (WingBR.transform.localEulerAngles.x <= 40 || WingBR.transform.localEulerAngles.x >= 359) {
										WingBR.transform.Rotate (-100 * Time.deltaTime, 0, 0, Space.Self);
										WingBL.transform.Rotate (-100 * Time.deltaTime, 0, 0, Space.Self);
								}
						}
		

				if (Input.GetKey ("left")) {
			plane.transform.Rotate (0, 0, 200 * Time.deltaTime, Space.Self);
						print (WingFR.transform.localEulerAngles.x);
						if (WingFR.transform.localEulerAngles.x <= 40 || WingFR.transform.localEulerAngles.x >= 325) {
								WingFR.transform.Rotate (-100 * Time.deltaTime, 0, 0, Space.Self);
						}
						if (WingFL.transform.localEulerAngles.x >= 322 || WingFL.transform.localEulerAngles.x <= 35) {
								WingFL.transform.Rotate (100 * Time.deltaTime, 0, 0, Space.Self);
						}
				} else {
						if (WingFL.transform.localEulerAngles.x <= 40 || WingFL.transform.localEulerAngles.x >= 359) {
								WingFL.transform.Rotate (-100 * Time.deltaTime, 0, 0, Space.Self);
						}
						if (WingFR.transform.localEulerAngles.x >= 322 || WingFR.transform.localEulerAngles.x <= 1) {
								WingFR.transform.Rotate (100 * Time.deltaTime, 0, 0, Space.Self);
						}
				}
		
				if (Input.GetKey ("right")) {
			plane.transform.Rotate (0, 0, -200 * Time.deltaTime, Space.Self);
						print (WingFL.transform.localEulerAngles.x);
						if (WingFL.transform.localEulerAngles.x <= 40 || WingFL.transform.localEulerAngles.x >= 325) {
								WingFL.transform.Rotate (-100 * Time.deltaTime, 0, 0, Space.Self);
						}
						if (WingFR.transform.localEulerAngles.x >= 322 || WingFR.transform.localEulerAngles.x <= 35) {
								WingFR.transform.Rotate (100 * Time.deltaTime, 0, 0, Space.Self);
						}
				} else {
						if (WingFR.transform.localEulerAngles.x <= 40 || WingFR.transform.localEulerAngles.x >= 359) {
								WingFR.transform.Rotate (-100 * Time.deltaTime, 0, 0, Space.Self);
						}
						if (WingFL.transform.localEulerAngles.x >= 322 || WingFL.transform.localEulerAngles.x <= 1) {
								WingFL.transform.Rotate (100 * Time.deltaTime, 0, 0, Space.Self);
						}
				}

		if (Input.GetKey ("a")) 
		{
						plane.transform.Rotate (0, -200 * Time.deltaTime, 0, Space.Self);
						print(tail.transform.localEulerAngles.y);
						if(tail.transform.localEulerAngles.y >= 322 || tail.transform.localEulerAngles.y <= 35){
								tail.transform.Rotate (0, 100 * Time.deltaTime, 0, Space.Self);
						}
				}

				else
				{
						if(tail.transform.localEulerAngles.y <= 40 || tail.transform.localEulerAngles.y >= 359){
								tail.transform.Rotate (0, -100 * Time.deltaTime, 0, Space.Self);
						}
				}
		
				if (Input.GetKey ("d")) {
						plane.transform.Rotate (0, 200 * Time.deltaTime, 0, Space.Self);
						print (tail.transform.localEulerAngles.y);
						if (tail.transform.localEulerAngles.y <= 40 || tail.transform.localEulerAngles.y >= 325) {
								tail.transform.Rotate (0, -100 * Time.deltaTime, 0, Space.Self);
						}
				} else {
						if (tail.transform.localEulerAngles.y >= 322 || tail.transform.localEulerAngles.y <= 1) {
								tail.transform.Rotate (0, 100 * Time.deltaTime, 0, Space.Self);
						}
				}
				if (Input.GetKeyDown ("s")) {
						if (!smokeIsOn) {
								smoke.SetActive (true);
								smokeIsOn = true;
			} 
						else 
						{
				smoke.SetActive (false);
				smokeIsOn = false;
			}
		}
	}
}
