using UnityEngine;
using System.Collections.Generic;

public class BulletSpawner : MonoBehaviour
{
    [Header("Cấu hình")]
    public GameObject bulletPrefab;
    public int bulletCount = 5;
    public float spacing = 1.2f;
    public float queueSpeed = 10f;
    public float bulletFlightSpeed = 20f;

    private List<GameObject> bulletList = new List<GameObject>();
    private Vector3 targetShootPosition;
    private bool isFiring = false;

    private Vector3 shootingPoint => transform.position;

    private void Start()
    {
        SpawnInitialBullets();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && bulletList.Count > 0)
        {
            targetShootPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            targetShootPosition.z = 0;
            isFiring = true;
        }

        MoveBulletsInQueue();
        CheckAndShoot();
    }

    void SpawnInitialBullets()
    {
        for (int i = 0; i < bulletCount; i++)
        {
            Vector3 spawnPos = shootingPoint + Vector3.left * spacing * i;
            GameObject bullet = Instantiate(bulletPrefab, spawnPos, Quaternion.identity);
            bulletList.Add(bullet);
        }
    }

    void MoveBulletsInQueue()
    {
        for (int i = 0; i < bulletList.Count; i++)
        {
            if (bulletList[i] == null) continue;

            // Đích đến là vị trí xếp hàng sau shootingPoint
            Vector3 targetPos = shootingPoint + Vector3.left * spacing * i;

            bulletList[i].transform.position = Vector3.MoveTowards(
                bulletList[i].transform.position,
                targetPos,
                queueSpeed * Time.deltaTime
            );
        }
    }

    void CheckAndShoot()
    {
        if (!isFiring || bulletList.Count == 0) return;

        GameObject bulletA = bulletList[0];

        // Chỉ bắn khi viên đạn đã về tới vị trí đầu hàng (Shooting Point)
        if (Vector3.Distance(bulletA.transform.position, shootingPoint) < 0.05f)
        {
            BulletMovement movement = bulletA.GetComponent<BulletMovement>();
            if (movement == null) movement = bulletA.AddComponent<BulletMovement>();

            // Gửi 'this' vào để viên đạn biết quay về đâu
            movement.Launch(targetShootPosition, bulletFlightSpeed, this);

            bulletList.RemoveAt(0);

            if (bulletList.Count == 0) isFiring = false;
        }
    }

    // Hàm quan trọng để thu hồi đạn
    public void ReturnBulletToQueue(GameObject bullet)
    {
        if (!bulletList.Contains(bullet))
        {
            bulletList.Add(bullet);
        }
    }
}