using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    private float _remaining = 300f;

    // Update is called once per frame
    void Update()
    {
        if(_remaining >= 0)
        {
            _remaining -= Time.deltaTime;
            GetComponent<TextMeshProUGUI>().text = Mathf.Floor(_remaining / 60).ToString("0") + ":" + Mathf.Floor(_remaining % 60).ToString("00"); 
        }

    }
}
