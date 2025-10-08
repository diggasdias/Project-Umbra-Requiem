using UnityEngine;

public class ExperienceGem : Pickup, ICollectable
{
    public int experienceGranted;
    PlayerStats player;

    public void Collect()
    {
        if (player == null)
            player = FindAnyObjectByType<PlayerStats>();

        if (player != null)
            player.IncreaseExperience(experienceGranted);
        else
            Debug.LogError("PlayerStats não encontrado ao coletar ExperienceGem!");
    }
}
