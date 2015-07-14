using UnityEngine;
using System.Collections;

public class FishScript : MonoBehaviour {

	// Use this for initialization

	GameObject[] bodyParts = new GameObject[0];
	Vector3[] bodyPartsPositions = new Vector3[0];

	public int numberBodyParts = 50;
	public GameObject prefabBodyPart;

	float tailLenght = 0f;
	public float nextTailLenght = 1f;




	float[] randomValues = new float[0];
	int[] randomSens = new int[0];
	bool started = false;
	MainScript main;








	float valueVirage = 0f;
	float nextValueVirage = 0f;
	 float speedVirage = 0f;
	int sensVirage = 1;
	 float maxAddValueVirage = 1f;
	Vector2 minMaxSpeedVirage;



	public float radiusMotionBodyParts = 1f;


	float speedMotion = 1f;
	public ProgressValue radiusMotion = new ProgressValue();
	public float progressCircle;
	public ProgressValue speedProgressCircle = new ProgressValue();

	public Color mainColorA;
	public Color mainColorB;
	public Color outlineColorA;
	public Color outlineColorB;


	float headSize = 0f;
	public float nextHeadSize = 2f;

	float outlineWidth = 0f;
	public float nextOutlineWidth;

	float timeLife = 0f;

	public Vector3 centerPond = new Vector3(17f,0f,0f);



	[Range(0f,1f)]
	public float progressChangementPond = 0f;

	[Range(0f,1f)]
	public float progressCentreLittlePond = 0f;




	bool goCenter = false;
	bool goBigPond = false;

	float transitionFromProgressCircle = 0f;

	float transitionProgressCircle = 0f;

	public AnimationCurve curveGoCenter;


	float timerBeforeGoBigPond = 5f;

	bool isInBigPond = false;

	public bool dying = false;
	bool dead = false;
	float finalLife = 1f;
	public AnimationCurve curveToNewPond;


	public float durationBeforeGoBigPond = 2.5f;
	public float durationGoCenterLittlePond = 2.5f;


	public AnimationCurve curveSize;




	[System.Serializable]
	public class ProgressValue
	{
		public float value = 0f;
		public float prevValue = 0f;
		public float nextValue = 0f;

		public float progress = 0f;


		public Vector2 minMaxNextValue = new Vector2(0f,10f);

		public float durationProgress = 1f;
		public Vector2 minMaxDurationProgress = new Vector2(0f,10f);
		public AnimationCurve curveProgress;

		public bool useRandomSigne = false;
		
		//public float sens = 1f;

		public void Init()
		{
			value = Random.Range (minMaxNextValue.x,minMaxNextValue.y);
			ChangeValues();
		}

		public void Update()
		{

			progress=Mathf.Clamp01(progress+Time.deltaTime/durationProgress);

			value = Mathf.Lerp(prevValue,nextValue,curveProgress.Evaluate(progress));

			if(progress==1f)
			{
				ChangeValues();
			}


				


		}


		public void ChangeValues()
		{
			prevValue = nextValue;
			nextValue =  Random.Range (minMaxNextValue.x,minMaxNextValue.y);

			if(useRandomSigne && Random.value>0.5f)
			{
				nextValue*=-1f;
			}


			progress = 0f;

			durationProgress = Random.Range(minMaxDurationProgress.x,minMaxDurationProgress.y);




			//sens = value>nextValue?-1f:1f;
		}

	}




	void Start () 
	{
		main = MainScript.Instance;
		InitValues();

		RandomColors();

		ChangeValuesVirage();
		GenerateBody();

	}

	void RandomColors()
	{
		mainColorA = new Color(Random.value,Random.value,Random.value);
		mainColorB = new Color(Random.value,Random.value,Random.value);
		outlineColorA = new Color(Random.value,Random.value,Random.value);
		outlineColorB = new Color(Random.value,Random.value,Random.value);
	}


	void InitValues()
	{



		radiusMotion.Init();
		speedProgressCircle.Init();








		//nextHeadSize = Random.Range (1f,4f);
		//nextTailLenght= Random.Range (0.2f,1.2f);
		//tailLenght= nextTailLenght;
		progressCircle = Random.value;

		radiusMotionBodyParts = Random.Range (0f,1.2f);



	}

	public void SetInitPropertiesValues(float p1, float p2, float p3)
	{
		nextTailLenght = p1;
		tailLenght = p1;

		nextOutlineWidth = p2;
		outlineWidth = p2;

		nextHeadSize = p3;
		headSize = 0f;
	}

	void ChangeValuesVirage()
	{
		float addValue = 0f;
		while(addValue ==0f)
		{
			addValue = Random.Range (-maxAddValueVirage,maxAddValueVirage);
		}

		nextValueVirage+=addValue;

		sensVirage = nextValueVirage>valueVirage?1:-1;
		speedVirage = Random.Range (minMaxSpeedVirage.x,minMaxSpeedVirage.y);

	}



	void GenerateBody()
	{


		bodyParts = new GameObject[numberBodyParts];
		bodyPartsPositions = new Vector3[numberBodyParts];
		randomValues = new float[numberBodyParts];
		randomSens = new int[numberBodyParts];





		for(int i = 0; i<bodyParts.Length;i++)
		{
			float progress = main.Map ((float)i,0f,(float)(bodyParts.Length-1),1f,0f);


			GameObject bodyPart = Instantiate(prefabBodyPart,transform.position,Quaternion.identity) as GameObject;
			//bodyPart.transform.eulerAngles = new Vector3(0f,0f,Random.Range (0f,360f));
			bodyPart.transform.parent = transform;
			bodyPart.GetComponent<Renderer>().material.SetFloat ("_RotationPerlin",Random.Range (0f,360f));



			bodyPart.GetComponent<BodyPartScript>().SetSize(headSize* progress);
			bodyParts[i] = bodyPart;

			//bodyPart.GetComponent<BodyPartScript>().SetLerpsColors(Mathf.PerlinNoise(progress*512f,0f),Mathf.PerlinNoise(progress*512f,0f));
			bodyPart.GetComponent<BodyPartScript>().SetLerpsColors(main.Map(progress,1f,0f,0.2f,0.8f),main.Map(progress,1f,0f,0.2f,0.8f));
			bodyPart.GetComponent<BodyPartScript>().SetOutlineWidth(Random.Range(0.0f,0.2f));
			bodyPart.GetComponent<BodyPartScript>().SetOutlineWidth(Random.value>0.5f?1f:0f);
			bodyPart.GetComponent<BodyPartScript>().SetOutlineWidth(0f);






			bodyPart.GetComponent<BodyPartScript>().SetColors(mainColorA,mainColorB,mainColorB,mainColorA);


			if(i>0)
			{
				//bodyPart.GetComponent<LineRenderer>().SetWidth(1.1f*bodyPart.transform.localScale.x,1.1f*bodyParts[i-1].transform.localScale.x);
			}
			else
			{
				Destroy (bodyPart.GetComponent<LineRenderer>());
			}

			bodyPart.GetComponent<TrailRenderer>().startWidth =bodyPart.GetComponent<BodyPartScript>().GetSize();
			bodyPart.GetComponent<TrailRenderer>().endWidth =0f;
			bodyPart.GetComponent<TrailRenderer>().material.color = bodyPart.GetComponent<BodyPartScript>().GetColors()[(int)(Random.value>0.5f?1f:0f)];
			bodyPart.GetComponent<TrailRenderer>().material.color = bodyPart.GetComponent<BodyPartScript>().GetColors()[0];
			bodyPart.GetComponent<TrailRenderer>().time = progress*0.3f+0.3f;

			bodyPartsPositions[i] = transform.position;





			randomValues[i]=Random.value;
			randomSens[i]=Random.value<0.5f?-1:1;

		}
		
		started = true;
	}
	
	// Update is called once per frame
	void Update () 
	{

		if(!dead)
		{
			if(started)
			{
				UpdateValues();
				Mouvement();
				PositionBodyParts();
				
				
				timeLife+=Time.deltaTime;
			}
		}

			
	}

	void UpdateValues()
	{
		radiusMotion.Update();
		speedProgressCircle.Update();
		ProgressionsProperties();
	}


	public void LaunchGoBigPond()
	{
		if(goBigPond==false && goCenter==false && isInBigPond == false)
		{
			goCenter = true;
			//progressCentreLittlePond = 0f;
			transitionFromProgressCircle = Modulo(progressCircle,1f)-4f;
			timerBeforeGoBigPond = durationGoCenterLittlePond+durationBeforeGoBigPond;
		}



	}

	public void StopGoBigPond()
	{
		if(goBigPond==false && goCenter == true&& isInBigPond == false)
		{
			goCenter = false;
			//progressCircle = Modulo(progressCircle,1f);
			//progressCentreLittlePond = 1f;
			//transitionFromProgressCircle = Modulo(progressCircle,1f)-4f;



			//PROGRESSCIRCLE GO ATAN TOO
			

			
			//_progress = Mathf.Clamp01(progressIntro*2f);
			


			progressCircle = transitionProgressCircle = Mathf.Lerp (transitionFromProgressCircle,0f,progressCentreLittlePond);


		
		}
		
		
		
	}






	void Mouvement()
	{
		progressCircle+=Time.deltaTime*speedProgressCircle.value*main.pulse;







		float _progressCircle = progressCircle;



		if(isInBigPond==false)
		{
			if(goCenter)
			{
				progressCentreLittlePond=Mathf.Clamp01(progressCentreLittlePond+Time.deltaTime/durationGoCenterLittlePond);



				transitionProgressCircle = Mathf.Lerp (transitionFromProgressCircle,0f,progressCentreLittlePond);
				_progressCircle = transitionProgressCircle;
			}
			else
			{
				progressCentreLittlePond=Mathf.Clamp01(progressCentreLittlePond-Time.deltaTime*1.5f);




				//transitionProgressCircle = progressCircle;
				//_progressCircle = progressCircle;
			}


			if(goBigPond)
			{

				progressChangementPond = Mathf.Clamp01(progressChangementPond+Time.deltaTime*0.2f);
				centerPond = Vector3.Lerp(Vector3.right*15f,Vector3.zero,curveToNewPond.Evaluate(progressChangementPond));


				if(progressChangementPond==1f)
				{

					main.currentFish = null;
					isInBigPond = true;

					goBigPond = false;
					goCenter = false;
				}
			}
			else 
			{


				centerPond = Vector3.Lerp(Vector3.right*17f,Vector3.right*15f,curveGoCenter.Evaluate(progressCentreLittlePond));

				if(goCenter)
				{

					
					timerBeforeGoBigPond-=Time.deltaTime;
					
					if(timerBeforeGoBigPond<=0f)
					{

						goBigPond = true;
						goCenter = false;
						
						radiusMotion.minMaxNextValue = new Vector2(1f,11f);
						radiusMotion.minMaxDurationProgress= new Vector2(1f,1.5f);
						
						
						speedProgressCircle.minMaxNextValue = new Vector2(0.1f,0.3f);
						radiusMotion.minMaxDurationProgress= new Vector2(1f,3f);
						speedProgressCircle.useRandomSigne = true;
						
						main.currentFish = null;
						
					}



						






					
					
				}



			}
		}
		else
		{
			centerPond = Vector3.zero;
			progressCentreLittlePond = 0f;
		}








		Vector3 pos = centerPond;

		//_progress = Mathf.Clamp01(progressIntro*2f);

		pos.x+=Mathf.Cos(_progressCircle*Mathf.PI*2f)*radiusMotion.value*(1f-progressCentreLittlePond);
		pos.y+=Mathf.Sin(_progressCircle*Mathf.PI*2f)*radiusMotion.value*(1f-progressCentreLittlePond);


		pos+=Vector3.up*Mathf.Sin(Time.realtimeSinceStartup*2f)*progressCentreLittlePond*0.2f+Vector3.right*Mathf.Cos(Time.realtimeSinceStartup*2f)*progressCentreLittlePond*0.2f;






		transform.position = pos;

	}

	void PositionBodyParts()
	{
		if(started)
		{

			for (int i=0; i<bodyParts.Length; i++)
			{

				//float _i = (float)i;


				//float progress = Map (_i,0f,(float)(bodyParts.Length-1),0f,1f);


				if(i!=0)
				{
					float progressB = Map ((float)i,1f,(float)(bodyParts.Length-1),0f,1f);

					float nodeAngle	 =	Mathf.Atan2(bodyPartsPositions[i].y - bodyPartsPositions[i-1].y,bodyPartsPositions[i].x - bodyPartsPositions[i-1].x);


					float thisTailLength = tailLenght*(1f-progressB);







					bodyPartsPositions[i] = new Vector3(bodyPartsPositions[i-1].x + thisTailLength * Mathf.Cos(nodeAngle),bodyPartsPositions[i-1].y + thisTailLength * Mathf.Sin(nodeAngle),0f);




					Vector3 toForward = Vector3.Normalize (bodyPartsPositions[i]-bodyPartsPositions[i-1]);
					Vector3 toUp = Vector3.Cross (toForward,Vector3.forward);



					float randomValue = randomValues[i];
					float randomValueT =  randomValue + Time.realtimeSinceStartup*randomSens[i];


					bodyParts[i].transform.position = bodyPartsPositions[i]+toForward*Mathf.Cos (randomValueT*2f*Mathf.PI)*radiusMotionBodyParts*progressB+toUp*Mathf.Sin (randomValueT*2f*Mathf.PI)*radiusMotionBodyParts*progressB;


					//bodyParts[i].GetComponent<LineRenderer>().SetPosition(0,bodyParts[i].transform.position);
					//bodyParts[i].GetComponent<LineRenderer>().SetPosition(1,bodyParts[i-1].transform.position);


				}
				else
				{
					bodyPartsPositions[0] = transform.position;
					bodyParts[0].transform.position = bodyPartsPositions[0];
				}

				//bodyParts[i].transform.localScale = Vector3.one*headSize* (1f-Mathf.Clamp01(progress));


			}
		}
	}

	void LateUpdate()
	{
		if(dead)
		DestroyImmediate(this.gameObject);
	}



	float Radians(float value)
	{
		float val = Mathf.PI*value/180;
		return(val);
	}

	float Map(float val, float fromMin, float fromMax, float toMin, float toMax) {
		return ((val - fromMin) / (fromMax - fromMin)) * (toMax - toMin) + toMin;
	}

	float Modulo(float a,float b)
	{
		return a - b * Mathf.Floor(a / b);
	}

	public void AffectProperty1(float _value)
	{
		//nextTailLenght = nextTailLenght>0.7f?Random.Range (0.1f,0.7f):Random.Range (0.7f,1.3f);

		nextTailLenght = _value;
		//ProgressionsProperties();


	}

	public void AffectProperty2(float _value)
	{
		//radiusMotionBodyParts = radiusMotionBodyParts>0.75f?Random.Range (0f,0.75f):Random.Range (0.75f,1.5f);
		//nextOutlineWidth = nextOutlineWidth>0.5f?Random.Range (0f,0.5f):Random.Range (0.5f,1f);
		nextOutlineWidth = _value;
		//ProgressionsProperties();
	}

	public void AffectProperty3(float _value)
	{
		//nextHeadSize = nextHeadSize>2.5f?Random.Range (0f,2.5f):Random.Range (2.5f,4f);
		nextHeadSize = _value;
	//	ProgressionsProperties();
	}

	void ProgressionsProperties()
	{

		float valueProgressProgress = Map (Mathf.Clamp01(timeLife),0f,1f,0.1f,1f);





		outlineWidth += (nextOutlineWidth-outlineWidth)*valueProgressProgress;





		tailLenght+=(nextTailLenght-tailLenght)*valueProgressProgress;

		headSize+=(nextHeadSize-headSize)*valueProgressProgress;

		if(dying)
		{
			finalLife=Mathf.Clamp01(finalLife-Time.deltaTime);
		}


		headSize = headSize*finalLife;







		for (int i=0; i<bodyParts.Length; i++)
		{
			float progress = Map ((float)i,0f,(float)(bodyParts.Length-1),0f,1f);
			bodyParts[i].GetComponent<BodyPartScript>().SetSize(headSize* (1f-Mathf.Clamp01(progress)));
			bodyParts[i].GetComponent<TrailRenderer>().startWidth =bodyParts[i].GetComponent<BodyPartScript>().GetSize();
			bodyParts[i].GetComponent<TrailRenderer>().endWidth =0f;

			bodyParts[i].GetComponent<Renderer>().material.SetFloat ("_OutlineWidth",outlineWidth);
		}

		if(finalLife==0f)
		{
			GoDestroy();
		}







	}

	void GoDestroy()
	{
		dead = true;

	}

	public void GoDie()
	{
		dying = true;


	}



}
