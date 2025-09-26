using UnityEngine;

public class SpeedCloak : PassiveItem
{
    protected override void ApplyModifier()
    {
        player.currentMoveSpeed *= 1 + passiveItemData.Multiplier / 100f;
    }
}
