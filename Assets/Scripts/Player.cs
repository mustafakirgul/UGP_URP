﻿using Normal.Realtime;
using UnityEngine;

public class Player : RealtimeComponent<PlayerModel>
{
    public int _id;
    public string playerName;
    public float playerHealth;
    public float maxPlayerHealth;
    public Vector3 explosionForce;
    public float armourDefenseModifier = 0f;
    public float healModifier = 0f;
    protected override void OnRealtimeModelReplaced(PlayerModel previousModel, PlayerModel currentModel)
    {
        if (previousModel != null)
        {
            // Unregister from events
            previousModel.playerNameDidChange -= PlayerNameChanged;
            previousModel.healthDidChange -= PlayerHealthChanged;
            previousModel.forcesDidChange -= PlayerForcesChanged;
        }
        if (currentModel != null)
        {
            // If this is a model that has no data set on it, populate it with the current mesh renderer color.
            // use [ if (currentModel.isFreshModel)] to initialize player prefab
            playerName = currentModel.playerName;
            currentModel.playerNameDidChange += PlayerNameChanged;
            currentModel.healthDidChange += PlayerHealthChanged;
            currentModel.forcesDidChange += PlayerForcesChanged;
        }
    }

    public void SetPlayerName(string _name)
    {
        if (_name.Length > 0)
        {
            model.playerName = _name;
        }
    }

    public void ChangeExplosionForce(Vector3 _origin)
    {
        model.forces = _origin;
    }

    public void DamagePlayer(float damage)
    {
        model.health -= ((1 - armourDefenseModifier)* damage);
        //PlayerHealthChanged(_model, (playerHealth - damage));
    }

    public void HealPlayer(float healingPower)
    {
        model.health += ((1 + healModifier) * healingPower);
        //PlayerHealthChanged(_model, (playerHealth + healingPower));
    }

    private void PlayerHealthChanged(PlayerModel playerModel, float value)
    {
        playerHealth = value;
    }

    private void PlayerForcesChanged(PlayerModel playerModel, Vector3 value)
    {
        explosionForce = value;
    }

    private void PlayerNameChanged(PlayerModel playerModel, string value)
    {
        playerName = value;
    }
}
