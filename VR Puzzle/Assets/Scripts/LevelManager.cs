using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private Scene[] levels;

    private void Awake()
    {
        InitLevels();
        SetClearedLevels();
    }

    private void InitLevels()
    {
    }

    private void SetClearedLevels()
    {

    }
}
