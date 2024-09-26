using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitionHandler : MonoBehaviour
{
    static public SceneTransitionHandler sceneTransitionHandler {  get; internal set; }
    [SerializeField] private string DefaultMainMenu = "StartMenu";

    [HideInInspector]
    public delegate void ClientLoadedSceneDelegateHandler(ulong clientId);
    [HideInInspector]
    public event ClientLoadedSceneDelegateHandler OnClientLoadedScene;

    [HideInInspector]
    public delegate void SceneStateChangedDelegateHandler(SceneStates newState);
    [HideInInspector]
    public event SceneStateChangedDelegateHandler OnSceneStateChanged;

    public enum SceneStates
    {
        Init,
        Start,
        Lobby,
        InGame
    }
    private SceneStates _sceneState;
    private int _numberOfClientLoaded;
    void Awake()
    {
        if (sceneTransitionHandler != this && sceneTransitionHandler != null)
        {
            Destroy(sceneTransitionHandler.gameObject);
        }
        sceneTransitionHandler = this;
        SetSceneState(SceneStates.Init);
        DontDestroyOnLoad(this);
    }

    private void SetSceneState(SceneStates sceneState)
    {
        _sceneState = sceneState;
        if (OnSceneStateChanged != null)
        {
            OnSceneStateChanged.Invoke(_sceneState);
        }
    }
    public SceneStates GetCurrentSceneStates()
    {
        return _sceneState;
    }
    // Update is called once per frame
    void Start()
    {
        if (_sceneState == SceneStates.Init)
        {
            SceneManager.LoadScene(DefaultMainMenu);
        }
    } 
    public void RegisterCallbacks()
    {
        NetworkManager.Singleton.SceneManager.OnLoadComplete += OnLoadComplete;
    }
    private void OnLoadComplete(ulong clientId, string sceneName, LoadSceneMode loadSceneMode)
    {
        _numberOfClientLoaded += 1;
        OnClientLoadedScene?.Invoke(clientId);
    }
    public bool AllClientAreLoaded()
    {
        return _numberOfClientLoaded == NetworkManager.Singleton.ConnectedClients.Count;
    }
    public void SwitchScene(string sceneName)
    {
        if (NetworkManager.Singleton.IsListening)
        {
            _numberOfClientLoaded = 0;
            NetworkManager.Singleton.SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
        }
        else
        {
            SceneManager.LoadSceneAsync(sceneName);
        }
    }
    public void ExitAndLoadStartMenu()
    {
        if (NetworkManager.Singleton != null && NetworkManager.Singleton.SceneManager != null) 
        {
            NetworkManager.Singleton.SceneManager.OnLoadComplete -= OnLoadComplete;
        }
        OnClientLoadedScene = null;
        SetSceneState(SceneStates.Start);
        SceneManager.LoadScene(1);
    }
}
