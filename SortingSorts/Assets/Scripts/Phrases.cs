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
    
    public List<string> phrases; //
	public List<string> hints; // 
	public Text hintText; //

    private string currentPhrase; //

    public GameObject blockPrefab; //
	public GameObject emptyBlockPrefab; //
    public GameObject underscorePrefab; //

	public Text scoreText; //
	public Text timerText; //

	public Transform box;
	public Transform stick;
	public float stickOffsetX;
	public float stickOffsetY;
	public bool playing; //
	public Text finalScore; //
	public GameObject ggMenu; //

	private float roundTimer; //
	private int score; //
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

	// Using to test functions for now
	void Start()
	{
		score = 0;
		roundTimer = 120f;

		underscores = new List<Underscore> ();
		blocks = new List<Transform> ();

		blockWidth = blockPrefab.GetComponentInChildren<SpriteRenderer> ().bounds.size.x + 1.2f;
		currentPhrase = "";
		ChoosePhrase();
		SortPhrase();
	}

    void ChoosePhrase()
    {
        int i = Random.Range(0, phrases.Count);

		while (currentPhrase == phrases [i])
		{
			i = Random.Range(0, phrases.Count);
		}
		currentPhrase = phrases[i];
		hintText.text = hints[i];

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
				playing = false;
				ggMenu.SetActive (true);
				finalScore.text = score.ToString();
			}
		}

	}

	public void Play()
	{
		playing = true;
	}

    void SortPhrase()
    {
		float x = stickOffsetX + blockWidth;
		float y = stickOffsetY;
		int nextSpace = 0;
		for (int i = 0; i < currentPhrase.Length; i++) 
		{
			if (currentPhrase[i] == ' ') 
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
					while (x >= 60 - blockWidth * 10)
					{
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
					while (x >= 60 - blockWidth * 10)
					{
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
				{
					nextSpace = currentPhrase.Length - 1;
				}



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
		float offset = blockWidth - 1.5f;
		int tries = 0;
		int width = Mathf.Min(currentPhrase.Length /3, 7);
		width = Mathf.Max (width, 5);
		do
		{
			x = -width* offset + Random.Range (0, width*2) * offset;
			same = false;

			for (int i = 0; i < blocks.Count; i++) 
			{
				if (x == blocks[i].localPosition.x && y == blocks[i].localPosition.y) 
				{
					same = true;
					y = -15;
				}
			}
			if(same || x > width * offset + offset || x < -width * offset - offset)
			{
				if(y < -15)
				{
					same = true;
					y = -15;
					continue;
				}
				else
				{
					y = -30;
					x = -width* offset + Random.Range (0, width*2) * offset;
				}
			}
			same = false;
			for (int i = 0; i < blocks.Count; i++) 
			{
				if (x == blocks[i].localPosition.x && y == blocks[i].localPosition.y) 
				{
					same = true;
					y = -15;
				}
			}
			tries++;
			if(tries > 100)
			{
				same = false;
			}
		} while(same == true);

	
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
			if (currentPhrase [i] != ' ')
			{
				currentPhraseNoSpaces += currentPhrase [i];
			}
		}

		for (int i = 0; i < underscores.Count; i++) 
		{
			if (underscores [i].transform.childCount >= 1)
			{
				if (underscores [i].transform.childCount > 1) 
				{
					Letter l = underscores [i].transform.GetComponentInChildren<Letter>();
					submitted += l.letter;
				}
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

				blocks[i].position = blocks[i].GetComponent<Letter>().startPos;
			}
			for (int i = 0; i < underscores.Count; i++) 
			{
				underscores [i].taken = false;
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
		roundTimer += 60f;
	}

	public void Restart()
	{
		Reset ();
		roundTimer = 120;
		playing = true;
	}
}


