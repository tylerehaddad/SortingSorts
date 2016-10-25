using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class Phrases : MonoBehaviour
{

    /// <summary>
    /// List phrases contains the list of all phrases
    /// to be used in the game. List is public
    /// so that the list can be easily edited
    /// in the Unity editor at any time.
    /// The string currentPhrase is used to 
    /// store the selected phrase for use
    /// in generating spaces and letter blocks.
    /// </summary>
    
    
    public List<string> phrases;
    private string currentPhrase;

    /// <summary>
    /// ChoosePhrase() is used to 
    /// determine the phrase to be
    /// used in generation of the 
    /// spaces and letter blocks.
    /// </summary>

    void ChoosePhrase()
    {
        int i = Random.Range(0, phrases.Count);
        currentPhrase = phrases[i];
        Debug.Log(currentPhrase);
    }

    /// <summary>
    /// Uses currentPhrase to generate a string
    /// with no spaces and one with
    /// only underscores and spaces. The former
    /// is used to generate letter blocks, the
    /// latter to generate underscores for
    /// the letter blocks to attach to.
    /// </summary>

    void GenerateSpaces()
    {
        string phraseNoSpaces = currentPhrase.Replace(" ", "");
        string phraseBlanks = "";

        for (int i = 0; i < currentPhrase.Length - 1; i++)
        {
            if (currentPhrase[i] == ' ')
            {
                phraseBlanks += " ";
            }
            else
            {
                phraseBlanks += "_";
            }
        }

        Debug.Log(phraseBlanks);
    }

    // Using to test functions for now

    void Start()
    {
        ChoosePhrase();
        GenerateSpaces();
    }
}


