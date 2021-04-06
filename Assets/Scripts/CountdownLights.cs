﻿using UnityEngine;

public class CountdownLights : MonoBehaviour
{
    public GameObject[] lights;

    public void TurnOntheLight(int index)
    {
        lights[index].SetActive(true);
    }

    public void Reset()
    {
        for (int i = 0; i < lights.Length; i++)
        {
            lights[i].SetActive(false);
        }
    }
}