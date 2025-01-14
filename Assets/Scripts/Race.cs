﻿using System;
using Normal.Realtime;

public class Race : RealtimeComponent<RaceModel>
{
    public double m_fGameStartTime;
    public float m_fRaceDuration;
    public int m_iPhase;
    public bool m_isOn;
    public int countDown;

    protected override void OnRealtimeModelReplaced(RaceModel previousModel, RaceModel currentModel)
    {
        if (previousModel != null)
        {
            previousModel.gameStartTimeDidChange -= GameTimeChanged;
            previousModel.phaseDidChange -= PhaseChanged;
            previousModel.isOnDidChange -= IsOnChanged;
        }

        if (currentModel != null)
        {
            if (currentModel.isFreshModel)
            {
                currentModel.phase = m_iPhase;
                currentModel.isOn = m_isOn;
                currentModel.countDown = GameManager.instance.counter != null
                    ? GameManager.instance.counter.start
                    : FindObjectOfType<CountdownLights>().lights.Length;
            }

            currentModel.gameStartTimeDidChange += GameTimeChanged;
            currentModel.phaseDidChange += PhaseChanged;
            currentModel.isOnDidChange += IsOnChanged;
        }
    }

    private void Update()
    {
        m_iPhase = model.phase;
        m_isOn = model.isOn;
        countDown = model.countDown;
    }

    public int CountOneDown()
    {
        if (model == null) return -1;
        if (model.countDown > 0)
        {
            model.countDown--;
        }

        return model.countDown;
    }

    public void ChangeIsOn(bool state)
    {
        model.isOn = state;
    }

    public void ChangePhase(int phase)
    {
        model.phase = phase;
    }

    private void PhaseChanged(RaceModel raceModel, int phase)
    {
        m_iPhase = phase;
        GameManager.instance.phaseManager.JumpToPhase(phase);
    }

    public void ChangeGameTime(double time)
    {
        model.gameStartTime = time;
    }

    private void GameTimeChanged(RaceModel raceModel, double value)
    {
        m_fGameStartTime = value;
    }

    private void IsOnChanged(RaceModel raceModel, bool value)
    {
        m_isOn = value;
    }
}