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
	public GameObject emptyBlockPrefab;
    public GameObject underscorePrefab;
	public Text scoreText;
	public Text timerText;
	public Transform box;
	public Transform stick;
	public bool playing;


	private float roundTimer;
	private int score;
    private int screenCenter;
	private List<Underscore> underscores;
	private List<Transform> blocks;
	private float blockWidth;
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
		if (playing) 
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

	}

	public void Play()
	{
		playing = true;
	}

    void SortPhrase()
    {
		float x = 60 + blockWidth;
		float y = -26;
		int nextSpace = 0;
		for (int i = 0; i < currentPhrase.Length; i++) 
		{
			if (currentPhrase [i] == ' ') 
			{
				nextSpace = i;
				break;
			}
		}

        for (int i = 0; i < currentPhrase.Length; i++)
        {
			x -= blockWidth;

            if (currentPhrase[i] != ' ')
            {
                GenerateBlocks(currentPhrase[i]);
                Transform t = Instantiate(underscorePrefab).transform;
				t.SetParent(stick);
				if (x - blockWidth *(nextSpace - i) <= 60 - blockWidth * 9 && y == -26) 
				{
					

					while (x >= 60 - blockWidth * 10) {
						Transform empty = Instantiate(emptyBlockPrefab).transform;
						empty.SetParent(stick);
						empty.localPosition = new Vector2(x,y);
						x -= blockWidth;
					}

					y = -14;
					x = 60;
				}
				else if (x - blockWidth *(nextSpace - i) <= 60 - blockWidth * 9 && y == -14) 
				{
					while (x >= 60 - blockWidth * 10) {
						Transform empty = Instantiate(emptyBlockPrefab).transform;
						empty.SetParent(stick);
						empty.localPosition = new Vector2(x,y);
						x -= blockWidth;
					}
					y = -2;
					x = 60;
				}

				t.localPosition = new Vector2(x,y);

				underscores.Add (t.GetComponent<Underscore>());
            }
			else
			{
				//Find Next Word Last I
				for (int j = i+1; j < currentPhrase.Length; j++) 
				{
					if (currentPhrase [j] == ' ') 
					{
						nextSpace = j-1;
						break;
					}
				}
				if (nextSpace <= i)
					nextSpace = currentPhrase.Length-1;



				Transform t = Instantiate(emptyBlockPrefab).transform;
				t.SetParent(stick);
				t.localPosition = new Vector2(x,y);

			}
        }
    }

	void GenerateBlocks(char c)
    {
		bool same;
		float x;
		float y = -15;
		do
		{
			x = -currentPhrase.Length/2 * blockWidth + Random.Range (0, currentPhrase.Length) * blockWidth;
			same = false;
			if(x > 70 || x < -70)
			{
				y -= 15;
				x = (x > 70)?x-70:x+70;
			}
			for (int i = 0; i < blocks.Count; i++) 
			{
				if (x == blocks[i].localPosition.x && y == blocks[i].localPosition.y) 
				{
					same = true;
					y = -15;
				}
			}

				

		}while(same == true);

	
		Letter l = Instantiate (blockPrefab).GetComponent<Letter> ();
		l.transform.localPosition = new Vector2 (x, y);
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

		foreach (GameObject g in GameObject.FindGameObjectsWithTag ("EmptyBlock"))
		{
			Destroy (g);
		}

		underscores = new List<Underscore> ();
		blocks = new List<Transform> ();
		ChoosePhrase();
		SortPhrase();
		roundTimer = 60f;
	}

    // Using to test functions for now

	void Start()
    {
		score = 0;
		roundTimer = 60f;
		underscores = new List<Underscore> ();
		blocks = new List<Transform> ();
		blockWidth = blockPrefab.GetComponentInChildren<SpriteRenderer> ().bounds.size.x + 1.2f;
		print (blockWidth);
        ChoosePhrase();
        SortPhrase();
    }
}


