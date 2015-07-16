using UnityEngine;
using System.Collections;

public class LumScript : MonoBehaviour {

	// Use this for initialization

	public Light lum;
	MainScript main;
	public Vector2 minMaxIntensity;
	public GameObject plane;

	public bool isTouched = false;
	float progressTouched = 0f;
	public Color colorOff;
	public Color colorOn;


	void Start () {
		lum = GetComponent<Light>();
		main = MainScript.Instance;
	}
	
	// Update is called once per frame
	void Update () 
	{
	
		lum.intensity = Map(main.ValueHeartBeat(),0f,1f,minMaxIntensity.x,minMaxIntensity.y);



		if(isTouched)
		{
			progressTouched+=Time.deltaTime*4f;
		}
		else
		{
			progressTouched-=Time.deltaTime*4f;
		}

		progressTouched=Mathf.Clamp01(progressTouched);

		plane.GetComponent<Renderer>().material.color = Color.Lerp(colorOff,colorOn,progressTouched);


	}

	public float Map(float val, float fromMin, float fromMax, float toMin, float toMax) {
		return ((val - fromMin) / (fromMax - fromMin)) * (toMax - toMin) + toMin;
	}
}
