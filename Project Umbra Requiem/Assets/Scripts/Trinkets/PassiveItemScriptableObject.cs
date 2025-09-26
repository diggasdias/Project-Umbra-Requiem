using UnityEngine;
[CreateAssetMenu(fileName = "TrinketScriptableObject", menuName = "ScriptableObjects/Trinket")]
public class PassiveItemScriptableObject : ScriptableObject
{
    [SerializeField]
    float multiplier;
    public float Multiplier { get => multiplier; private set => multiplier = value; }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Start()
    {

    }

}
