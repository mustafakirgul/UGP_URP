﻿using System.Collections;
using Normal.Realtime;
using UnityEngine;

[RequireComponent(typeof(RealtimeView))]
public class Melee : MonoBehaviour
{
    public int originalOwnerID;
    public float weight;
    public float meleePower;
    public float armorFactor;
    public Transform parent;
    public Melee opponent;
    public ParticleSystem crashParticle;
    private LayeredAudioPlayer LAP;
    public NewCarController controller, opponentController;
    public Rigidbody carRB, rb;
    public WaitForSeconds wait;
    public RealtimeView rt;
    public StatsEntity statsEntity;
    public Player player;
    public Realtime realtime => FindObjectOfType<Realtime>();
    public float testMeleeForce = 666f;
    private WaitForSeconds meleeDelay;
    public AudioPlayer lootSound;

    private void Start()
    {
        rt = GetComponent<RealtimeView>();
        originalOwnerID = rt.ownerIDInHierarchy;
        meleeDelay = new WaitForSeconds(.333f);
        if (rt.isOwnedRemotelyInHierarchy)
        {
            foreach (var c in FindObjectsOfType<NewCarController>())
            {
                if (c._realtimeView.ownerIDInHierarchy == originalOwnerID)
                {
                    Setup(c);
                }
            }
        }
    }

    public void Setup(NewCarController _controller)
    {
        parent = _controller.transform;
        transform.parent = null;
        controller = _controller;
        player = _controller._player;
        meleePower = controller.meleeDamageModifier;
        armorFactor = player.armourDefenseModifier;
        statsEntity = player.statsEntity;
        carRB = controller.CarRB;
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;
        wait = new WaitForSeconds(.25f);
        if (crashParticle == null) crashParticle = GetComponentInChildren<ParticleSystem>();
        LAP = GetComponent<LayeredAudioPlayer>();
    }

    private void Update()
    {
        if (parent == null || rb == null) return;
        rb.MovePosition(parent.position);
        rb.MoveRotation(parent.rotation);
    }

    public Vector3 ReturnVelocity()
    {
        return carRB.velocity;
    }

    private IEnumerator OnTriggerEnter(Collider other)
    {
        opponent = other.transform.GetComponent<Melee>();
        if (opponent != null)
            opponentController = opponent.controller;
        else
            yield break;
        if (controller == null) yield break;
        LAP.Play();
        if (controller.isBoosting)
        {
            HitOther();
            crashParticle.Play();

            yield return wait;
            crashParticle.Stop();
        }
        else if (opponentController.isBoosting)
        {
            StartCoroutine(GetHit());
            crashParticle.Play();
            yield return wait;
            crashParticle.Stop();
        }
    }

    private void HitOther()
    {
        if (statsEntity == null) statsEntity = player.statsEntity;

        StatsEntity opponentStatsEntity =
            StatsManager.instance.ReturnStatsEntityById(opponentController._realtimeView.ownerIDInHierarchy);

        Debug.LogWarning("Grabbed opponent stats entity: " + opponentStatsEntity);

        if (controller._realtimeView.isOwnedLocallyInHierarchy)
            carRB.AddForce((opponent.transform.position - transform.position).normalized * (testMeleeForce * .33f));

        //Debug.LogWarning("Before Melee | Local loot: " + player.statsEntity._loot + "Remote loot: " + opponentStatsEntity._loot);

        if (opponentStatsEntity._loot > 0)
        {
            statsEntity.ReceiveStat(StatType.loot);
            lootSound.PlayIndex(0);
        }

        controller.currentAmmo++;

        controller.RegisterDamage(10f, controller._realtimeView);

        //Debug.LogWarning("After Melee | Local loot: " + player.statsEntity._loot + "Remote loot: " + opponentStatsEntity._loot);
    }

    private IEnumerator GetHit()
    {
        if (statsEntity == null) statsEntity = player.statsEntity;
        if (controller._realtimeView.isOwnedLocallyInHierarchy)
            carRB.AddForce((transform.position - opponent.transform.position).normalized * testMeleeForce);

        yield return meleeDelay;

        if (statsEntity._loot > 0)
        {
            statsEntity.LoseLoot();
            lootSound.PlayIndex(1);
        }

        controller.RegisterDamage(10f * opponent.player.meleeModifier, controller._realtimeView);
    }
}