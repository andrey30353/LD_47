using System.Collections;
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


    void Start()
	{
		maxValue = timeAmount; 	

		slider.fillRect.GetComponent<Image>().color = color;

		slider.maxValue = maxValue;
		slider.minValue = 0;
		current = maxValue;

	}


    void Update()
	{	
		if(comic.activeSelf == false) current -= Time.deltaTime;

		if (current < 0) current = 0;

		if (current > maxValue) current = maxValue;

		slider.value = current;

		CheckDead();
	}


	public void CheckDead()
	{
		if (current <= 0)
		{
			current = maxValue;

			CurrentWorm.Dead();

            CurrentWorm = Instantiate(WormPrefab, SpawnPoint);
		}
	}
}
