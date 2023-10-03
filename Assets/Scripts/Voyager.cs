using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Voyager : MonoBehaviour
{
    public string destination;

    public void voyager()
    {
        if (destination != null && destination == "SpiderJour2")
        {
            Debug.Log("Réveillez-vous !");
            StartCoroutine(voyageDans(1f));
        }
        else if (destination != null && destination == "DreamJour1")
        {
            Debug.Log("Bonne Nuit !");
            StartCoroutine(voyageDans(1f));
        }
        else if(destination != null)
        {
            Debug.Log("Bon Voyage !");
            StartCoroutine(voyageDans(3f));
        }

    }

    IEnumerator voyageDans(float secondes)
    {
        yield return new WaitForSeconds(secondes);
        SceneManager.LoadScene(destination);
    }
}
    
