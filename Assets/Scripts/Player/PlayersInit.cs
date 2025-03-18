using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class PlayersInit : MonoBehaviour
{
    [SerializeField]
    public GameObject playerPrefab;
    [SerializeField]
    public Transform playerArea;

    private List<GameObject> players = new List<GameObject>();
    private GameObject currentPlayer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        int i = 0;

        while (i < 9)
        {
            // Create a new player prefab for that lineup spot
            GameObject playerObject = Instantiate(playerPrefab, playerArea);

            // Update the prefab text
            playerObject.transform.Find("Player Name").GetComponent<TMP_Text>().text = $"Player #{i + 1}";

            // Add player to list of players
            players.Add(playerObject);

            // Increment
            i++;
        }

        SetNextPlayer();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetNextPlayer()
    {
        Player playerScript;

        // If there is no current player, we're setting the game up, set to lineup spot 1
        if (currentPlayer == null)
        {
            currentPlayer = players.First();
        }
        // Otherwise find the next one
        else
        {
            // Start by removing the "current" status from the current active player
            // Get the script attached to this game object, then unset it.
            playerScript = currentPlayer.GetComponent<Player>();
            if (playerScript != null)
            {
                playerScript.UnsetCurrent();
            }

            // Then find the next one
            currentPlayer = players.SkipWhile(x => x != currentPlayer).Skip(1).FirstOrDefault();

            // End of list, go to first
            if (currentPlayer == null)
            {
                currentPlayer = players.First();
            }
        }

        // Get the current player script and set it to active;
        playerScript = currentPlayer.GetComponent<Player>();
        if (playerScript != null)
        {
            playerScript.SetCurrent();
        }
    }
}
