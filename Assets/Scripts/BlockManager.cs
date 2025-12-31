using System.Collections.Generic;
using UnityEngine;

public class BlockManager : MonoBehaviour
{
    public List<Transform> Po;
    public Block blockPrefab;
    public Capypara_AddBulet Capypara_AddBuletPrefab;
    public int spawnCount = 5;
    public float moveDistance = 1.5f;
    public float moveDuration = 0.3f;






    private void Start()
    {
        FistSpawnBlocks();
    }


  
    public void FistSpawnBlocks()
    {
        if (Po == null || Po.Count == 0 || blockPrefab == null)
            return;

        float[] offsets = { 1.18f, 1.18f * 2, 1.18f * 3 };

        foreach (Transform spawnPos in Po)
        {
            float randomOffsetY = offsets[Random.Range(0, offsets.Length)];

            Vector3 finalPosition = new Vector3(spawnPos.position.x, spawnPos.position.y - randomOffsetY, spawnPos.position.z);

            Block block = Instantiate(blockPrefab, finalPosition, Quaternion.identity);

            int randomHealth = Random.Range(1, 5);
            block.SetHealth(randomHealth);

        }
    }








    public void SpawnBlocks()
    {
        if (Po == null || Po.Count == 0 || blockPrefab == null)
            return;

        List<Transform> availablePos = new List<Transform>(Po);

        BulletSpawner spawner = FindFirstObjectByType<BulletSpawner>();
        int currentBulletCount = (spawner != null) ? spawner.bulletCount : 0;

        bool canSpawnItem = false;
        if (currentBulletCount < 8)
        {
            if (Random.value <= 0.2f)
            {
                canSpawnItem = true;
            }
        }

        int blocksToSpawn = canSpawnItem ? Mathf.Min(spawnCount - 1, availablePos.Count - 1) : Mathf.Min(spawnCount, availablePos.Count);

        for (int i = 0; i < blocksToSpawn; i++)
        {
            if (availablePos.Count == 0) break;

            int randomIndex = Random.Range(0, availablePos.Count);
            Transform spawnPos = availablePos[randomIndex];
            availablePos.RemoveAt(randomIndex);

            Block block = Instantiate(blockPrefab, spawnPos.position, Quaternion.identity);
            int randomHealth = Random.Range(5, 10);
            block.SetHealth(randomHealth);
            block.MoveDownTween(moveDistance, moveDuration);
        }

        if (canSpawnItem && availablePos.Count > 0 && Capypara_AddBuletPrefab != null)
        {
            int randomIndex = Random.Range(0, availablePos.Count);
            Transform spawnPos = availablePos[randomIndex];

            Capypara_AddBulet item = Instantiate(Capypara_AddBuletPrefab, spawnPos.position, Quaternion.identity);
            item.MoveDownTween(moveDistance, moveDuration);
        }
    }
}