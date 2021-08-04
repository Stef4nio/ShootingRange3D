using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using TMPro;
using UnityEngine;

public class LeaderboardView : MonoBehaviour
{
    [SerializeField] private GameObject _leaderboardTable = null;
    
    // Start is called before the first frame update
    void Start()
    {
        ClearTable();
        Show(JsonConvert.DeserializeObject<List<PlayerResultsContainer>>(
            PlayerPrefs.GetString(Config.PLAYERS_RESULTS_PLAYERPREFS_KEY)));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
    
    public void Show(List<PlayerResultsContainer> results)
    {
        gameObject.SetActive(true);
        int currResultPosition = 0;
        bool skipHeader = true;
        foreach (Transform row in _leaderboardTable.transform.GetChild(0))
        {
            if (skipHeader)
            {
                skipHeader = false;
                continue;
            }

            if (currResultPosition < results.Count)
            {
                row.GetChild(1).GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text =
                    results[currResultPosition].CurrentPlayerName;
                row.GetChild(1).GetChild(1).gameObject.GetComponent<TextMeshProUGUI>().text =
                    results[currResultPosition].GetShortPlayerTime();
                currResultPosition++;
            }
            else
            {
                foreach (Transform cell in row.GetChild(1))
                {
                    cell.gameObject.GetComponent<TextMeshProUGUI>().text = "";
                }
            }
        }
    }

    private void ClearTable()
    {
        bool skipHeader = true;
        foreach (Transform row in _leaderboardTable.transform.GetChild(0))
        {
            if (skipHeader)
            {
                skipHeader = false;
                continue;
            }
            foreach (Transform cell in row.GetChild(1))
            {
                cell.gameObject.GetComponent<TextMeshProUGUI>().text = "";
            }
        }
    }
}
