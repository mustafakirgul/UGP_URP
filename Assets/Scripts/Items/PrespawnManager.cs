﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Normal.Realtime;

public class PrespawnManager : MonoBehaviour
{
    [SerializeField] List<GameObject> PrespawnedItems = new List<GameObject>();


    [SerializeField] List<Vector3> SpawnCoordinates = new List<Vector3>();

    // Start is called before the first frame update
    public Realtime _realtime;

    void Awake()
    {
        //_realtime = FindObjectOfType<Realtime>();
        ReferenceItemsToSpawn();
    }

    private void ReferenceItemsToSpawn()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            PrespawnedItems.Add(this.transform.GetChild(i).gameObject);
        }
    }

    public void SpawnPredeterminedItems()
    {
        foreach (GameObject PowerUp in PrespawnedItems)
        {
            SpawnItemInGameWorld(PowerUp.GetComponent<PreSpawnSelector>().PUSpawnReferencePoint,
                PowerUp.GetComponent<Loot>().id);
        }
    }

    private void SpawnItemInGameWorld(Vector3 lootLocation, int lootID)
    {
        {
            GameObject _temp = Realtime.Instantiate("PredeterminedDrop",
                position: lootLocation,
                rotation: Quaternion.identity,
                ownedByClient:
                true,
                preventOwnershipTakeover:
                false,
                destroyWhenOwnerOrLastClientLeaves: true,
                useInstance:
                _realtime);
            GameManager.instance.RecordRIGO(_temp);
            _temp.GetComponent<LootContainer>().SetID(lootID);
            _temp.transform.position = lootLocation;
        }
    }

    public void DeActivateSpawner()
    {
        this.gameObject.SetActive(false);
    }

    public void ReActivateSpawner()
    {
        this.gameObject.SetActive(true);
    }
}