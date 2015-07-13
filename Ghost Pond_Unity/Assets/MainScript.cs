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

	public ProgressModifProperty property1;
	public ProgressModifProperty property2;
	public ProgressModifProperty property3;

	float timeSinceLastHeartBeat = 0f;

	public int lastCode = 0;






	void Awake()
	{
		Instance = this;
	
	}

	void Start () 
	{
	
		property1.Init();
		property2.Init();
		property3.Init();
	}
	

	void Update () {
		if(Input.GetKeyDown(KeyCode.R))
			Application.LoadLevel(0);

		if(Input.GetKeyDown(KeyCode.Escape))
			Application.Quit ();

		if(Input.GetKeyDown(KeyCode.Space))
			CreateFish ();


		if(Input.GetKey (KeyCode.Keypad1))
		{
			property1.Update();
			AffectProperty1();
		}

		if(Input.GetKey (KeyCode.Keypad2))
		{
			property2.Update();
			AffectProperty2();
		}

		if(Input.GetKey (KeyCode.Keypad3))
		{
			property3.Update();
			AffectProperty3();
		}



		Shader.SetGlobalFloat("_GlobalOutlineWidth",globalOutlineWidth);





		pulseProgress-=Time.deltaTime/durationCalmPulse;
		pulseProgress = Mathf.Clamp01(pulseProgress);
		pulse = curvePulse.Evaluate(pulseProgress);

		if(Input.GetMouseButtonDown(0))
		{
			HeartBeat(true);
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

		bpm = Mathf.Max (0f,bpm);
		durationCalmPulse = 60f/bpm;

		timeSinceLastHeartBeat+=Time.deltaTime;

		if(timeSinceLastHeartBeat>2f)
		{
			autoHeartBeat = true;
		}
		else
		{
			autoHeartBeat = false;
		}


		ActivateStuff();

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





		currentProgressAutoHeartBeat = Mathf.Min (2f,currentProgressAutoHeartBeat);




		currentProgressAutoHeartBeat-=Time.deltaTime;

		if(currentProgressAutoHeartBeat<=0f)
		{
			currentProgressAutoHeartBeat = 60f/bpm;
			HeartBeat(false);
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




		currentFish = Instantiate (prefabFish,Vector3.right*17f,Quaternion.identity) as GameObject;

		/*AffectProperty1();

		AffectProperty2();

		AffectProperty3();*/

		property1.Init ();
		property2.Init ();
		property3.Init ();

		currentFish.GetComponent<FishScript>().SetInitPropertiesValues(property1.GetValue(),property2.GetValue(),property3.GetValue());
	}

	public void HeartBeat(bool isTrueHeartBeat)
	{
		pulseProgress = 1f;
		pulse = curvePulse.Evaluate(pulseProgress);


		if(isTrueHeartBeat)
		timeSinceLastHeartBeat = 0f;
	}

	public void AffectProperty1()
	{
		if(currentFish!=null)
		{
			property1.Update();
			currentFish.GetComponent<FishScript>().AffectProperty1(property1.GetValue());
		}
			
	}
	
	public void AffectProperty2()
	{
		if(currentFish!=null)
		{
			property2.Update();
			currentFish.GetComponent<FishScript>().AffectProperty2(property2.GetValue());
		}
	}

	public void AffectProperty3()
	{
		if(currentFish!=null)
		{
			property3.Update();
			currentFish.GetComponent<FishScript>().AffectProperty3(property3.GetValue());
		}
	}

	[System.Serializable]
	public class ProgressModifProperty
	{
		float progress;
		public float durationLoop;
		public float valueMin;
		public float valueMax;
		public int freq;
		public int freq2;
		public int freq3;
		int phi;

		public void Init()
		{
			progress = Random.value;
			phi = Random.Range (0,360);
		}


		public void Update()
		{
			progress+=Time.deltaTime/durationLoop;
		}

		public float GetValue()
		{
			return(Map(GetLissajouValue(progress),0f,1f,valueMin,valueMax));
		}

		float GetLissajouValue(float progressValue)
		{
			return(	(Mathf.Sin (progressValue*Mathf.PI*2f*freq+Radians (phi))*0.5f+0.5f)*(Mathf.Cos (progressValue*Mathf.PI*2f*freq2)*0.5f+0.5f)*(Mathf.Cos (progressValue*Mathf.PI*2f*freq3)*0.5f+0.5f)			);
		}

		float Radians(float value)
		{
			float val = Mathf.PI*value/180;
			return(val);
		}


		float Map(float val, float fromMin, float fromMax, float toMin, float toMax) {
			return ((val - fromMin) / (fromMax - fromMin)) * (toMax - toMin) + toMin;
		}


	}


	public void GetMessageFromArduino(int code)
	{

		lastCode = code;
		//Debug.Log ("int: "+code);

	}

	void ActivateStuff()
	{
		string _code = GetIntBinaryString(lastCode);

		//Debug.Log ("binary: "+_code);

		if(_code.Length>=3)
		{
			if(_code[0].ToString ()=="1")
			{
				AffectProperty1();
				CheckToCreateFish();


			}
			
			if(_code[1].ToString ()=="1")
			{
				AffectProperty2();
				CheckToCreateFish();
			}
			
			if(_code[2].ToString ()=="1")
			{
				AffectProperty3();
				CheckToCreateFish();
			}
		}



	}

	void CheckToCreateFish()
	{
		if(currentFish == null)
		{

		}
	}

	static string GetIntBinaryString(int n)
	{
		char[] b = new char[3];
		int pos = 2;
		int i = 0;
		
		while (i < 3)
		{
			if ((n & (1 << i)) != 0)
			{
				b[pos] = '1';
			}
			else
			{
				b[pos] = '0';
			}
			pos--;
			i++;
		}
		return new string(b);
	}


}
