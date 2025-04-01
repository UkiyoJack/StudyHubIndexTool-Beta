using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;

public class SubmitPage : MonoBehaviour
{
    public TMP_InputField nameInputField;
    public TMP_InputField postCodeInputField;
    public TMP_InputField coordinatesXInputField;
    public TMP_InputField coordinatesYInputField;
    public TMP_InputField descriptionInputField;

    public TMP_Dropdown noiseLevelDropdown;
    public Toggle wifiToggle;
    public Toggle chargingPortsToggle;
    public Toggle indoorsToggle;
    public Toggle outdoorsToggle;

    /*public List<Location> studySpaces; //ref to list*/

    private SearchPage searchPage;  //ref to searchpage

    private void Start()
    {
        searchPage = FindObjectOfType<SearchPage>(); //find existing searchpage
    }

    public void OnSubmit()
    {
        //get data from input fields
        string locationName = nameInputField.text;
        string postCode = postCodeInputField.text;
        float coordinatesX = float.Parse(coordinatesXInputField.text);
        float coordinatesY = float.Parse(coordinatesYInputField.text);

        string description = descriptionInputField.text;

        //get noise level 
        string noiseLevel = noiseLevelDropdown.options[noiseLevelDropdown.value].text;

        //get values from the toggles
        bool hasWiFi = wifiToggle.isOn;
        bool hasChargingPorts = chargingPortsToggle.isOn;
        bool isIndoors = indoorsToggle.isOn;
        bool isOutdoors = outdoorsToggle.isOn;

        // Validation: Ensure the data is valid
        if (string.IsNullOrEmpty(locationName) || string.IsNullOrEmpty(postCode) || isIndoors == isOutdoors)
        {
            Debug.LogWarning("Invalid input! Ensure all fields are filled correctly and that indoors and outdoors are not both selected.");
            return;
        }

        //create new location
        Location newLocation = new Location(locationName, postCode, new Vector2(coordinatesX, coordinatesY), description, noiseLevel, hasChargingPorts, hasWiFi, isIndoors);

        // Add the new location to the list of study spaces
        SearchPage.studySpaces.Add(newLocation);

        //log the new location
        Debug.Log($"New location added: {newLocation.name}, {newLocation.postCode}, {newLocation.coords}, {newLocation.desc}");

        //log entire location list
        Debug.Log("Current list of locations:");
        foreach (Location location in SearchPage.studySpaces)
        {
            Debug.Log($"{location.name} - {location.postCode} - {location.coords}");
        }

        Debug.Log($"New location '{newLocation.name}' added successfully!");

        //clear fields afterwards
        ClearInputFields();
    }

    //ensure input fields are cleared
    private void ClearInputFields()
    {
        nameInputField.text = "";
        postCodeInputField.text = "";
        coordinatesXInputField.text = "";
        coordinatesYInputField.text = "";
        noiseLevelDropdown.value = 0; // Reset to the first dropdown option
        wifiToggle.isOn = false;
        chargingPortsToggle.isOn = false;
        indoorsToggle.isOn = false;
        outdoorsToggle.isOn = false;
    }
}
