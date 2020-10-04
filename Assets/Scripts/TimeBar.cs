﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeBar : MonoBehaviour
{
    private float maxValue;
	Color color = Color.red;		
	public Slider slider;				
    private float current;

    public float timeAmount = 15f;
	

	public Snake WormPrefab;

    public Transform SpawnPoint;

    public Snake CurrentWorm;

	public GameObject comic;

	public bool IsActive => comic.activeSelf == false;

	public PostProcessControll postProcessControl;

	void Start()
	{
		maxValue = timeAmount; 	

		slider.fillRect.GetComponent<Image>().color = color;

		slider.maxValue = maxValue;
		slider.minValue = 0;
		current = maxValue;


		//StartCoroutine(Beep);
	}	

	private IEnumerable Beep()
	{
		while (true)
		{
			if (!IsActive)
				yield return null;


			yield return new WaitForSeconds(1f);
		}		
	}

	private bool isBeeped = false;
	void Update()
	{	
		if(comic.activeSelf == false) current -= Time.deltaTime;

		if (!isBeeped)
		{ 
			StartCoroutine(postProcessControl.Beep());
			isBeeped = true;
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
			StartCoroutine(postProcessControl.BigBeep());
			

			current = maxValue;

			CurrentWorm.Dead();

            CurrentWorm = Instantiate(WormPrefab, SpawnPoint);
		}
	}
}
