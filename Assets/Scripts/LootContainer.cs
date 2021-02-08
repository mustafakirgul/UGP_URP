﻿using System;
using System.Collections;
using Normal.Realtime;
using UnityEngine;

public class LootContainer : MonoBehaviour
{
    private Loot content => GetComponent<Loot>();
    public GameObject loot, pickup;
    public int id, collectedBy;
    public float dieDelay = 1f;
    private RealtimeView _realtime => GetComponent<RealtimeView>();
    private Transform selection;
    private bool animate;

    [SerializeField] private bool isNetworkInstance;

    private void OnDrawGizmos()
    {
        if (loot == null || pickup == null) return;
        if (!loot.activeInHierarchy && !pickup.activeInHierarchy)
        {
            Gizmos.DrawCube(transform.position, new Vector3(2.2f, 2.2f, 2.2f));
        }
    }

    private void Start()
    {
        isNetworkInstance = !_realtime.isOwnedLocallyInHierarchy;
        if (isNetworkInstance)
            GetComponent<Rigidbody>().isKinematic = true;
        animate = GetComponent<PowerUpMeshGetter>() == null;
    }

    private void Update()
    {
        if (content == null) return;
        content.Update();
        if (selection == null && animate) return;
        selection.localEulerAngles = new Vector3(0, selection.localEulerAngles.y + (180 * Time.deltaTime), 0);
    }

    public int SetID(int _id)
    {
        content.SetID(_id);
        id = _id;
        if (loot == null)
            loot = transform.GetChild(0).gameObject;
        if (pickup == null)
            pickup = transform.GetChild(1).gameObject;
        loot.SetActive(id > 0);
        pickup.SetActive(id < 0);
        selection = loot.activeInHierarchy ? loot.transform : pickup.transform;
        return _id;
    }

    private int SetCollectedBy(int _collectedBy)
    {
        collectedBy = content.SetCollectedBy(_collectedBy);
        return collectedBy;
    }

    public int GetCollected(int _collectorID)
    {
        if (content.collectedBy == 0)
        {
            GetComponent<Rigidbody>().isKinematic = true;
            GetComponent<BoxCollider>().enabled = false;
            StartCoroutine(CR_Die());
            SetCollectedBy(_collectorID);
            return id; // return id of collected item
        }

        return
            0; //return 0 meaning the item was already collected by someone and pending to be destroyed from the game world
    }

    public IEnumerator CR_Die()
    {
        yield return new WaitForSeconds(dieDelay);
        if (!isNetworkInstance) Realtime.Destroy(gameObject);
    }
}