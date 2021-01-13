﻿using Normal.Realtime;
using TMPro;
using UnityEngine;

public class NetworkManager : MonoBehaviour
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

    //bool isConnected;

    private void Awake()
    {
        chaseCam = GameObject.FindObjectOfType<ChaseCam>();
        _enterNameCanvas.gameObject.SetActive(true);
        // Get the Realtime component on this game object
        _realtime = GetComponent<Realtime>();

        // Notify us when Realtime successfully connects to the room
        _realtime.didConnectToRoom += DidConnectToRoom;
        _realtime.didDisconnectFromRoom += DidDisconnectFromRoom;
        spawnPoint = new Vector3(
            Random.Range(minimum.x, maximum.x),
            Random.Range(minimum.y, maximum.y),
            Random.Range(minimum.z, maximum.z)
        );
    }

    private void DidDisconnectFromRoom(Realtime realtime)
    {
        chaseCam.ResetCam();
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
                preferredCar = "NewCar1";
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
        }
    }
    private void DidConnectToRoom(Realtime realtime)
    {
        //isConnected = true;
        _tempName = preferredCar != "" ? preferredCar : "Car";
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
    }
}
