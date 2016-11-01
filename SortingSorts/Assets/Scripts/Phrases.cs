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
    public GameObject blockPrefab;
    public GameObject underscorePrefab;
    private int screenCenter;
	private List<Underscore> underscores;
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

    void SortPhrase()
    {
		//-currentPhrase.length / 2 * (letterWidth + spacing) + i * (letterWidth + spacing)
        for (int i = 0; i < currentPhrase.Length; i++)
        {
            if (currentPhrase[i] != ' ')
            {
                GenerateBlocks(currentPhrase[i]);
                Transform t = Instantiate(underscorePrefab).transform;
				t.position = new Vector2(currentPhrase.Length/2 * 6 - i * 6, 0);
				underscores.Add (t.GetComponent<Underscore>());
            }
        }
    }

    void GenerateBlocks(char c)
    {
        Letter l = Instantiate(blockPrefab).GetComponent<Letter>();
		l.transform.position = new Vector2 (0, 10);
        l.letter = c;
        l.AssignLetter();
    }

	public Underscore getNextUnderscore()
	{
		if (underscores.Count >= 1) 
		{
			for (int i = 0; i < underscores.Count; i++) 
			{
				if (underscores [i].taken == false) 
				{
					return underscores [i];
				}
			}
		}
		return null;
	}

    // Using to test functions for now

    void Start()
    {
		underscores = new List<Underscore> ();
        ChoosePhrase();
        SortPhrase();
    }
}


