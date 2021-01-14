﻿using Normal.Realtime;
using TMPro;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class NetworkManager : RealtimeComponent<GameManagerModel>
{
    private Realtime _realtime;
    public Vector3 minimum, maximum;
    Vector3 spawnPoint;
    public TextMeshProUGUI playerNameInputField;
    public Canvas _enterNameCanvas;
    public Camera _miniMapCamera;
    public string preferredCar;
    string _tempName;
    ChaseCam chaseCam;

    //Game Manager Params
    public float m_fMaxTimer;
    public float m_localTimer;
    public int m_iNumOfPlayersForGameStart;
    [SerializeField]
    public GameManagerModel _model;
    public double m_fGameStartTime;
    public float m_fGameDuration;
    private PlayerManager playerManager;
    private UIManager uIManager;
    bool readyToStart;

    private GameSceneManager gameSceneManager;

    //bool isConnected;

    private void Awake()
    {
        gameSceneManager = FindObjectOfType<GameSceneManager>();
        chaseCam = GameObject.FindObjectOfType<ChaseCam>();
        playerManager = FindObjectOfType<PlayerManager>();
        uIManager = FindObjectOfType<UIManager>();
        _enterNameCanvas.gameObject.SetActive(true);
        // Get the Realtime component on this game object
        _realtime = GetComponent<Realtime>();

        // Notify us when Realtime successfully connects to the room
        _realtime.didConnectToRoom += DidConnectToRoom;
        _realtime.didDisconnectFromRoom += DidDisconnectFromRoom;
        spawnPoint = new Vector3(
            UnityEngine.Random.Range(minimum.x, maximum.x),
            UnityEngine.Random.Range(minimum.y, maximum.y),
            UnityEngine.Random.Range(minimum.z, maximum.z)
        );
        StartCoroutine(gameSceneManager.FadeToBlackOutSquare(false, 2));
    }

    private void Start()
    {
        //StartCoroutine(gameSceneManager.FadeToBlackOutSquare(false, 2));
    }

    private void PlayerCountDownCheck()
    {
        //Only start this coroutine if there are 
        if (playerManager && playerManager.connectedPlayers.Count >= m_iNumOfPlayersForGameStart)
        {
            readyToStart = true;
            StartCoroutine(CountDownTimeContinously());
        }
    }
    private void DidDisconnectFromRoom(Realtime realtime)
    {
        chaseCam.ResetCam();
    }

    protected override void OnRealtimeModelReplaced(GameManagerModel previousModel, GameManagerModel currentModel)
    {
        if (previousModel != null)
        {
            // Unregister from events
            previousModel.currentGameTimerDidChange -= GameTimerChange;
            previousModel.currentSceneNumberDidChange -= SceneNumberChange;
        }

        if (currentModel != null)
        {
            //First time a model runs set timer to max
            if (currentModel.isFreshModel)
            {
                m_fGameStartTime = currentModel.currentGameTimer;
            }
            currentModel.currentGameTimerDidChange += GameTimerChange;
            currentModel.currentSceneNumberDidChange += SceneNumberChange;

            //Update current model of player when applicable
            _model = currentModel;
        }
    }

    private void Update()
    {
        if (_model != null && readyToStart)
        {
            if (_model.currentGameTimer == 0)
            {
                m_fGameStartTime = _realtime.room.time;
                _model.currentGameTimer = m_fGameStartTime;
            }
            else
            {
                m_fGameStartTime = _model.currentGameTimer;
            }
        }
    }
    private void GameTimerChange(GameManagerModel model, double Time)
    {
        m_fGameStartTime = Time;
    }

    private void SceneNumberChange(GameManagerModel model, int SceneNumber)
    {
        //Not used
    }
    //private void Update()
    //{
    //    if (isConnected)
    //    {
    //        NetworkInfo _networkInfo = _realtime.room.GetNetworkStatistics();
    //        Debug.Log("Roundtrip Time: " + _networkInfo.roundTripTime + " | SBw: " + _networkInfo.sentBandwidth + " | RBw: " + _networkInfo.receivedBandwidth + " | LostPac%: " + _networkInfo.percentOfPacketsLost);
    //        Debug.Log("_______________________");
    //        Debug.Log(_networkInfo.ToString());
    //        Debug.Log("_______________________");
    //    }
    //}
    public void ConnectToRoom(int _selection)
    {
        switch (_selection)
        {
            case 1:
                preferredCar = "Car1";
                break;
            case 2:
                preferredCar = "Car2";
                break;
            case 3:
                preferredCar = "Car3";
                break;
            default:
                break;
        }
        if (playerNameInputField.text.Length > 0)
        {
            _realtime.Connect("UGP_TEST");
            StartCoroutine(gameSceneManager.FadeInAndOut(3, 1));
        }
    }
    private void DidConnectToRoom(Realtime realtime)
    {
        //isConnected = true;
        _tempName = preferredCar != "" ? preferredCar : "Car1";
        GameObject _temp = Realtime.Instantiate(_tempName,
                            position: spawnPoint,
                            rotation: Quaternion.identity,
                       ownedByClient: true,
            preventOwnershipTakeover: true,
                         useInstance: _realtime);

        if (_temp.GetComponent<NewCarController>()._realtime)
        {
            _temp.GetComponent<NewCarController>()._realtime = _realtime;
        }
        else
        {
            _temp.GetComponent<NewCarController>()._realtime = _realtime;
        }
        _temp.GetComponent<Player>().SetPlayerName(playerNameInputField.text);
        FindObjectOfType<MiniMapCamera>()._master = _temp.transform;
        _enterNameCanvas.gameObject.SetActive(false);
        _miniMapCamera.enabled = true;
        playerManager.AddExistingPlayers();
        StartCoroutine(DelayPlayerCountCheck(2));
    }

    private IEnumerator DelayPlayerCountCheck(int DelayTime)
    {
        yield return new WaitForSeconds(DelayTime);
        PlayerCountDownCheck();
    }
    private IEnumerator CountDownTimeContinously()
    {
        while (true)
        {
            TimerCountDown();
            yield return null;
        }
    }
    private void TimerCountDown()
    {
        double _temp = m_fGameDuration - (_realtime.room.time - m_fGameStartTime);
        Debug.Log("Remaining Time: " + _temp);
        uIManager.remainingTime = _temp;
        //Update the timer for all managers instances
        if (m_fMaxTimer <= 0)
        {
            //SceneTransition Commence Logic should be here
            //Fade out of scene first
            StartCoroutine(gameSceneManager.FadeToBlackOutSquare(true, 1));
            StartCoroutine(gameSceneManager.DelaySceneTransiton(4f,
                SceneManager.GetActiveScene().buildIndex + 1));
        }
    }
}
public struct GameWinConditions
{
    //Parameters to fulfill winconditions
    public int winConIndex;
    public int playerIDtoAward;
    public bool isCompleted;
}

