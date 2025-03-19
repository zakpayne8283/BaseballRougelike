using UnityEngine;
using UnityEngine.UI;

public class FieldOfPlayManager : MonoBehaviour
{
    [SerializeField] private Transform fieldOfPlay;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateFromGameState(bool runnerOnFirst, bool runnerOnSecond, bool runnerOnThird)
    {
        // Update runner on first
        if (runnerOnFirst)
        {
            fieldOfPlay.Find("First").GetComponent<Image>().color = Color.red;
        }
        else
        {
            fieldOfPlay.Find("First").GetComponent<Image>().color = Color.white;
        }

        // Update runner on second
        if (runnerOnSecond)
        {
            fieldOfPlay.Find("Second").GetComponent<Image>().color = Color.red;
        }
        else
        {
            fieldOfPlay.Find("Second").GetComponent<Image>().color = Color.white;
        }

        // Update runner on third
        if (runnerOnThird)
        {
            fieldOfPlay.Find("Third").GetComponent<Image>().color = Color.red;
        }
        else
        {
            fieldOfPlay.Find("Third").GetComponent<Image>().color = Color.white;
        }
    }
}
