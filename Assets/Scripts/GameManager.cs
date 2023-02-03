﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance {get; private set;}
    
    public static int s_CurrentLevel = 0;

    public static int s_MaxAvailableLevel = 5;

    // The value of -1 means no hats have been purchased
    public static int s_ActiveHat = 0;

    public AssetReference AssetReference;
    [SerializeField] private Image gameLogoImage;

    public void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void OnEnable()
    {
        s_CurrentLevel = 0;

        AsyncOperationHandle<Sprite> asyncOperationHandle = Addressables.LoadAssetAsync<Sprite>(AssetReference);
        asyncOperationHandle.Completed += LogoOperationHandle_Completed;
    }

    private void LogoOperationHandle_Completed(AsyncOperationHandle<Sprite> asyncOperationHandle)
    {
        Debug.Log("AsyncOperationHandle Status: " + asyncOperationHandle.Status);

        if (asyncOperationHandle.Status == AsyncOperationStatus.Succeeded)
        {
            gameLogoImage.sprite = asyncOperationHandle.Result;
        }
    }

    public void ExitGame()
    {
        s_CurrentLevel = 0;
    }

    public void SetCurrentLevel(int level)
    {
        s_CurrentLevel = level;
    }

    public static void LoadNextLevel()
    {
        Addressables.LoadSceneAsync("LoadingScene");
    }

    public static void LevelCompleted()
    {
        s_CurrentLevel++;

        // Just to make sure we don't try to go beyond the allowed number of levels.
        s_CurrentLevel = s_CurrentLevel % s_MaxAvailableLevel;

        LoadNextLevel();
    }

    public static void ExitGameplay()
    {
        Addressables.LoadSceneAsync("MainMenu");
    }
}
