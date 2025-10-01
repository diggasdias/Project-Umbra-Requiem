using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class DeathRingTrinket : PassiveItem
{
    protected override void ApplyModifier()
    {
        player.currentMight *= 1 + passiveItemData.Multiplier;
    }
}
