using UnityEngine;
using TMPro;
public class timer : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI timerText;
    [SerializeField] float remainigTime;

    void Update()
    {
        remainigTime += Time.deltaTime;
        int minutes = Mathf.FloorToInt(remainigTime / 60);
        int seconds = Mathf.FloorToInt(remainigTime % 60);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}
