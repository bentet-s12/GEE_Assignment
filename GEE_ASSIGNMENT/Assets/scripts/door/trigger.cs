using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class portaltrigger : MonoBehaviour
{
    //place this class on a do not destroy object
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] private string loadscene;
    [SerializeField] private string currentscenename;
    [SerializeField] private List<string> scenenames;


    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("contact made");
        if (other.CompareTag("Player"))
        {
            Debug.Log(scenenames.Count);
            if (currentscenename.Equals("lobby"))
            {
                if (scenenames.Count > 0)
                {
                    int chosenscene = Random.Range(0, scenenames.Count - 1);
                    loadscene = scenenames[chosenscene];
                    SceneManager.LoadScene(loadscene);
                    //inform the scene manager that the scene is loading to change variables on scene start here
                }

            }
            else
            {
                if (scenenames.Count > 0)
                {
                    int chosenscene = Random.Range(0, scenenames.Count - 1);
                    loadscene = scenenames[chosenscene];
                    SceneManager.LoadScene(loadscene);
                    //inform the scene manager that the scene is loading to change variables on scene start here
                }
            }
        }
    }
}