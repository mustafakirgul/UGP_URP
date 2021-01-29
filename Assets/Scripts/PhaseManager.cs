﻿using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using Normal.Realtime;

public class PhaseManager : MonoBehaviour
{
    public List<Phase> phases;
    public int phase;
    public TextMeshProUGUI messageTMPro;
    public UnityEvent endOfPhasesEvent;
    private bool timer, displayMessage;
    private float wait, waitForMessage;

    [SerializeField] private GameObject[] Walls;

    public void StartPhaseSystem()
    {
        //Debug.LogWarning("Phase system Start");
        if (GameManager.instance.lootTruck.isOwnedLocallyInHierarchy)
            JumpToPhase(0);
    }

    public void NextPhase() //called locally by the local game manager, depending on conditions
    {
        //Debug.LogWarning("Previous Phase: " + phase);
        if (!GameManager.instance.lootTruck.isOwnedLocallyInHierarchy) return;
        phase++;
        if (phase == phases.Count)
        {
            endOfPhasesEvent.Invoke();
            phase = -1;
            GameManager.instance._race.ChangePhase(phase);
        }
        else if (phase < phases.Count)
        {
            //Debug.LogWarning("Current Phase: " + phase);
            GameManager.instance._race.ChangePhase(phase);
        }
    }

    public void JumpToPhase(int newPhase) //called by the network instance if the phase number is changed
    {
        phase = newPhase;
        phases[phase].startEvent.Invoke();
        //Debug.LogWarning("Start Event of Phase " + phase);
        if (phases[phase].startMessage.Length > 0)

            DisplayMessage(phases[phase].startMessage);
        if (phases[phase].type == PhaseType.timeBased
        ) //if time based set up the timer - if not, just wait for someone to trigger the transition
            StartTimer(phases[phase].duration);
    }

    private void StartTimer(float duration)
    {
        Invoke("NextPhase", duration);
    }

    private void DisplayMessage(string message)
    {
        if (!displayMessage)
        {
            displayMessage = true;
            messageTMPro.SetText(message);
            Invoke("ClearMessage", message.Length * .1f);
        }
    }

    private void ClearMessage()
    {
        messageTMPro.ClearMesh();
        displayMessage = false;
    }
}

[Serializable]
public struct Phase
{
    public string startMessage;
    public PhaseType type;
    [Tooltip("If phase is time based")] public float duration; //if timed phase

    [Tooltip("Triggers when phase starts")]
    public UnityEvent startEvent;
}

public enum PhaseType
{
    timeBased,
    conditionBased
}