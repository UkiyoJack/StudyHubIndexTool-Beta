using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class buttonLoic : MonoBehaviour
{
    // Start is called before the first frame update
    public void FinderButton()
    {
        SceneManager.LoadScene(1);
    }

    public void SubmitButton()
    {
        SceneManager.LoadScene(2);
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene(0);
    }
}