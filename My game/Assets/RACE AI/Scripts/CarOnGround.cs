using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleDetection : MonoBehaviour
{
    public float detectionRange = 100f;              // ��������� �����������
    public float sideRayOffset = 200f;               // �������� ��� ������� Raycast'��
    public LayerMask obstacleLayer;                 // ���� ��� �����������

    public float turn;                              // ���� ��������, ������� ����� �������������� � CarAI

    private void FixedUpdate()
    {
        DetectObstacles();
    }

    private void DetectObstacles()
    {
        RaycastHit hit;
        Vector3 forwardDirection = transform.forward;

        // �������� �������
        bool isObstacleAhead = Physics.Raycast(transform.position, forwardDirection, out hit, detectionRange, obstacleLayer);

        // �������� �����
        Vector3 leftRayOrigin = transform.position - transform.right * sideRayOffset;
        bool isObstacleLeft = Physics.Raycast(leftRayOrigin, forwardDirection, out hit, detectionRange, obstacleLayer);

        // �������� ������
        Vector3 rightRayOrigin = transform.position + transform.right * sideRayOffset;
        bool isObstacleRight = Physics.Raycast(rightRayOrigin, forwardDirection, out hit, detectionRange, obstacleLayer);

        // ������ ��� ��������� ���� ��������
        if (isObstacleAhead)
        {
            if (!isObstacleRight)
            {
                // ���� ������ ��� �����������, ������������ �������
                turn = 1f;
            }
            else if (!isObstacleLeft)
            {
                // ���� ����� ��� �����������, ������������ ������
                turn = -1f;
            }
            else
            {
                // ���� ���� ����������� �� ���� ��������
                turn = -1f; // ����� ������������ � ���� �������, ��������, ������
            }
        }
        else
        {
            turn = 0f; // ��� �����������, ���������� �������� ������
        }
    }

    public float GetTurnDirection()
    {
        return turn; // ���������� ������� �������� ���� ��������
    }
}
