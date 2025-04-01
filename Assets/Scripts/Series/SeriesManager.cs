using UnityEngine;

public class SeriesManager : MonoBehaviour
{
    [SerializeField] public Transform seriesInfo;      // Where to add game objects

    private SeriesObj currentSeries;                    // The series the campaign is currently on

    [SerializeField] public GameObject seriesPrefab;    // The game tile prefab added to the seriesInfo area

    private SeriesObj[] allSeries;                      // All series in the campaign so far

    void Awake()
    {
        // Load any data we need to load first.
        allSeries = CampaignManager.Instance.campaignData.getSeries();
        
        // Set the current series to latest one
        currentSeries = allSeries[allSeries.Length - 1];
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        populateSeries();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void populateSeries()
    {
        // For each series 
        for (int i = 0; i < allSeries.Length; i++)
        {
            // Create a new series prefab
            GameObject createdSeries = Instantiate(seriesPrefab, seriesInfo);

            // Initialize the new series prefab with the series object we have
            createdSeries.GetComponent<SeriesPrefab>().Initialize(allSeries[i]);

            // Hide the created series if it's not the current series
            if (allSeries[i] != currentSeries)
            {
                createdSeries.SetActive(false);
            }
        }
    }
}
