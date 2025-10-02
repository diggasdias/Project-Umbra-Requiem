using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class DeathRingTrinket : PassiveItem
{
    protected override void ApplyModifier()
    {
        player.CurrentMight *= 1 + passiveItemData.Multiplier;
    }
}
