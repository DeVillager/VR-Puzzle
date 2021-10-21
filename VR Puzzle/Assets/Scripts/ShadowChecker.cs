using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class ShadowChecker : MonoBehaviour
{
    [SerializeField] private bool initWinAmountOnStartUp;
    public int winAmount;
    public float successPercentage = 0.9f;
    [SerializeField] private int cols = 5;
    [SerializeField] private int rows = 5;
    [SerializeField] private float offSetZ;

    [SerializeField] private float castAreaLength = 1f;
    [SerializeField] private float rayCastLength = 2f;
    [SerializeField] private List<Vector3> raycastStartPositions;
    [SerializeField] private List<LineRenderer> lineRenderers;

    [SerializeField] private int puzzleObjectLayer;
    [SerializeField] private int grabbableObjectLayer;

    private float scale;
    private LayerMask mask;
    private RaycastHit[] hits;
    private RaycastHit hit;
    private bool silhouetteHit;
    private bool puzzleObjectHit;
    private LayerMask hitGameObjectLayer;

    [ReadOnly(true)] public int silhouetteHits;
    [ReadOnly(true)] public int puzzleObjectHits;
    [ReadOnly(true)] public int silhouetteAndPuzzleObjectHits;
    [ReadOnly(true)] public int onlyPuzzleObjectHits;
    [ReadOnly(true)] public int correctHits;
    [SerializeField] private float punishValue = 0.2f;

    private void Start()
    {
        mask = LayerMask.GetMask("Grabbable", "PuzzleObject");
        puzzleObjectLayer = LayerMask.GetMask("PuzzleObject");
        grabbableObjectLayer = LayerMask.GetMask("Grabbable");

        InitRaycastPositions();

        if (initWinAmountOnStartUp)
        {
            CalculateWinAmount();
        }
    }

    private void CalculateWinAmount()
    {
        winAmount = ThrowRayCasts(true);
        Debug.Log("Win amount calculated");
    }
    
    public void InitRaycastPositions()
    {
        raycastStartPositions.Clear();
        scale = castAreaLength / cols;
        int halfCols = (int)Mathf.Floor(cols / 2f);
        int halfRows = (int)Mathf.Floor(rows / 2f);
        for (int i = -halfCols; i <= halfCols; i++)
        {
            for (int j = -halfRows; j <= halfRows; j++)
            {
                raycastStartPositions.Add(
                    new Vector3(i, j, 0) * scale + transform.position + new Vector3(0, 0, offSetZ));
            }
        }
        Debug.Log("Raycast positions updated");
    }

    private int ThrowRayCasts(bool init)
    {
        silhouetteHits = 0;
        puzzleObjectHits = 0;
        silhouetteAndPuzzleObjectHits = 0;
        onlyPuzzleObjectHits = 0;
        foreach (Vector3 raycastStartPosition in raycastStartPositions)
        {
            hits = Physics.RaycastAll(raycastStartPosition, transform.forward, rayCastLength, mask);
            silhouetteHit = false;
            puzzleObjectHit = false;
            for (int i = 0; i < hits.Length; i++)
            {
                hit = hits[i];
                hitGameObjectLayer = hit.collider.gameObject.layer;
                silhouetteHit = 1 << hitGameObjectLayer == puzzleObjectLayer || silhouetteHit;
                puzzleObjectHit = 1 << hitGameObjectLayer == grabbableObjectLayer || puzzleObjectHit;
            }
            if (silhouetteHit) silhouetteHits++;
            if (puzzleObjectHit) puzzleObjectHits++;
            if (silhouetteHit && puzzleObjectHit) silhouetteAndPuzzleObjectHits++;
            if (!silhouetteHit && puzzleObjectHit) onlyPuzzleObjectHits++;

            DrawDebugLines(raycastStartPosition);
        }

        correctHits = silhouetteAndPuzzleObjectHits - Mathf.RoundToInt(onlyPuzzleObjectHits * punishValue);
        return init ? silhouetteHits : correctHits;
    }
    
    private void DrawDebugLines(Vector3 raycastStartPosition)
    {
        if (silhouetteHit && puzzleObjectHit)
        {
            Debug.DrawLine(raycastStartPosition, raycastStartPosition + transform.forward * 2, Color.green);
        }
        else if (silhouetteHit)
        {
            Debug.DrawLine(raycastStartPosition, raycastStartPosition + transform.forward * 2, Color.yellow);
        }
        else if (puzzleObjectHit)
        {
            Debug.DrawLine(raycastStartPosition, raycastStartPosition + transform.forward * 2, Color.yellow);
        }
        else // !silhouetteHit && !puzzleObjectHit
        {
            Debug.DrawLine(raycastStartPosition, raycastStartPosition + transform.forward * 2, Color.red);
        }
    }

    void Update()
    {
        if (CheckWinCondition())
        {
            LevelClear();
        }
    }

    private void LevelClear()
    {
        GameManager.instance.LevelClear();
    }

    private bool CheckWinCondition()
    {
        return ThrowRayCasts(false) >= winAmount * successPercentage;
    }
}