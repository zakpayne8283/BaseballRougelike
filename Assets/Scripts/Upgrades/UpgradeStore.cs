using UnityEngine;

public class UpgradeStore : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void openUpgradeStore()
    {
        // Set the store to visible
        gameObject.SetActive(true);
    }

    public void closeUpgradeStore()
    {
        gameObject.SetActive(false);
    }
}
