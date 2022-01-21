using System.Collections.Generic;
using System;
using Yushan.Enums;


public delegate void DarkComboDelegate(bool isDarkSpinKick, bool isDarkWhirlWindKick, bool isDarkSweepKick, bool isDarkDoubleSweepKick, bool isDarkSpinHeadKick);

public delegate void DemonDelegate(bool isDemon, bool isDemonIdle, bool isDemonPunch, bool isDemonPowerCharge, bool isDemonPowerPunch, bool isDemonSecondPunch, bool isDemonPowerCharge2, bool isDemonPowerPunch2);
public static class EventHandler
{

    public static event Action<InventoryLocation, List<InventoryItem>> InventoryUpdatedEvent;

    public static void CallInventoryUpdatedEvent(InventoryLocation inventoryLocation, List<InventoryItem> inventoryList)
    {
        if (InventoryUpdatedEvent != null)
        {
            InventoryUpdatedEvent(inventoryLocation, inventoryList);
        }
    }
    //movement event


    public static event DemonDelegate DemonEvent;
    //demon actions event call for publishers
    public static void CallDemonEvent(bool isDemon, bool isDemonIdle, bool isDemonPunch, bool isDemonPowerCharge, bool isDemonPowerPunch, bool isDemonSecondPunch, bool isDemonPowerCharge2, bool isDemonPowerPunch2)
    {
        if (DemonEvent != null)
        {
            DemonEvent(isDemon, isDemonIdle, isDemonPunch, isDemonPowerCharge, isDemonPowerPunch, isDemonSecondPunch, isDemonPowerCharge2, isDemonPowerPunch2);
        }
    }

    //dark combo system call for publishers

    public static event DarkComboDelegate DarkComboEvent;
    public static void CallDarkComboEvent(bool isDarkSpinKick, bool isDarkWhirlWindKick, bool isDarkSweepKick, bool isDarkDoubleSweepKick, bool isDarkSpinHeadKick)
    {
        if (DarkComboEvent != null)
        {
            DarkComboEvent(isDarkSpinKick, isDarkWhirlWindKick, isDarkSweepKick, isDarkDoubleSweepKick, isDarkSpinHeadKick);
        }

    }
}
