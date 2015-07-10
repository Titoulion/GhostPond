using UnityEngine;
using System.Collections;

public class BodyPartScript : MonoBehaviour {



	public void SetColors(Color mainColorA, Color mainColorB, Color outlineColorA, Color outlineColorB)
	{
		GetComponent<Renderer>().material.SetColor("_MainColorA",mainColorA);
		GetComponent<Renderer>().material.SetColor("_MainColorB",mainColorB);
		GetComponent<Renderer>().material.SetColor("_OutlineColorA",outlineColorA);
		GetComponent<Renderer>().material.SetColor("_OutlineColorB",outlineColorB);
	}

	public void SetLerpsColors(float lerpMainColor,float lerpOutlineColor)
	{
		GetComponent<Renderer>().material.SetFloat("_LerpMainColor",lerpMainColor);
		GetComponent<Renderer>().material.SetFloat("_LerpOutlineColor",lerpOutlineColor);
	}

	public void SetOutlineWidth(float outlineWidth)
	{
		GetComponent<Renderer>().material.SetFloat("_OutlineWidth",outlineWidth);
	}

	public Color[] GetColors()
	{
		Color[] colors = new Color[2];
		colors[0] = Color.Lerp(GetComponent<Renderer>().material.GetColor("_MainColorA"),GetComponent<Renderer>().material.GetColor("_MainColorB"),GetComponent<Renderer>().material.GetFloat("_LerpMainColor"));
		colors[1] = Color.Lerp(GetComponent<Renderer>().material.GetColor("_OutlineColorA"),GetComponent<Renderer>().material.GetColor("_OutlineColorB"),GetComponent<Renderer>().material.GetFloat("_LerpOutlineColor"));
		return(colors);
	}

	public void SetSize(float size)
	{
		GetComponent<Renderer>().material.SetFloat("_Size",size);
	}

	public float GetSize()
	{
		return(GetComponent<Renderer>().material.GetFloat("_Size"));
	}


}
