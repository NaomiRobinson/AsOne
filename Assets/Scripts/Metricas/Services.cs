using System;
using Unity.Services.Core;
using Unity.Services.Analytics;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Services : MonoBehaviour
{
    // [SerializeField] private SceneController sceneController;

    async void Awake()
    {
        try
        {
            await UnityServices.InitializeAsync();
        }
        catch (Exception e)
        {
            Debug.LogException(e);
        }
    }

    public void StartDataCollection()
    {
        AnalyticsService.Instance.StartDataCollection();
        SceneManager.LoadScene("Menu"); //sceneControllerStartGame()
    }

    public void StopDataCollection()
    {
        SceneManager.LoadScene("Menu"); //sceneControllerStartGame()
    }
}