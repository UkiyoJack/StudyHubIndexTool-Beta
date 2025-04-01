/*using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    public static DataManager Instance { get; private set; }
    public List<Location> studySpaces = new List<Location>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeSampleData();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void InitializeSampleData()
    {
        studySpaces.Add(new Location("Library", "AB12 3CD", new Vector2(51.5074f, -0.1278f), "Quiet library with Wi-Fi", "Low", true, true, true));
        studySpaces.Add(new Location("Cafe", "EF45 6GH", new Vector2(51.5094f, -0.1280f), "Cozy cafe with power outlets", "Medium", true, true, false));
        studySpaces.Add(new Location("Study Room", "GH78 9IJ", new Vector2(51.5054f, -0.1275f), "Private study room", "Low", false, true, true));
    }
}*/