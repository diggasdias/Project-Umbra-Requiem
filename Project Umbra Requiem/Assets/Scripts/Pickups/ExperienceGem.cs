using UnityEngine;

public class ExperienceGem : MonoBehaviour, ICollectable
{
    public int experienceGranted;

    public void Collect()
    {
        PlayerStats player = FindAnyObjectByType<PlayerStats>();
        player.IncreaseExperience(experienceGranted);
        Destroy(gameObject);
    }
}
