﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;


public class TimeBar : MonoBehaviour
{
    private float maxValue;
	Color color = Color.red;		
	public Slider slider;				
    public float current;

    public float timeAmount = 15f;
	
	public GameObject comic;

	public bool IsActive => comic.activeSelf == false;

	public PostProcessControll postProcessControl;
	[SerializeField] private float[] beepTime = new float[10];
	
	public static TimeBar Instance;

	public void Awake()
	{
		Instance = this;
	}

	void Start()
	{
		maxValue = timeAmount; 	

		slider.fillRect.GetComponent<Image>().color = color;

		slider.maxValue = maxValue;
		slider.minValue = 0;
		current = maxValue;


		//StartCoroutine(Beep);
	}	

	//private IEnumerable Beep()
	//{
	//	while (true)
	//	{
	//		if (!IsActive)
	//			yield return null;


	//		yield return new WaitForSeconds(1f);
	//	}		
	//}

	private bool isBeeped = false;
	void Update()
	{	
		if(comic.activeSelf == false) current -= Time.deltaTime;

		if (!isBeeped)
		{ 
			if(postProcessControl != null)
			{
				StartCoroutine(postProcessControl.Beep(timeAmount - 10));
				isBeeped = true;
			}			
		}

		if (current < 0)
		{ 
			current = 0;			
		}

		if (current > maxValue) current = maxValue;

		slider.value = current;

		

		CheckDead();
	}
	

	public void CheckDead()
	{
		if (current <= 0)
		{
			if (postProcessControl != null)
			{
				postProcessControl.flash = true;
				postProcessControl.flashBig = true;
				isBeeped = false;
			}

			current = maxValue;

			Game.Instance.WormDead();			
		}
	}


	public void ButtonDead() 
	{
		if (postProcessControl != null) 
		{
			postProcessControl.flash = true;
			postProcessControl.flashBig = true;
			isBeeped = false;
		}

		current = maxValue;

		Game.Instance.WormDead();
	}
}
