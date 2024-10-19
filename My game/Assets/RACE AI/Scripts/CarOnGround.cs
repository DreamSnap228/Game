using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleDetection : MonoBehaviour
{
    public float detectionRange = 100f;              // Дистанция обнаружения
    public float sideRayOffset = 200f;               // Смещение для боковых Raycast'ов
    public LayerMask obstacleLayer;                 // Слой для препятствий

    public float turn;                              // Угол поворота, который будет использоваться в CarAI

    private void FixedUpdate()
    {
        DetectObstacles();
    }

    private void DetectObstacles()
    {
        RaycastHit hit;
        Vector3 forwardDirection = transform.forward;

        // Проверка впереди
        bool isObstacleAhead = Physics.Raycast(transform.position, forwardDirection, out hit, detectionRange, obstacleLayer);

        // Проверка слева
        Vector3 leftRayOrigin = transform.position - transform.right * sideRayOffset;
        bool isObstacleLeft = Physics.Raycast(leftRayOrigin, forwardDirection, out hit, detectionRange, obstacleLayer);

        // Проверка справа
        Vector3 rightRayOrigin = transform.position + transform.right * sideRayOffset;
        bool isObstacleRight = Physics.Raycast(rightRayOrigin, forwardDirection, out hit, detectionRange, obstacleLayer);

        // Логика для изменения угла поворота
        if (isObstacleAhead)
        {
            if (!isObstacleRight)
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
                // Если есть препятствия по всем сторонам
                turn = -1f; // Можно поворачивать в одну сторону, например, налево
            }
        }
        else
        {
            turn = 0f; // Нет препятствий, продолжаем движение вперед
        }
    }

    public float GetTurnDirection()
    {
        return turn; // Возвращаем текущее значение угла поворота
    }
}
