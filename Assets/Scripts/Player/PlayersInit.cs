using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class PlayersInit : MonoBehaviour
{
    // Player prefab to create when making the lineups
    [SerializeField] public GameObject playerPrefab;
    // The Players panel on screen
    [SerializeField] public Transform playerArea;

    // Default player types in lineup
    private readonly PLAYER_TYPE[] defaultLineup =
    {
        PLAYER_TYPE.SPEED,
        PLAYER_TYPE.NONE,
        PLAYER_TYPE.CONTACT,
        PLAYER_TYPE.POWER,
        PLAYER_TYPE.POWER,
        PLAYER_TYPE.NONE,
        PLAYER_TYPE.NONE,
        PLAYER_TYPE.NONE,
        PLAYER_TYPE.SPEED
    };

    private List<GameObject> awayPlayers = new List<GameObject>();
    public GameObject currentPlayer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        int i = 0;

        // Generate the away team lineup
        while (i < defaultLineup.Length)
        {
            // Create a new player prefab for that lineup spot
            GameObject playerObject = Instantiate(playerPrefab, playerArea);

            // Update the prefab text
            playerObject.transform.Find("Player Name").GetComponent<TMP_Text>().text = $"Player #{i + 1}";

            // Set the player type from the default values
            playerObject.GetComponent<Player>().PlayerType = defaultLineup[i]; 

            // Add player to list of players
            awayPlayers.Add(playerObject);

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
            currentPlayer = awayPlayers.First();
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
            currentPlayer = awayPlayers.SkipWhile(x => x != currentPlayer).Skip(1).FirstOrDefault();

            // End of list, go to first
            if (currentPlayer == null)
            {
                currentPlayer = awayPlayers.First();
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
