using UnityEngine;
using TMPro;
public class timer : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI timerText;
    [SerializeField] float remainigTime;

    void Update()
    {
        if (remainigTime > 0)
        {
            remainigTime -= Time.deltaTime;
        }
        else if (remainigTime < 0)
        {
            remainigTime = 0;
        }

        int minutes = Mathf.FloorToInt(remainigTime / 60);
        int seconds = Mathf.FloorToInt(remainigTime % 60);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}
