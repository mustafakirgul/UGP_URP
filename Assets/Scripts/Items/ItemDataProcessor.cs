﻿using System.Collections.Generic;
using UnityEngine;

public enum ItemType { None, Weapon, Armour, Engine, Other }
public class ItemDataProcessor : MonoBehaviour
{
    [SerializeField]
    private Player m_player;
    [SerializeField]
    private List<ItemBase> m_listOfItems;
    [SerializeField]
    private float mf_ItemMeleeAttack;
    [SerializeField]
    private float mf_ItemHealth;
    [SerializeField]
    private float mf_ItemSpeed;
    [SerializeField]
    private float mf_ItemOther;
    [SerializeField]
    private float mf_maxItemCount;

    private void Awake()
    {
        m_player = GetComponent<Player>();
        processAllItemDate();
    }

    void processAllItemDate()
    {
        for (int i = 0; i < m_listOfItems.Count; i++)
        {
            switch (m_listOfItems[i]._ItemType)
            {
                case ItemType.None:
                    break;
                case ItemType.Weapon:
                    mf_ItemMeleeAttack += m_listOfItems[i].m_Attack;
                    break;
                case ItemType.Armour:
                    mf_ItemHealth += m_listOfItems[i].m_Health;
                    break;
                case ItemType.Engine:
                    mf_ItemSpeed += m_listOfItems[i].m_Speed;
                    break;
                case ItemType.Other:
                    mf_ItemOther += m_listOfItems[i].m_Speed;
                    break;
                default:
                    break;
            }
        }
    }

    //Use this for temporary items that will boost the attack or defense
    //of builds in game
    public void ProcessSingleItemData(ItemBase ItemPickUp)
    {
        switch (ItemPickUp._ItemType)
        {
            case ItemType.None:
                break;
            case ItemType.Weapon:
                mf_ItemMeleeAttack += ItemPickUp.m_Attack;
                break;
            case ItemType.Armour:
                mf_ItemHealth += ItemPickUp.m_Health;
                break;
            case ItemType.Engine:
                mf_ItemSpeed += ItemPickUp.m_Speed;
                break;
            case ItemType.Other:
                mf_ItemOther += ItemPickUp.m_Speed;
                break;
            default:
                break;
        }
    }

    void UpdateData()
    {
        //TO DO: Update data to Car Controller and other areas where pretinent
    }
}
