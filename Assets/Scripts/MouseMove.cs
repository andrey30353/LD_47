using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseMove : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
		if (Input.GetMouseButton(0)) // Удерживать правую кнопку мыши
		{
					
				RaycastHit hit;
				Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
				if (Physics.Raycast(ray, out hit, 10f))
				{
				/*if (GetTag(hit.transform.tag) && hit.rigidbody && !curObj)
				{
					curObj = hit.transform;
					mass = curObj.GetComponent<Rigidbody>().mass; // запоминаем массу объекта
					curObj.GetComponent<Rigidbody>().mass = 0.0001f; // убираем массу, чтобы не сбивать другие объекты
					curObj.GetComponent<Rigidbody>().useGravity = false; // убираем гравитацию
					curObj.GetComponent<Rigidbody>().freezeRotation = true; // заморозка вращения
					curObj.position += new Vector3(0, 0.5f, 0); // немного приподымаем выбранный объект
				}*/

				Vector3 mousePosition = Camera.main.ScreenToWorldPoint(
						new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.transform.position.y));
				hit.transform.position = mousePosition;
				//mousePosition.GetComponent<Rigidbody>().MovePosition(new Vector3(mousePosition.x, curObj.position.y + Input.GetAxis("Mouse ScrollWheel") * step, mousePosition.z));
			}			
			
		}
		/*
		else if (curObj)
		{
			if (curObj.GetComponent<Rigidbody>())
			{
				curObj.GetComponent<Rigidbody>().freezeRotation = false;
				curObj.GetComponent<Rigidbody>().useGravity = true;
				curObj.GetComponent<Rigidbody>().mass = mass;
			}
			else
			{
				curObj.GetComponent<Rigidbody2D>().freezeRotation = false;
				curObj.GetComponent<Rigidbody2D>().mass = mass;
				curObj.GetComponent<Rigidbody2D>().gravityScale = 1;
			}
			curObj = null;
		}*/
	}
}
