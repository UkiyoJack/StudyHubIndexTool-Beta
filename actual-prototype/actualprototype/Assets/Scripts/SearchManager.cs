using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;
using System;
using static UnityEditor.FilePathAttribute;
using Unity.VisualScripting;

public class SearchPage : MonoBehaviour
{

    public TMP_InputField searchInputField; //input field ref

    public Transform resultsContent; //content area ref

    public GameObject resultPrefab; //search results prefab

    public GameObject detailsPage; // Reference to the details page

    public TextMeshProUGUI nameText; // Text field for the name on the details page

    public TextMeshProUGUI postCodeText; // Text field for the post code

    public TextMeshProUGUI descText; // Text field for the description

    /*    public TextMeshProUGUI detailsCoordinatesText;*/



    //Active Filters:
    private string selectedNoiseLevel = "";

    private bool filterChargingPorts = false;

    private bool filterWiFi = false;

    private bool filterIndoors = false;

    private bool filterOutdoors = false;



    //placeholder for location data
    private List<Location> studySpaces = new List<Location>();

    private void Start()
    {
        // Initialize with sample data
        studySpaces.Add(new Location("Library", "AB12 3CD", new Vector2(51.5074f, -0.1278f), "Quiet library with Wi-Fi", "Low", true, true, true));
        studySpaces.Add(new Location("Cafe", "EF45 6GH", new Vector2(51.5094f, -0.1280f), "Cozy cafe with power outlets", "Medium", true, true, false));
        studySpaces.Add(new Location("Study Room", "GH78 9IJ", new Vector2(51.5054f, -0.1275f), "Private study room", "Low", false, true, true));
    }

    public void SetNoiseLevel(string level)
    {
        selectedNoiseLevel = level;
        OnSearch(); // Update results
    }

    public void ToggleChargingPorts()
    {
        filterChargingPorts = !filterChargingPorts;
        OnSearch(); // Update results
    }

    public void ToggleWiFi()
    {
        filterWiFi = !filterWiFi;
        OnSearch(); // Update results
    }

    public void ToggleIndoors()
    {
        filterIndoors = !filterIndoors;
        filterOutdoors = false; // Can't be indoors and outdoors at the same time
        OnSearch(); // Update results
    }

    public void ToggleOutdoors()
    {
        filterOutdoors = !filterOutdoors;
        filterIndoors = false; // Can't be indoors and outdoors at the same time
        OnSearch(); // Update results
    }


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


        //OLD FOREACH LOOP

        /*//foreach loop to loop through data and display results
        foreach (Location space in studySpaces)
        {
            if (space.name.ToLower().Contains(query))
            {
                // Create new result item
                GameObject newResult = Instantiate(resultPrefab, resultsContent);

                // Set text of the result with more data (name, postal code, etc.)
                newResult.GetComponentInChildren<TextMeshProUGUI>().text = space.name;

                *//*//OnClick event to open the details page
                newResult.GetComponent<Button>().onClick.AddListener(() => OpenDetailsPage(space));*//*
                Button resultButton = newResult.GetComponent<Button>();
                if (resultButton != null)
                {
                    // Make sure to pass the correct space data to the OpenDetailsPage method
                    resultButton.onClick.AddListener(() => OpenDetailsPage(space));
                }
            }
        }
    }*/
        foreach (Location space in studySpaces)
        {
            //check query
            if (!space.name.ToLower().Contains(query))
                continue;

            //check through filters
            if (!string.IsNullOrEmpty(selectedNoiseLevel) && space.noiseLevel != selectedNoiseLevel)
                continue;
            if (filterChargingPorts && !space.hasChargingPorts)
                continue;
            if (filterWiFi && !space.hasWiFi)
                continue;
            if (filterIndoors && !space.isIndoors)
                continue;
            if (filterOutdoors && space.isIndoors) // Outdoors means not indoors
                continue;

            //if criteria match, display result
            GameObject newResult = Instantiate(resultPrefab, resultsContent);
            newResult.GetComponentInChildren<TextMeshProUGUI>().text = space.name;

            Button resultButton = newResult.GetComponent<Button>();
            if (resultButton != null)
            {
                resultButton.onClick.AddListener(() => OpenDetailsPage(space));
            }
        }
    }

    public void OpenDetailsPage(Location location)
    {
        // Set the details text fields with the location information
        nameText.text = location.name;
        postCodeText.text = location.postCode;
        descText.text = location.desc;
        /*detailsCoordinatesText.text = "Coordinates: " + location.coordinates.ToString();*/

        // Show the details page
        detailsPage.SetActive(true);
    }

    
    public void CloseDetailsPage()
    {
        detailsPage.SetActive(false);
    }
}

[System.Serializable]
public class Location
{
    //fields for 
    public string name;         //Name of location
    public string postCode;     //Post Code of location
    public Vector2 coords;      //Maps Co-ords
    public string desc;         //Breif description

    //criteria/filters:
    public string noiseLevel;  //Low/Med/High
    public bool hasChargingPorts; //y/n
    public bool hasWiFi; //y/n
    public bool isIndoors; //y/n

    // Constructor to initialize a new location
    public Location(string name, string postCode, Vector2 coordinates, string description, string noiseLevel, bool hasChargingPorts, bool hasWiFi, bool isIndoors)
    {
        this.name = name;
        this.postCode = postCode;
        this.coords = coordinates;
        this.desc = description;

        this.noiseLevel = noiseLevel;
        this.hasChargingPorts = hasChargingPorts;
        this.hasWiFi = hasWiFi;
        this.isIndoors = isIndoors;
    }
}

//SCRIPT USING FIREBASE (DISUSED)

/*using Firebase.Database;
using Firebase.Extensions;
using UnityEngine;
using TMPro;
using Firebase;

public class SearchManager : MonoBehaviour
{
    public TMP_InputField searchInputField;  // The input field where the user types the search term
    public TMP_Text searchResultText;        // The UI text element to display the result
    private DatabaseReference databaseReference;

    *//*FirebaseApp app = FirebaseApp.DefaultInstance;*//*

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


        *//*void OnDestroy()
        {
            // Make sure to clean up Firebase when exiting play mode
            
            if (app != null)
            {
                app.Dispose();
                Debug.Log("Firebase app disposed.");
            }
        }*//*


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
}*/


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