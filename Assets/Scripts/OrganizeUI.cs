using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrganizeUI : MonoBehaviour
{
    public GameObject _UIScreen;
  
   void Awake()
  {
      Instantiate(_UIScreen);
  }

}
