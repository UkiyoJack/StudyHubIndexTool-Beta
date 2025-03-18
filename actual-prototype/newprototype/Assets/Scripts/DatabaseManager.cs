/*using Firebase.Database;
using Firebase.Extensions;
using UnityEngine;

public class RetrieveData : MonoBehaviour
{
    private DatabaseReference databaseReference;

    void Start()
    {
        // Get a reference to the database
        databaseReference = FirebaseDatabase.DefaultInstance.RootReference;

        // Retrieve data from the 'Locations' node
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

            // Iterate through each location in 'Locations'
            foreach (DataSnapshot locationSnapshot in snapshot.Children)
            {
                // Get the 'Name' field from each location entry
                if (locationSnapshot.Child("Name").Value != null)
                {
                    string locationName = locationSnapshot.Child("Name").Value.ToString();
                    Debug.Log("Location: " + locationName);

                    // You can populate UI elements with the locationName here if needed
                }
                else
                {
                    Debug.LogWarning("Name field is missing for a location.");
                }
            }
        });
    }
}
*/