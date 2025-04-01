using UnityEngine;

[System.Serializable]
public class Campaign
{
    // Player's current deck
    public DeckObj deck;

    // History of all series played so far (including game results)
    public SeriesObj[] series;
    
    // Number of upgrades available for purchase from the store
    public int upgradesAvailable = 0;

    /// <summary>
    /// Build campaign data from a save file
    /// </summary>
    /// <param name="fromSave"></param>
    public Campaign(CampaignSaveData fromSave)
    {
        deck                = new DeckObj(fromSave.deckSave);
        series              = loadSeriesFromSeriesSave(fromSave.seriesSaves);
        upgradesAvailable   = fromSave.upgradesAvailable;
    }

    /// <summary>
    /// Copies the deck for the overall campaign.
    /// Game Instances will copy this so no changes can happen in game.
    /// </summary>
    /// <returns></returns>
    public DeckObj copyDeck()
    {
        return deck.copyObject();
    }

    /// <summary>
    /// Returns the current deck
    /// </summary>
    /// <returns></returns>
    public DeckObj getDeck()
    {
        return deck;
    }

    
    /// <summary>
    /// Copies ALL series to a new array
    /// </summary>
    /// <returns></returns>
    public SeriesObj[] copySeries()
    {
        SeriesObj[] output = new SeriesObj[series.Length];

        for (int i = 0; i < series.Length; i++)
        {
            output[i] = series[i];
        }

        return output;
    }

    /// <summary>
    /// Returns the current campaign's series
    /// </summary>
    /// <returns></returns>
    public SeriesObj[] getSeries()
    {
        return series;
    }

    private SeriesObj[] loadSeriesFromSeriesSave(SeriesSaveState[] _series)
    {
        SeriesObj[] output = new SeriesObj[_series.Length];

        for (int i = 0; i < _series.Length; i++)
        {
            output[i] = _series[i].copyToSeries();
        }

        return output;
    }

    /// <summary>
    /// Copy the current series to a save state format
    /// </summary>
    /// <returns></returns>
    public SeriesSaveState[] copySeriesSaveState()
    {
        SeriesSaveState[] output = new SeriesSaveState[series.Length];

        for (int i = 0; i < series.Length; i++)
        {
            output[i] = new SeriesSaveState(series[i]);
        }

        return output;
    }
}

// Object to be serialized in JSON for save data.
[System.Serializable]
public class CampaignSaveData
{
    public DeckSaveState deckSave;         // a DeckObj serialized as a string
    public SeriesSaveState[] seriesSaves;
    public int upgradesAvailable;

    public CampaignSaveData(Campaign fromCampaign)
    {
        deckSave          = fromCampaign.copyDeck().getDeckSaveState();
        seriesSaves       = fromCampaign.copySeriesSaveState();
        upgradesAvailable = fromCampaign.upgradesAvailable;
    }
}
