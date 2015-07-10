using UnityEngine;
using System.Collections;

public class MainScript : MonoBehaviour {

	public static MainScript Instance;
	public GameObject prefabFish;

	[Range(0f,1f)]
	public float globalOutlineWidth = 0f;
	float pulseProgress = 0f;
	public float pulse = 0f;
	public float durationCalmPulse = 2f;
	public AnimationCurve curvePulse;

	public bool autoHeartBeat = true;
	public float bpm = 80f;
	public float currentProgressAutoHeartBeat = 0f;



	public bool playerConnected = false;
	bool wasPlayerConnected = false;

	float securityProgressON = 0f;
	float securityProgressOFF = 0f;


	public float securityTimeON = 5f;
	public float securityTimeOFF = 5f;
	public GameObject currentFish = null;


	bool isRemoving = false;
	bool isConnecting = false;
	// Use this for initialization
	void Awake()
	{
		Instance = this;
	
	}

	void Start () 
	{
	
	
	}
	

	void Update () {
		if(Input.GetKeyDown(KeyCode.R))
			Application.LoadLevel(0);

		if(Input.GetKeyDown(KeyCode.Escape))
			Application.Quit ();

		if(Input.GetKeyDown(KeyCode.Space))
			CreateFish ();


		Shader.SetGlobalFloat("_GlobalOutlineWidth",globalOutlineWidth);





		pulseProgress-=Time.deltaTime/durationCalmPulse;
		pulseProgress = Mathf.Clamp01(pulseProgress);
		pulse = curvePulse.Evaluate(pulseProgress);

		if(Input.GetMouseButtonDown(0))
		{
			HeartBeat();
		}

		if(autoHeartBeat)
		{
			AutoHeartBeat();
		}


		if(wasPlayerConnected!=playerConnected)
		{
			if(playerConnected && isConnecting==false)
			{
				
				isConnecting = true;
				securityProgressON = securityTimeON;
				
			}
			
			

			if(!playerConnected&& isRemoving==false)
			{
				isRemoving = true;
				securityProgressOFF = securityTimeOFF;
			}
		}
		else
		{
			isConnecting = false;
			isRemoving=false;
		}


		if(isRemoving && playerConnected == false && wasPlayerConnected==true)
		{
			securityProgressOFF=Mathf.Clamp(securityProgressOFF-Time.deltaTime,0f,securityTimeOFF);
			if(securityProgressOFF==0f)
			{
				wasPlayerConnected = false;
				currentFish = null;
				isRemoving = false;
			}

		}

		if(isConnecting && playerConnected == true && wasPlayerConnected == false)
		{
			securityProgressON=Mathf.Clamp(securityProgressON-Time.deltaTime,0f,securityTimeON);
			if(securityProgressON==0f)
			{
				wasPlayerConnected = true;
				CreateFish();
				isConnecting = false;
			}
		}










		
	}

	public void ConnectionPlayer(bool connect)
	{
		playerConnected = connect;
	}

	public void SetBPM(float _bpm)
	{
		bpm=_bpm;
	}

	void AutoHeartBeat()
	{

		bpm = Mathf.Max (0f,bpm);
		currentProgressAutoHeartBeat = Mathf.Min (2f,currentProgressAutoHeartBeat);
		durationCalmPulse = 60f/bpm;



		currentProgressAutoHeartBeat-=Time.deltaTime;

		if(currentProgressAutoHeartBeat<=0f)
		{
			currentProgressAutoHeartBeat = 60f/bpm;
			HeartBeat();
		}
	}

	public float Map(float val, float fromMin, float fromMax, float toMin, float toMax) {
		return ((val - fromMin) / (fromMax - fromMin)) * (toMax - toMin) + toMin;
	}

	float Modulo(float a,float b)
	{
		return a - b * Mathf.Floor(a / b);
	}

	void CreateFish()
	{
		currentFish = Instantiate (prefabFish,Vector3.zero,Quaternion.identity) as GameObject;
	}

	public void HeartBeat()
	{
		pulseProgress = 1f;
		pulse = curvePulse.Evaluate(pulseProgress);
	}

	public void AffectProperty1()
	{
		if(currentFish!=null)
			currentFish.GetComponent<FishScript>().AffectProperty1();
	}
	
	public void AffectProperty2()
	{
		if(currentFish!=null)
			currentFish.GetComponent<FishScript>().AffectProperty2();
	}

	public void AffectProperty3()
	{
		if(currentFish!=null)
			currentFish.GetComponent<FishScript>().AffectProperty3();
	}
}
