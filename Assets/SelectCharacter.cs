using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SelectCharacter : MonoBehaviour
{
    public const string LISTZ = "listz";

    public const string KANEKO = "kaneko";

    public const string KARUMA = "karuma";

    public string pickString;

    public string id;

    public void PickString ()
    {
        switch(id)
        {
            case LISTZ:
                PlayerPrefs.SetString("avatar", LISTZ);
                break;
            case KANEKO:
                PlayerPrefs.SetString("avatar", KANEKO);
                break;
            case KARUMA:
                PlayerPrefs.SetString("avatar", KARUMA);
                break;
        }

        SceneManager.LoadScene(1);

        return;
    }
}
