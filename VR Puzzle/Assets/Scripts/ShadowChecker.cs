using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using UnityEngine;

public class ShadowChecker : MonoBehaviour
{
    [SerializeField] private int winAmount;
    [SerializeField] private float successPercentage = 0.9f;
    [SerializeField] private int layerMask;
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
    private bool puzzleObjectHit;
    private bool grabbableObjectHit;
    private LayerMask hitGameObjectLayer;
    
    [ReadOnly(true)]
    public int correctHits;
    // public int requiredLayers;


    private void Start()
    {
        scale = castAreaLength / cols;
        mask = LayerMask.GetMask("Grabbable", "PuzzleObject");
        puzzleObjectLayer = LayerMask.GetMask("PuzzleObject");
        grabbableObjectLayer = LayerMask.GetMask("Grabbable");
        // grabbableObjectLayer = 1 << grabbableObjectLayer;
        // requiredLayers = puzzleObjectLayer | grabbableObjectLayer;

        for (int i = -cols; i < cols; i++)
        {
            for (int j = -rows; j < rows; j++)
            {
                raycastStartPositions.Add(new Vector3(i, j, 0) * scale + transform.position + new Vector3(0, 0, offSetZ));
            }
        }
        CalculateWinAmount();
    }

    private void CalculateWinAmount()
    {
        winAmount = -ThrowRayCasts();
    }

    private int ThrowRayCasts()
    {
        correctHits = 0;
        foreach (Vector3 raycastStartPosition in raycastStartPositions)
        {
            hits = Physics.RaycastAll(raycastStartPosition, transform.forward, rayCastLength, mask);
            puzzleObjectHit = false;
            grabbableObjectHit = false;
            for (int i = 0; i < hits.Length; i++)
            {
                hit = hits[i];
                hitGameObjectLayer = hit.collider.gameObject.layer;
                // Debug.Log(1 << hit.collider.gameObject.layer == LayerMask.GetMask("Grabbable"));
                puzzleObjectHit = 1 << hitGameObjectLayer == puzzleObjectLayer || puzzleObjectHit;
                grabbableObjectHit = 1 << hitGameObjectLayer == grabbableObjectLayer || grabbableObjectHit;
            }
            if (puzzleObjectHit && grabbableObjectHit)
            {
                Debug.DrawLine(raycastStartPosition, raycastStartPosition + transform.forward * 2, Color.green);
                correctHits++;
            }
            else if (puzzleObjectHit && !grabbableObjectHit)
            {
                Debug.DrawLine(raycastStartPosition, raycastStartPosition + transform.forward * 2, Color.yellow);
                correctHits--;
            }
            else if (!puzzleObjectHit && grabbableObjectHit)
            {
                Debug.DrawLine(raycastStartPosition, raycastStartPosition + transform.forward * 2, Color.yellow);
                correctHits--;
            }
            else
            {
                Debug.DrawLine(raycastStartPosition, raycastStartPosition + transform.forward * 2, Color.red);
            }
        }
        return correctHits;
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
        return ThrowRayCasts() >= winAmount * successPercentage;
    }
}