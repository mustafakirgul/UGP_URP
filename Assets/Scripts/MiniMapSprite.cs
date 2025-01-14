﻿using Normal.Realtime;
using UnityEngine;

public class MiniMapSprite : MonoBehaviour
{
    Transform _parent;
    Vector3 _origin;
    [SerializeField] private SpriteRenderer local, network;
    public bool networked; //meaning it has 2 different sprites for local and network instance versions of the object

    void Awake()
    {
        _parent = transform.parent;
        _origin = transform.rotation.eulerAngles;
        if (networked) return;
        if (local == null) local = transform.GetChild(0).GetComponent<SpriteRenderer>();
        local.enabled = false;
        if (network != null) network = transform.GetChild(1).GetComponent<SpriteRenderer>();
        network.enabled = false;
    }

    private void Start()
    {
        if (_parent.GetComponent<Truck>() != null) return;
        local.enabled = _parent.GetComponent<RealtimeView>().isOwnedLocallyInHierarchy;
        network.enabled = _parent.GetComponent<RealtimeView>().isOwnedRemotelyInHierarchy;
    }

    void Update()
    {
        if (transform != null && _parent != null)
        {
            transform.rotation = Quaternion.Euler(
                _origin.x,
                _parent.rotation.eulerAngles.y,
                _origin.z);
        }
    }
}