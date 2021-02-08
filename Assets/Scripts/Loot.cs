﻿using System;
using Normal.Realtime;
using UnityEngine;

public class Loot : RealtimeComponent<LootModel>
{
    [Space(10)] public int id;
    public int collectedBy;
    private LootContainer _container => transform.GetComponent<LootContainer>();

    protected override void OnRealtimeModelReplaced(LootModel previousModel, LootModel currentModel)
    {
        base.OnRealtimeModelReplaced(previousModel, currentModel);

        if (previousModel != null)
        {
            previousModel.idDidChange -= IdDidChange;
            previousModel.collectedByDidChange -= CollectedByDidChange;
        }

        if (currentModel != null)
        {
            if (currentModel.isFreshModel)
            {
                id = model.id;
                collectedBy = model.collectedBy;
            }

            currentModel.idDidChange += IdDidChange;
            currentModel.collectedByDidChange += CollectedByDidChange;
        }
    }

    private void Start()
    {
        if (id == 0) return;
        SetID(id);
    }

    public void Update()
    {
        id = model.id;
        collectedBy = model.collectedBy;
    }

    public void SetID(int _id)
    {
        if (model != null)
            model.id = _id;
    }

    public int SetCollectedBy(int _collectedBy)
    {
        if (_collectedBy > 0 && model.collectedBy == 0)
        {
            model.collectedBy = _collectedBy;
        }

        return model.collectedBy;
    }

    private void CollectedByDidChange(LootModel model, int value)
    {
        CollectedByChanged();
    }

    private void CollectedByChanged()
    {
        collectedBy = model.collectedBy;
    }

    private void IdDidChange(LootModel model, int value)
    {
        IDChanged();
    }

    private void IDChanged()
    {
        id = model.id;
    }
}