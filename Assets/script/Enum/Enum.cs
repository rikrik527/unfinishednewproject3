namespace Yushan.Enums
{
    public class Enum { }

    public enum ItemType
    {
        devilItem,
        normalItemPicture,
        recoveryItem,
        weapon,
        craftItem,
        Reapable_Scenary,
        none,
        count,


    }
    public enum Stats
    {
        isRunningRight,
        isRunningLeft,
        isWalkingRight,
        isWalkingLeft,
        isSprintingRight,
        isSprintingLeft,
        isIdle
    }
    public enum Yushan_Type
    {
        fatType,
        darkenType,
        demonType
    }
    public enum Demon_State
    {
        demonPowerCharge,
        demonPowerPunch,
        demonPunch,
        demonSecondPunch,
        demonTransform,
        demonPowerCharge2,
        demonPowerPunch2
    }
    public enum ToolEffect
    {
        none,
        watering
    }
    public enum PlayerState
    {
        running,
        walk,
        idle,
        attack,
        interact
    }
    public enum Direction
    {
        left,
        right,
        none

    }
    public enum InventoryLocation
    {
        player,
        chest,
        count
    }

}

