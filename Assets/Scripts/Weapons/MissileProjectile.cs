﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileProjectile : WeaponProjectileBase
{
    public float missileSpeed;
    public float missileRotationSpeed;
    public float missileDectectionRange;
    public float missileRadarRefresh;

    public Collider[] MissileTargets;

    [SerializeField]
    private Transform LockedTarget;
    // Start is called before the first frame update

    protected override void Start()
    {
        base.Start();
        MissileTargets = new Collider[0];
        StartCoroutine(DetectTarget());
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();

        MissileBrain();
    }

    private void MissileBrain()
    {

        if (rb == null)
        {
            return;
        }
        else
        {
            //Missile keeps going forward unless it finds a target
            rb.velocity = this.transform.forward * (missileSpeed + mf_carVelocity);

            if (LockedTarget != null)
            {
                //Determine possibly random target or target that is directly in front of the current car
                //Don't do this, just get the target in front
                var targetRotation = Quaternion.LookRotation(LockedTarget.position - transform.localPosition);

                rb.MoveRotation(Quaternion.RotateTowards(transform.localRotation, targetRotation, missileRotationSpeed * Time.deltaTime));
            }
        }
    }

    private IEnumerator DetectTarget()
    {
        while (true)
        {
            MissileDectection();
            yield return new WaitForSeconds(missileRadarRefresh);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawSphere(transform.position, missileDectectionRange);
    }

    private void MissileDectection()
    {
        MissileTargets = Physics.OverlapSphere(transform.position, missileDectectionRange);

        if (MissileTargets.Length == 0)
        {
            return;
        }
        else
        {
            //Detection logic for homing missile
            for (int i = 0; i < MissileTargets.Length; i++)
            {
                if (MissileTargets[i].transform.root.GetComponent<NewCarController>())
                {
                    if (MissileTargets[i].transform.root.GetComponent<NewCarController>().ownerID
                        != originOwnerID)
                    {
                        LockedTarget = MissileTargets[i].transform;
                        return;
                    }
                    else
                    {
                        continue;
                    }
                }
                //Logic to prioritize targets
                if (MissileTargets[i].transform.root.tag == "target")
                {
                    LockedTarget = MissileTargets[i].transform;
                    return;
                }
            }
        }
    }

    protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
    }
}
