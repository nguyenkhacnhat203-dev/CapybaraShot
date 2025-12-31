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
    private bool isWaitingForReturn = false;

    private Vector3 shootingPoint => transform.position;

    private void Start()
    {
        SpawnInitialBullets();

        UIManager.Instance.UpdateBulletCount(bulletCount);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0)
            && bulletList.Count == bulletCount
            && !isFiring
            && !isWaitingForReturn)
        {
            Vector3 clickWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            clickWorldPos.z = 0;

            if (clickWorldPos.y > shootingPoint.y + 0.2f)
            {
                targetShootPosition = clickWorldPos;
                isFiring = true;
            }
        }

        MoveBulletsInQueue();
        CheckAndShoot();
        CheckAllBulletsReturned();
    }

    void SpawnInitialBullets()
    {
        bulletList.Clear();

        for (int i = 0; i < bulletCount; i++)
        {
            CreateNewBullet(i);
        }
    }

    void CreateNewBullet(int index)
    {
        Vector3 spawnPos = shootingPoint + Vector3.left * spacing * index;
        GameObject bullet = Instantiate(bulletPrefab, spawnPos, Quaternion.identity);

        BulletMovement movement = bullet.GetComponent<BulletMovement>();
        if (movement == null)
            movement = bullet.AddComponent<BulletMovement>();

        bulletList.Add(bullet);
    }

    public void AddBullet()
    {
        bulletCount++;               
        CreateNewBullet(bulletList.Count);

        UIManager.Instance.UpdateBulletCount(bulletCount);
    }

    void MoveBulletsInQueue()
    {
        for (int i = 0; i < bulletList.Count; i++)
        {
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

        if (Vector3.Distance(bulletA.transform.position, shootingPoint) < 0.05f)
        {
            BulletMovement movement = bulletA.GetComponent<BulletMovement>();
            movement.Launch(targetShootPosition, bulletFlightSpeed, this);

            bulletList.RemoveAt(0); 

            if (bulletList.Count == 0)
            {
                isFiring = false;
                isWaitingForReturn = true;
            }
        }
    }

    public void ReturnBulletToQueue(GameObject bullet)
    {
        if (!bulletList.Contains(bullet))
        {
            bulletList.Add(bullet);
        }
    }

    void CheckAllBulletsReturned()
    {
        if (!isWaitingForReturn) return;
        if (bulletList.Count < bulletCount) return;

        for (int i = 0; i < bulletList.Count; i++)
        {
            Vector3 targetPos = shootingPoint + Vector3.left * spacing * i;
            if (Vector3.Distance(bulletList[i].transform.position, targetPos) > 0.05f)
                return;
        }

        isWaitingForReturn = false;
        OnTurnEnded();
    }

    void OnTurnEnded()
    {
        Block[] blocks = FindObjectsOfType<Block>();
        foreach (Block block in blocks)
        {
            block.MoveDownTween(1.18f, 0.25f);
        }

        Capypara_AddBulet[] capyparas = FindObjectsOfType<Capypara_AddBulet>();
        foreach (Capypara_AddBulet capy in capyparas)
        {
            capy.MoveDownTween(1.18f, 0.25f);
        }

        BlockManager blockManager = FindAnyObjectByType<BlockManager>();
        if (blockManager != null)
            blockManager.SpawnBlocks();
    }
}
