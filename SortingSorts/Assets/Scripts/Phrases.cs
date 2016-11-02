using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

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
	public Text scoreText;
	public Text timerText;

	private float roundTimer;
	private int score;
    private int screenCenter;
	private List<Underscore> underscores;
	private List<Transform> blocks;
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

	void Update()
	{
		timerText.text = roundTimer.ToString ("##.##");
		roundTimer -= Time.deltaTime;
		if (roundTimer < 0) 
		{
			score -= 5;
			scoreText.text = score.ToString ();
			Reset ();
		}
	}

    void SortPhrase()
    {
		
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
		bool same;
		float x;
		do
		{
			x = -currentPhrase.Length/2 * 6 + Random.Range (0, currentPhrase.Length) * 6;
			same = false;
			for (int i = 0; i < blocks.Count; i++) 
			{
				if (x == blocks [i].position.x) 
				{
					same = true;
				}
			}

		}while(same == true);
        Letter l = Instantiate(blockPrefab).GetComponent<Letter>();
		l.transform.position = new Vector2 (x, -10);
        l.letter = c;
        l.AssignLetter();
		blocks.Add (l.transform);
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

	//test if the phrase is correct
	public void testPhrase()
	{
		string submitted = "";
		string currentPhraseNoSpaces = "";
		for (int i = 0; i < currentPhrase.Length; i++) 
		{
			if (currentPhrase [i] != ' ') {
				currentPhraseNoSpaces += currentPhrase [i];
			}
		}

		for (int i = 0; i < underscores.Count; i++) 
		{
			if (underscores [i].transform.childCount >= 1) {
				Letter l = underscores [i].transform.GetComponentInChildren<Letter>();
				submitted += l.letter;
			}

		}

		if (submitted == currentPhraseNoSpaces) 
		{
			score += currentPhraseNoSpaces.Length;
			scoreText.text = score.ToString ();
			Reset ();
		} 
		else 
		{
			for (int i = 0; i < blocks.Count; i++) 
			{
				blocks [i].transform.SetParent (null);

				blocks[i].position = new Vector2 (blocks[i].position.x ,-10);
			}

		}

	}

	void Reset()
	{
		for (int i = 0; i < blocks.Count; i++) 
		{
			Destroy (blocks [i].gameObject);
		}
		for (int i = 0; i < underscores.Count; i++) 
		{
			Destroy (underscores [i].gameObject);
		}

		underscores = new List<Underscore> ();
		blocks = new List<Transform> ();
		ChoosePhrase();
		SortPhrase();
		roundTimer = 30f;
	}

    // Using to test functions for now

    void Start()
    {
		score = 0;
		roundTimer = 30f;
		underscores = new List<Underscore> ();
		blocks = new List<Transform> ();
        ChoosePhrase();
        SortPhrase();
    }
}


