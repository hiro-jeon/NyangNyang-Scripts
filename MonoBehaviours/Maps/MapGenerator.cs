using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapGenerator : MonoBehaviour
{
    [Header("맵 설정")]
    public int chunkSize = 32;

    [Header("타일맵")]
    public Tilemap tilemap;
    public TileBase groundTile;
    public TileBase wallTile;
    public TileBase waterTile;

    [Header("타일 생성 확률")]
    [Range(0, 100)] public int wallRate = 10;
    [Range(0f, 1f)] public float waterChance = 0.05f;

    [Header("물 퍼짐 반경")]
    public int waterSpreadRadius = 3;

    private Transform player;
    private HashSet<Vector2Int> generatedChunks = new();

     // ✅ blockedTiles 저장소 (A*에서 사용)
    public HashSet<Vector2Int> blockedTiles = new();

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        GenerateChunks(Vector3.zero);
    }

    void Update()
    {
		if (player == null)
		{
			player = GameObject.FindGameObjectWithTag("Player")?.transform;
		}
		else
		{
        	GenerateChunks(player.position);
		}
    }


	
    void GenerateChunks(Vector3 position)
    {
        Vector2Int currentChunk = WorldToChunkCoord(position);

        for (int dx = -1; dx <= 1; dx++)
        {
            for (int dy = -1; dy <= 1; dy++)
            {
                Vector2Int chunkCoord = currentChunk + new Vector2Int(dx, dy);
                if (!generatedChunks.Contains(chunkCoord))
                {
                    GenerateChunk(chunkCoord);
                    generatedChunks.Add(chunkCoord);
                }
            }
        }
    }

    Vector2Int WorldToChunkCoord(Vector3 position)
    {
        return new Vector2Int(
            Mathf.FloorToInt(position.x / chunkSize),
            Mathf.FloorToInt(position.y / chunkSize)
        );
    }

    void GenerateChunk(Vector2Int chunkCoord)
    {
        Vector3Int origin = new Vector3Int(chunkCoord.x * chunkSize, chunkCoord.y * chunkSize, 0);

        for (int x = 0; x < chunkSize; x++)
        {
            for (int y = 0; y < chunkSize; y++)
            {
                Vector3Int pos = origin + new Vector3Int(x, y, 0);
                if (tilemap.HasTile(pos)) continue; // 이미 생성된 타일이면 건너뜀

                tilemap.SetTile(pos, groundTile); // 기본 땅 깔기

                // 맵 외곽 근처일 경우 물 퍼뜨릴 가능성
                if (Random.value < waterChance && IsNearEdge(x, y))
                {
                    SpreadWater(pos, waterSpreadRadius);
                }
                // 벽 생성
                else if (Random.Range(0, 100) < wallRate)
                {
                    PlaceWallCluster(pos, Random.Range(2, 5));
                }
            }
        }
    }

    bool IsNearEdge(int x, int y)
    {
        // 청크 내에서 가장자리일 때만 물 가능
        return (x < 3 || y < 3 || x > chunkSize - 4 || y > chunkSize - 4);
    }

    void SpreadWater(Vector3Int center, int radius)
    {
        for (int dx = -radius; dx <= radius; dx++)
        {
            for (int dy = -radius; dy <= radius; dy++)
            {
                if (dx * dx + dy * dy <= radius * radius)
                {
                    Vector3Int pos = center + new Vector3Int(dx, dy, 0);
                    if (!tilemap.HasTile(pos))
                    {
                        blockedTiles.Add(new Vector2Int(pos.x, pos.y)); // ✅ 물 → blocked
                        tilemap.SetTile(pos, waterTile);
                    }
                }
            }
        }
    }

    void PlaceWallCluster(Vector3Int center, int count)
    {
        for (int i = 0; i < count; i++)
        {
            Vector3Int offset = center + new Vector3Int(
                Random.Range(-1, 2), Random.Range(-1, 2), 0
            );

            if (!tilemap.HasTile(offset))
            {
                tilemap.SetTile(offset, wallTile);
                blockedTiles.Add(new Vector2Int(offset.x, offset.y)); // ✅ A*에 반영
            }
        }
    }
}
