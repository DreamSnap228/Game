using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Car car { get; private set; }
    private void Awake()
    {
        car = GetComponent<Car>();
    }


    // Update is called once per frame
    void Update()
    {
        car.forward = Input.GetAxis("Vertical");
        car.turn = Input.GetAxis("Horizontal");
    }
}