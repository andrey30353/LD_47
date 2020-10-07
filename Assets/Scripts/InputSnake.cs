using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputSnake : MonoBehaviour
{
    Snake snake;
    float _inputSpeed = 0.2f;
    Vector3 _input;

    float _inputX;
    float _inputY;

    // Start is called before the first frame update
    void Awake()
    {
        snake =  GetComponent<Snake>();
    }
    
    // Update is called once per frame
    void Update()
    {

        snake = Game.Instance.CurrentWorm;
        /*
        _input = Vector3.zero;

        var horizontal = Input.GetAxis("Horizontal");
        var vertical = Input.GetAxis("Vertical");

        _input = new Vector3(horizontal, vertical, 0);
       */
        
        if(_inputX > 0.1f)        
            _inputX -= 0.05f;        
        if (_inputX < -0.1f)        
            _inputX += 0.05f;

        if (_inputY > 0.1f)
            _inputY -= 0.05f;
        if (_inputY < -0.1f)
            _inputY += 0.05f;

        print(_inputX);

        if (_inputX > -0.1f || _inputX < 0.1f)
            _inputX = 0;

        if (_inputY > -0.1f || _inputY < 0.1f)
            _inputY = 0;

        //_input = Vector3.zero;

        if (Input.GetKey(KeyCode.A))
            _inputX -= 0.2f;
        if (Input.GetKey(KeyCode.D))
            _inputX += 0.2f;

        if (Input.GetKey(KeyCode.W))
            _inputY += 0.2f;
        if (Input.GetKey(KeyCode.S))
            _inputY -= 0.2f;

        _inputX = Mathf.Clamp(_inputX, -1, 1);
        _inputY = Mathf.Clamp(_inputY, -1, 1);

        _input = new Vector3(_inputX, _inputY, 0);
        
        //print(_input);
       // snake._input = _input;
    }
}
