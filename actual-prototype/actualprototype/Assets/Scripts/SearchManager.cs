using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;
using System;
using Unity.VisualScripting;

public class SearchPage : MonoBehaviour
{

    public TMP_InputField searchInputField; //input field ref

    public Transform resultsContent; //content area ref

    public GameObject resultPrefab; //search results prefab

    public GameObject detailsPage; //reference to the details page

    public GameObject filtersPage; //reference to the details page

    public TextMeshProUGUI nameText; //text field for the name on the details page

    public TextMeshProUGUI postCodeText; //text field for the post code

    public TextMeshProUGUI descText; //text field for the description

    public TextMeshProUGUI detailsCoordinatesText; //text field for coords

    private Location currentSelectedLocation;


    //Active Filters:
    private string selectedNoiseLevel = "";

    private bool filterChargingPorts = false;

    private bool filterWiFi = false;

    private bool filterIndoors = false;

    private bool filterOutdoors = false;



    //placeholder for location data
    public static List<Location> studySpaces = new List<Location>();

    private void Start()
    {
        //initalize with data preloaded.
        studySpaces.Add(new Location("Pigeon Park, Birmingham", "B3 2QB", 52.481327867963614f, -1.8982472147330702f, "Public park in city centre with grassy areas and benches.", "High", false, false, false));
        studySpaces.Add(new Location("Waterstones Book Store", "B1 7SL", 52.478861828142996f, -1.8946746259916927f, "Cosy bookstore with cafe and seating upstairs.", "Low", true, true, true));
        studySpaces.Add(new Location("BCU drop-in Libraries", "B4 7BD", 52.48327379633388f, -1.8828681501529363f, "Drop-in study spaces (level 1, Curzon Building)", "Low", true, true, true));
        studySpaces.Add(new Location("Starbucks Coffee Birmingham", "B2 4JH", 52.4793208620976f, -1.89930639905682f, "Starbucks Coffee Shop with seating", "Medium", true, true, true));
        studySpaces.Add(new Location("Outdoor Quiet Seating (West side Birmingham)", "B1 1TT", 52.4782161f, -1.9065645f, "Outdoor quiet study space with stair-like seating", "Low", false, false, false));
        studySpaces.Add(new Location("Birmingham Library", "B1 2ND", 52.479546929725664f, -1.908384190556989f, "Europe's largest public library, large amount of study spaces.", "Medium", true, true, true));
        studySpaces.Add(new Location("Solihull Core Library", "B91 3RG", 52.412465036826234f, -1.779342922462714f, "Reasonably sized library with study areas", "Low", true, false, true));
        studySpaces.Add(new Location("Brueton Park, Solihull", "B91 3DL", 52.40861256376181f, -1.762546757205329f, "Large park with quiet study spaces", "Medium", false, false, false));
        studySpaces.Add(new Location("Touchwood Shopping Centre, Solihull", "B91 3GJ", 52.41312313738989f, -1.7797520319971898f, "Shopping centre with seating and study spaces", "High", true, false, true));
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

        if (location == null)
        {
            Debug.LogError("OpenDetailsPage: Location is null");
            return;
        }

        

        currentSelectedLocation = location; //store selected location value here

        //set the details text fields with the location information
        nameText.text = location.name;
        postCodeText.text = location.postCode;
        descText.text = location.desc;
        /*detailsCoordinatesText.text = currentSelectedLocation.coordsX.ToString() + currentSelectedLocation.coordsY.ToString();*/



        //show the details page
        detailsPage.SetActive(true);
        filtersPage.SetActive(false);
    }


    public void CloseDetailsPage()
    {
        detailsPage.SetActive(false);
        filtersPage.SetActive(true);
    }

    public void OpenInMapsButton()
    {
        /*string mapUrl = $"https://www.google.com/maps/search/?api=1&query={studySpaces.coordsX},{studySpaces.coordsY}";
        Application.OpenURL(mapUrl);*/

        if (currentSelectedLocation != null)
        {
            string mapUrl = $"https://www.google.com/maps/search/?api=1&query={currentSelectedLocation.coordsX},{currentSelectedLocation.coordsY}";
            Application.OpenURL(mapUrl);
        }
        else
        {
            Debug.LogWarning("No location selected to open in Maps.");
        }
    }
}

[System.Serializable]
public class Location
{
    //fields for 
    public string name;         //Name of location
    public string postCode;     //Post Code of location
    public float coordsX;       //Maps X Co-ords
    public float coordsY;       //Maps Y Co-ords
    public string desc;         //Breif description

    //criteria/filters:
    public string noiseLevel;  //Low/Med/High
    public bool hasChargingPorts; //y/n
    public bool hasWiFi; //y/n
    public bool isIndoors; //y/n

    // Constructor to initialize a new location
    public Location(string name, string postCode, float coordsX, float coordsY, string description, string noiseLevel, bool hasChargingPorts, bool hasWiFi, bool isIndoors)
    {
        this.name = name;
        this.postCode = postCode;
        this.coordsX = coordsX;
        this.coordsY = coordsY;
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