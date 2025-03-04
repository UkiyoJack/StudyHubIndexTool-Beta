/*using UnityEngine;
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
                newResult.GetComponentInChildren<TextMeshProUGUI>().text = space;
            }
        }
    }
}*/

//NEW SEARCHMANAGER

using Firebase.Database;
using Firebase.Extensions;
using UnityEngine;
using TMPro;
using Firebase;

public class SearchManager : MonoBehaviour
{
    public TMP_InputField searchInputField;  // The input field where the user types the search term
    public TMP_Text searchResultText;        // The UI text element to display the result
    private DatabaseReference databaseReference;

    void Start()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsCompleted)
            {
                Debug.Log("Firebase initialized successfully!");
                // Initialize Firebase database reference
                databaseReference = FirebaseDatabase.DefaultInstance.RootReference;
            }
            else
            {
                Debug.LogError("Failed to initialize Firebase: " + task.Exception);
            }
        });

        Firebase.FirebaseApp.LogLevel = Firebase.LogLevel.Debug;
    }

    // This function is triggered when the search button is clicked
    public void OnSearch()
    {
        Debug.Log("OnSearch called");
        if (searchInputField == null || searchResultText == null)
        {
            Debug.LogError("Input field or result text is not assigned!");
            return;
        }




        string searchTerm = searchInputField.text.Trim().ToLower();
        Debug.Log("Search Term: " + searchTerm);

        //retrieve data from 'Locations' node in Firebase and search through the Name fields
        databaseReference.Child("Locations").GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted)
            {
                Debug.LogError("Error retrieving data: " + task.Exception);
                return;
            }
            else if (task.IsCanceled)
            {
                Debug.Log("Retrieving data was canceled.");
                return;
            }

            DataSnapshot snapshot = task.Result;
            bool matchFound = false;  //only used to track if any matching location is found

            // Iterate through each location in the 'Locations' node
            foreach (DataSnapshot locationSnapshot in snapshot.Children)
            {
                // Check if 'Name' exists for each location and matches the search term
                if (locationSnapshot.Child("Name").Value != null)
                {
                    string locationName = locationSnapshot.Child("Name").Value.ToString().ToLower();
                    if (locationName.Contains(searchTerm))
                    {
                        matchFound = true;
                        searchResultText.text = "Match Found: " + locationSnapshot.Child("Name").Value.ToString();

                        // You can also retrieve other fields like coordinates or postcode if needed
                        string coordinates = locationSnapshot.Child("GOOGLE MAPS COORDINATES").Value.ToString();
                        string postcode = locationSnapshot.Child("POSTCODE").Value.ToString();

                        searchResultText.text += "\nCoordinates: " + coordinates + "\nPostcode: " + postcode;

                        // Stop searching after a match is found (optional: remove this if you want to keep searching for more matches)
                        break;
                    }
                }
            }

            if (!matchFound)
            {
                searchResultText.text = "No matches found for: " + searchTerm;
            }
        });
    }
}


/*using Firebase.Database;
using Firebase.Extensions;
using UnityEngine;
using TMPro;
using System.Collections.Generic;
using Firebase;

public class SearchManager : MonoBehaviour
{
    public TMP_InputField searchInputField;  // The input field where the user types the search term
    public TMP_Text searchResultText;        // The UI text element to display the result
    private DatabaseReference databaseReference;

    void Start()
    {
        // Initialize Firebase
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsCompleted)
            {
                Debug.Log("Firebase initialized successfully!");
                // Initialize Firebase database reference
                databaseReference = FirebaseDatabase.DefaultInstance.RootReference;
            }
            else
            {
                Debug.LogError("Failed to initialize Firebase: " + task.Exception);
            }
        });

        Firebase.FirebaseApp.LogLevel = Firebase.LogLevel.Debug;
    }

    // This function is triggered when the search button is clicked
    public void OnSearch()
    {
        Debug.Log("OnSearch called");

        // Validate that required fields are assigned
        if (searchInputField == null || searchResultText == null)
        {
            Debug.LogError("Input field or result text is not assigned!");
            return;
        }

        string searchTerm = searchInputField.text.Trim().ToLower(); // Get and format the search term
        Debug.Log("Search Term: " + searchTerm);

        // Clear the previous result text
        searchResultText.text = "";

        // Retrieve data from 'Locations' node in Firebase and search through 'NAME' fields
        databaseReference.Child("Locations").GetValueAsync().ContinueWithOnMainThread(task => {
            if (task.IsFaulted)
            {
                Debug.LogError("Error retrieving data: " + task.Exception);
                return;
            }
            else if (task.IsCanceled)
            {
                Debug.Log("Retrieving data was canceled.");
                return;
            }

            DataSnapshot snapshot = task.Result;
            bool matchFound = false;  // Track if any matching locations are found

            // Iterate through each location in the 'Locations' node
            foreach (DataSnapshot locationSnapshot in snapshot.Children)
            {
                // Check if 'NAME' exists for each location and matches the search term
                if (locationSnapshot.Child("NAME").Value != null)
                {
                    string locationName = locationSnapshot.Child("NAME").Value.ToString().ToLower();
                    if (locationName.Contains(searchTerm))
                    {
                        matchFound = true;

                        // Append the matching location to the searchResultText
                        searchResultText.text += locationSnapshot.Child("NAME").Value.ToString() + "\n";

                        // You can also retrieve other fields like coordinates and postcode
                        string coordinates = locationSnapshot.Child("GOOGLE MAPS COORDINATES").Value.ToString();
                        string postcode = locationSnapshot.Child("POSTCODE").Value.ToString();

                        // Optionally, display additional info like coordinates and postcode in the result
                        searchResultText.text += "Coordinates: " + coordinates + "\nPostcode: " + postcode + "\n\n";
                    }
                }
            }

            // If no match is found, show a message
            if (!matchFound)
            {
                searchResultText.text = "No matches found for: " + searchTerm;
            }
        });
    }
}
*/