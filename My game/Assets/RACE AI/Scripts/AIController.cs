using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarAI : MonoBehaviour
{
    public float detectionRange = 5f;            // ��������� �����������
    public float sideRayOffset = 2f;             // �������� ��� ������� Raycast'��
    public LayerMask obstacleLayer;               // ���� ��� �����������
    public Wheel[] frontwheels;                   // ������ �������� �����
    public float forwardSpeed = 10f;              // �������� �������� ������
    public float maxSteerAngle = 30f;             // ������������ ���� ��������
    public float avoidTurnSpeed = 2f;             // �������� �������� ��� ��������� �����������

    private float turn;                           // ���� ��������

    void FixedUpdate()
    {
        DetectObstacles();
        MoveCar();
    }

    private void DetectObstacles()
    {
        RaycastHit hit;

        // �������� �������
        bool isObstacleAhead = Physics.Raycast(transform.position, transform.forward, out hit, detectionRange, obstacleLayer);

        // �������� �����
        Vector3 leftRayOrigin = transform.position - transform.right * sideRayOffset;
        bool isObstacleLeft = Physics.Raycast(leftRayOrigin, transform.forward, out hit, detectionRange, obstacleLayer);

        // �������� ������
        Vector3 rightRayOrigin = transform.position + transform.right * sideRayOffset;
        bool isObstacleRight = Physics.Raycast(rightRayOrigin, transform.forward, out hit, detectionRange, obstacleLayer);

        // ������ ��� ��������� ���� ��������
        if (isObstacleAhead)
        {
            if (!isObstacleRight && !isObstacleLeft)
            {
                // ���� ����������� ������ �������, �� ������� ������������ � ����� �������
                turn = Random.Range(-1f, 1f) * avoidTurnSpeed;
            }
            else if (!isObstacleRight)
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
                // ���� ����������� �� ���� ��������, ������������� �������
                turn = 0f;
            }
        }
        else
        {
            turn = 0f; // ���� ��� �����������, ���������� �������� ������
        }
    }

    private void MoveCar()
    {
        // ������ �������� ������
        foreach (var wheel in frontwheels)
        {
            wheel.collider.motorTorque = forwardSpeed; // ������������� ���� ��� �����
            wheel.collider.steerAngle = turn * maxSteerAngle; // ������������� ���� ��������
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, transform.forward * detectionRange); // �������� Raycast
        Gizmos.DrawRay(transform.position - transform.right * sideRayOffset, transform.forward * detectionRange); // ����� Raycast
        Gizmos.DrawRay(transform.position + transform.right * sideRayOffset, transform.forward * detectionRange); // ������ Raycast
    }
}
