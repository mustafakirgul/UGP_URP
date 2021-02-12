﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class TurretAutoAim : MonoBehaviour
{
    [SerializeField]
    private GameObject enemy, lastEnemy;
    public GameObject turretFOV;
    public float turretRotationSpd;

    public float maxAngle;
    public float turretDetectionRange;
    public float radarSweepTimer;

    private int targetIndex = 0;

    private Quaternion targetRotation;
    private Quaternion LookAtRotation;
    private Vector3 lastCrossHairPos;

    [SerializeField]
    private float playerColliderID;

    private NewCarController carController;

    [SerializeField]
    private Collider[] turretTargets;
    [SerializeField]
    private LayerMask turretDetectionLayer;

    public List<Collider> targetList = new List<Collider>();

    [SerializeField]
    private List<Collider> targetsToIgnore = new List<Collider>();

    public bool isManualTargeting = false;

    private UIManager m_uiManager;

    public Image CrossHairUI;
    public Camera currentCam;

    [SerializeField]
    RectTransform parentCanvas;

    float lerpTime = 1f;
    public float currentLerpTime;

    // Update is called once per frame
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(transform.position, turretDetectionRange);
    }
    private void Start()
    {
        targetList.Clear();
        carController = GetComponentInParent<NewCarController>();

        if (!carController.isNetworkInstance)
        {
            lastCrossHairPos = Vector3.zero;
            m_uiManager = FindObjectOfType<UIManager>();
            CrossHairUI = m_uiManager.CrossHairUI;
            currentCam = m_uiManager.UIcamera;
            parentCanvas = m_uiManager.ScreenCanvas.GetComponent<RectTransform>();
            StartCoroutine(DelayRadarAtStart());
        }
        playerColliderID = carController.ownerID;
    }

    private IEnumerator DelayRadarAtStart()
    {
        yield return new WaitForSeconds(2f);
        StartCoroutine(turretRadarSweep());
    }
    void Update()
    {
        if (!carController.isNetworkInstance)
        {
            RotateTurret();
            MoveCrossHair();
        }
    }

    void MoveCrossHair()
    {
        if (enemy != null)
        {
            Vector3 Pos = Camera.main.WorldToViewportPoint(enemy.transform.position);
            Vector3 PosCalculated = new Vector3(parentCanvas.rect.width * (Pos.x - 0.5f),
                parentCanvas.rect.height * (Pos.y - 0.5f), 0);

            if (lastEnemy != null)
            {
                Vector3 LastPos = Camera.main.WorldToViewportPoint(lastEnemy.transform.position);
                Vector3 LastPosCalculated = new Vector3(parentCanvas.rect.width * (LastPos.x - 0.5f),
                    parentCanvas.rect.height * (LastPos.y - 0.5f), 0);


                //CrossHairUI.rectTransform.localPosition = new Vector3(parentCanvas.rect.width * (Pos.x - 0.5f),
                //    parentCanvas.rect.height * (Pos.y - 0.5f), 0);
                currentLerpTime += Time.deltaTime * 3f;

                if (currentLerpTime > lerpTime)
                {
                    currentLerpTime = lerpTime;
                }

                //lerp!
                float perc = currentLerpTime / lerpTime;
                //transform.position = Vector3.Lerp(startPos, endPos, perc);
                //CrossHairUI.rectTransform.localPosition =
                //    new Vector3(Mathf.Lerp(LastPosCalculated.x, PosCalculated.x, perc),
                //                Mathf.Lerp(LastPosCalculated.y, PosCalculated.y, perc), 0);

                CrossHairUI.rectTransform.localPosition =
                    Vector3.Lerp(LastPosCalculated, PosCalculated, perc);
            }
        }
    }

    public IEnumerator ResetManualTargetingCR()
    {
        isManualTargeting = true;
        yield return new WaitForSeconds(3f);
        isManualTargeting = false;
    }

    private IEnumerator turretRadarSweep()
    {
        while (true)
        {
            ObtainTargets();
            RemoveTargets();
            if (!isManualTargeting)
                AutoSelectTarget();
            yield return new WaitForSeconds(radarSweepTimer);
        }
    }

    void ObtainTargets()
    {
        turretTargets = Physics.OverlapSphere(transform.position,
            turretDetectionRange, turretDetectionLayer);

        for (int i = 0; i < turretTargets.Length; i++)
        {
            //An id check here would mean removing or ignoring colliders of the same id is not necessary
            if (!targetList.Contains(turretTargets[i]))
            {
                targetList.Add(turretTargets[i]);

                if (turretTargets[i].GetComponent<NewCarController>() != null)
                {
                    if (turretTargets[i].GetComponent<NewCarController>().ownerID == playerColliderID)
                    { targetList.Remove(turretTargets[i]); }
                }
            }
        }
    }

    void RemoveTargets()
    {
        if (turretTargets.Length != targetList.Count)
        {
            for (int i = 0; i < targetList.Count; i++)
            {
                if (!turretTargets.Contains(targetList[i]))
                {
                    targetList.Remove(targetList[i]);
                }
            }
        }
    }

    public void CycleSelectTarget()
    {
        if (targetList.Count != 0)
        {
            currentLerpTime = 0;

            if(targetIndex < targetList.Count)
            {
                lastEnemy = targetList[targetIndex].gameObject;
            }
            targetIndex++;
            targetIndex %= targetList.Count;
            Debug.Log("Selecting Target " + targetIndex);
            Collider toREmove = targetList[targetIndex];
            enemy = targetList[targetIndex].gameObject;

            //This ensures autotargeting stays on target as 
            //It selects the first elements in the target list
            //targetList.Remove(toREmove);
            //targetList.Insert(0, toREmove);
        }
        else
        {
            Debug.Log("No Targets in Range");
            return;
        }
    }
    void AutoSelectTarget()
    {
        if (targetList.Count == 0)
        {
            Debug.Log("No Targets in Range");
            return;
        }
        else
        {
            enemy = targetList[0].gameObject;
        }
    }

    void RotateTurret()
    {
        if (EnemyInFieldOfView(turretFOV))
        {
            Vector3 direction = enemy.transform.position - transform.position;
            targetRotation = Quaternion.LookRotation(direction);
            LookAtRotation = Quaternion.RotateTowards(transform.rotation, targetRotation,
                            Time.deltaTime * turretRotationSpd);
            transform.rotation = LookAtRotation;
        }
        else
        {
            targetRotation = Quaternion.Euler(0, 0, 0);
            transform.localRotation = Quaternion.RotateTowards(transform.localRotation,
                    targetRotation, Time.deltaTime * turretRotationSpd);
        }
    }

    bool EnemyInFieldOfView(GameObject turret)
    {
        if (enemy != null)
        {

            float range = Vector3.Distance(transform.position, enemy.transform.position);
            Vector3 targetDir = enemy.transform.position - transform.position;
            float angle = Vector3.Angle(targetDir, turret.transform.forward);

            if (angle < maxAngle && range < turretDetectionRange)
            {
                if (!CrossHairUI.gameObject.activeInHierarchy)
                {
                    CrossHairUI.gameObject.SetActive(true);
                }
                return true;
            }
            else
            {

                CrossHairUI.gameObject.SetActive(false);
                return false;
            }
        }
        else
        {
            CrossHairUI.gameObject.SetActive(false);
            return false;
        }
    }
}
