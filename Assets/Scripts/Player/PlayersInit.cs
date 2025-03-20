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
    private List<GameObject> homePlayers = new List<GameObject>();
    public GameObject currentPlayer;

    private bool topInning = true;

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
            playerObject.transform.Find("Player Name").GetComponent<TMP_Text>().text = $"Player #{i + 1}{playerTypeInName(defaultLineup[i])}";

            // Set the player type from the default values
            playerObject.GetComponent<Player>().PlayerType = defaultLineup[i]; 

            // Add player to list of players
            awayPlayers.Add(playerObject);

            // Increment
            i++;
        }

        // Create the hidden home team lineup
        i = 0;
        while (i < defaultLineup.Length)
        {
            // Create a new player prefab for that lineup spot
            GameObject playerObject = Instantiate(playerPrefab, playerArea);

            // Update ]the prefab text
            playerObject.transform.Find("Player Name").GetComponent<TMP_Text>().text = $"Player #{i + 10}{playerTypeInName(defaultLineup[i])}"; // +10 -> +1 for display, +9 because second lineup

            // Set the player type from the default values
            playerObject.GetComponent<Player>().PlayerType = defaultLineup[i];

            playerObject.SetActive(false);

            // Add player to list of players
            homePlayers.Add(playerObject);

            // Increment
            i++;
        }

        SetNextPlayer();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetNextPlayer(bool changeInning=false)
    {
        Player playerScript;

        // If there is no current player, we're setting the game up, set to lineup spot 1
        if (currentPlayer == null)
        {
            currentPlayer = awayPlayers.First();

            // Get the current player script and set it to active;
            playerScript = currentPlayer.GetComponent<Player>();
            if (playerScript != null)
            {
                playerScript.SetCurrent();
            }
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

            // Set the next batter
            currentPlayer = findNextActivePlayer();

            // Get the current player script and set it to active;
            playerScript = currentPlayer.GetComponent<Player>();
            if (playerScript != null)
            {
                playerScript.SetCurrent();
            }

            // Change the top/bottom of the inning
            if (changeInning)
            {
                topInning = !topInning;

                // set currentPlayer value to the next player for the other team
                if (topInning)
                {
                    currentPlayer = awayPlayers.Where(x => x.GetComponent<Player>().currentPlayer).FirstOrDefault();

                    if (currentPlayer == null)
                    {
                        currentPlayer = awayPlayers.First();
                    }
                }
                else
                {
                    currentPlayer = homePlayers.Where(x => x.GetComponent<Player>().currentPlayer).FirstOrDefault();

                    if (currentPlayer == null)
                    {
                        currentPlayer = homePlayers.First();

                        playerScript = currentPlayer.GetComponent<Player>();
                        if (playerScript != null)
                        {
                            playerScript.SetCurrent();
                        }
                    }
                }

                // Hide all elements of current team and unhide other team
                if (topInning)
                {
                    foreach (GameObject _player in homePlayers)
                    {
                        _player.SetActive(false);
                    }

                    foreach(GameObject _player in awayPlayers)
                    {
                        _player.SetActive(true);
                    }
                }
                else
                {
                    foreach (GameObject _player in awayPlayers)
                    {
                        _player.SetActive(false);
                    }

                    foreach(GameObject _player in homePlayers)
                    {
                        _player.SetActive(true);
                    }
                }
            }
        }
    }

    private string playerTypeInName(PLAYER_TYPE lineupSlotType)
    {
        string output = "";

        if (lineupSlotType != PLAYER_TYPE.NONE)
        {
            switch (lineupSlotType)
            {
                case PLAYER_TYPE.CONTACT:
                    output += "(C)";
                    break;
                case PLAYER_TYPE.POWER:
                    output += "(P)";
                    break;
                case PLAYER_TYPE.SPEED:
                    output += "(S)";
                    break;
                default:
                    break;
            }
        }

        return output;
    }

    public GameObject findNextActivePlayer()
    {
        List<GameObject> lineupToUse = (topInning ? awayPlayers : homePlayers);

        // Then find the next one
        GameObject nextPlayer = lineupToUse.SkipWhile(x => x != currentPlayer).Skip(1).FirstOrDefault();

        // End of list, go to first
        if (nextPlayer == null)
        {
            nextPlayer = lineupToUse.First();
        }

        return nextPlayer;
    }
}
