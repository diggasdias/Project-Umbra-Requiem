using UnityEngine;

public class ExperienceGem : Pickup, ICollectable
{
    public int experienceGranted;
    PlayerStats player;

    void Start()
    {
        player = FindAnyObjectByType<PlayerStats>();
    }

    public void Collect()
    {
        player.IncreaseExperience(experienceGranted);
    }
}
