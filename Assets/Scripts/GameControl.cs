using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GameControl : MonoBehaviour
{
    private static GameControl instance;
    public static GameControl Instance { get => instance; set => instance = value; }

    public GameObject[] opponents;
    public GameObject player;

    public Text[] opponentTexts;
    public Text rankText, finishText;

    public GameObject firstPanel, SecondPanel;

    int finishedOnes;

    bool wait;

    int playerRank, playerLastRank;

    private void Awake()
    {
        playerLastRank = -1;
        finishedOnes = 0;
        instance = this;
        wait = true;
    }

    void Update()
    {
        if(wait)
            StartCoroutine(SortByTime());
    }

    IEnumerator SortByTime()
    {
        wait = false;
        yield return new WaitForSeconds(0.1f);
        wait = true;
        SortOpponents();
        SortCharacter();
        WriteRankToText();
    }

    private void SortOpponents()
    {
        for(int i = finishedOnes; i < opponents.Length; i++)
            if (opponents[i].GetComponent<OpponentMovement>().isFinished())
            {
                SwapGameObjects(i, finishedOnes);
                finishedOnes++;
            }

        for (int i = finishedOnes; i < opponents.Length - 1; i++)
        {
            for (int j = i + 1; j < opponents.Length; j++)
                if (opponents[j].GetComponent<OpponentMovement>().CurrentTarget() > opponents[i].GetComponent<OpponentMovement>().CurrentTarget())
                    SwapGameObjects(i, j);
        }
        for (int i = finishedOnes; i < opponents.Length; i++)
        {
            for (int j = i + 1; j < opponents.Length; j++)
            {
                if (opponents[j].GetComponent<OpponentMovement>().CurrentTarget() != opponents[i].GetComponent<OpponentMovement>().CurrentTarget())
                {
                    SortInterval(i, j - 1);
                    i = j - 1;
                }
            }
        }
    }

    void WriteRankToText()
    {
        rankText.text = "" + (playerRank + 1);
        for (int i = 0; i < opponentTexts.Length; i++)
        {
            if (i == playerRank)
                opponentTexts[i].text = "You";
            else if (i > playerRank)
                opponentTexts[i].text = opponents[i - 1].GetComponent<OpponentMovement>().GetName();
            else
                opponentTexts[i].text = opponents[i].GetComponent<OpponentMovement>().GetName();
        }
    }

    internal void ToWall()
    {
        firstPanel.SetActive(false);
        SecondPanel.SetActive(true);
    }

    private void SortInterval(int left, int right)
    {
        for (int i = left; i < right; i++)
        {
            for (int j = i + 1; j <= right; j++)
                if (opponents[j].GetComponent<OpponentMovement>().DistanceToTarget() < opponents[i].GetComponent<OpponentMovement>().DistanceToTarget())
                    SwapGameObjects(i, j);
        }
    }

    private void SwapGameObjects(int i, int j)
    {
        GameObject temp = opponents[i];
        opponents[i] = opponents[j];
        opponents[j] = temp;
    }

    private void SortCharacter()
    {
        if (playerLastRank < 0)
        {
            bool cont = true;
            for (int i = 0; i < opponents.Length; i++)
                if (opponents[i].transform.InverseTransformDirection(opponents[i].transform.position - player.transform.position).z < 0)
                {
                    playerRank = i;
                    cont = false;
                    break;
                }
            if (cont)
                playerRank = 10;
        }
    }

    internal void PlayerEnded()
    {
        playerLastRank = playerRank;
        if (playerLastRank == 0)
        {
            finishText.text = "You Won";
            finishText.color = Color.green;
        }
        else if (playerLastRank < 2)
        {
            finishText.text = "2nd";
            finishText.color = Color.yellow;
        }
        else if (playerLastRank < 3)
        {
            finishText.text = "3rd";
            finishText.color = Color.red;
        }
        else
            finishText.text = (playerLastRank + 1) + "th";
    }
}
