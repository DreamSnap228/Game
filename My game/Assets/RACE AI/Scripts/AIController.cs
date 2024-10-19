using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarAI : MonoBehaviour
{
    public float detectionRange = 5f;            // Дистанция обнаружения
    public float sideRayOffset = 2f;             // Смещение для боковых Raycast'ов
    public LayerMask obstacleLayer;               // Слой для препятствий
    public Wheel[] frontwheels;                   // Массив передних колес
    public float forwardSpeed = 10f;              // Скорость движения вперед
    public float maxSteerAngle = 30f;             // Максимальный угол поворота
    public float avoidTurnSpeed = 2f;             // Скорость поворота для избегания препятствий

    private float turn;                           // Угол поворота

    void FixedUpdate()
    {
        DetectObstacles();
        MoveCar();
    }

    private void DetectObstacles()
    {
        RaycastHit hit;

        // Проверка впереди
        bool isObstacleAhead = Physics.Raycast(transform.position, transform.forward, out hit, detectionRange, obstacleLayer);

        // Проверка слева
        Vector3 leftRayOrigin = transform.position - transform.right * sideRayOffset;
        bool isObstacleLeft = Physics.Raycast(leftRayOrigin, transform.forward, out hit, detectionRange, obstacleLayer);

        // Проверка справа
        Vector3 rightRayOrigin = transform.position + transform.right * sideRayOffset;
        bool isObstacleRight = Physics.Raycast(rightRayOrigin, transform.forward, out hit, detectionRange, obstacleLayer);

        // Логика для изменения угла поворота
        if (isObstacleAhead)
        {
            if (!isObstacleRight && !isObstacleLeft)
            {
                // Если препятствия только спереди, то немного поворачиваем в любую сторону
                turn = Random.Range(-1f, 1f) * avoidTurnSpeed;
            }
            else if (!isObstacleRight)
            {
                // Если справа нет препятствия, поворачиваем направо
                turn = 1f;
            }
            else if (!isObstacleLeft)
            {
                // Если слева нет препятствия, поворачиваем налево
                turn = -1f;
            }
            else
            {
                // Если препятствия по всем сторонам, останавливаем поворот
                turn = 0f;
            }
        }
        else
        {
            turn = 0f; // Если нет препятствий, продолжаем движение вперед
        }
    }

    private void MoveCar()
    {
        // Логика движения машины
        foreach (var wheel in frontwheels)
        {
            wheel.collider.motorTorque = forwardSpeed; // Устанавливаем силу для колес
            wheel.collider.steerAngle = turn * maxSteerAngle; // Устанавливаем угол поворота
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, transform.forward * detectionRange); // Передний Raycast
        Gizmos.DrawRay(transform.position - transform.right * sideRayOffset, transform.forward * detectionRange); // Левый Raycast
        Gizmos.DrawRay(transform.position + transform.right * sideRayOffset, transform.forward * detectionRange); // Правый Raycast
    }
}
