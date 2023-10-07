using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    float _minWidth = -10f;
    float _maxWidth = 10f;
    float _minHeight = -24f;
    float _maxHeight = 12f;
    float _speed = 20f;
    float _originHieght;
    public void SetSize(float minWidth, float maxWidth, float minHeight, float maxHeight)
    {
        _minWidth = minWidth;
        _maxWidth = maxWidth;
        _minHeight = minHeight;
        _maxHeight = maxHeight;
    }

    private void Start()
    {
        _originHieght = transform.position.y;
    }

    private void Update()
    {
        float interval = 10f;
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        if (Input.mousePosition.x >= Screen.width - interval && transform.position.x < _maxWidth || horizontal > 0f)
            transform.position += Vector3.right * _speed * Time.deltaTime;
        if (Input.mousePosition.x <= interval && transform.position.x > _minWidth || horizontal < 0f)
            transform.position += Vector3.left * _speed * Time.deltaTime;
        if (Input.mousePosition.y >= Screen.height - interval && transform.position.z < _maxHeight || vertical > 0f)
            transform.position += Vector3.forward * _speed * Time.deltaTime;
        if (Input.mousePosition.y <= interval && transform.position.z > _minHeight || vertical < 0f)
            transform.position += Vector3.back * _speed * Time.deltaTime;
    }
}