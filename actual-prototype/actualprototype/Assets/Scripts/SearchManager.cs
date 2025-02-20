using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;
using System;

public class SearchPage : MonoBehaviour
{
    
    public TMP_InputField searchInputField; //input field ref

   
    public Transform resultsContent; //content area ref

    
    public GameObject resultPrefab; //search results prefab

    //placeholder for database
    private List<string> studySpaces = new List<string> { "Library", "Cafe", "Study Room", "Lab" };

    //SEARCH FUNCTION:
    public void OnSearch()
    {
        Debug.Log("Called Search Function!");
        
        string query = searchInputField.text.ToLower(); //convert to lowercase

        //clear previous search queries
        foreach (Transform child in resultsContent)
        {
            Destroy(child.gameObject);
        }

        //foreach loop to loop through data and display results
        foreach (string space in studySpaces)
        {
            if (space.ToLower().Contains(query))
            {
                //create new result item
                GameObject newResult = Instantiate(resultPrefab, resultsContent);

                //set text of result
                newResult.GetComponentInChildren<Text>().text = space;
            }
        }
    }
}