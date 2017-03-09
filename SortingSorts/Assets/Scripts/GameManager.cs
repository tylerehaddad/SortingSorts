using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

	/// <summary>
	/// List phrases contains the list of all phrases
	/// to be used in the game. List is public
	/// so that the list can be easily edited
	/// in the Unity editor at any time.
	/// The string currentPhrase is used to 
	/// store the selected phrase for use
	/// in generating spaces and letter blocks.
	/// </summary>

	[Header("Phrases:")]
	public List<string> phrases;
	public List<string> hints;

	[Header("Tutorials:")]
	public List<GameObject> tutorials;
	public GameObject tutorialMenu;

	[Header("UI References:")]
	public Text hintText;
	public Text scoreText;
	public Text timerText;
	public GameObject gameOverMenu;
	public GameObject menu;
	public Text finalScore;

	[Header("UI Labels:")]
	public string hintTextLabel;

	[Header("Prefabs:")]
	public GameObject blockPrefab;
	public GameObject emptyBlockPrefab;
	public GameObject underscorePrefab;

	[Header("Stick:")]
	public Transform stick;
	public float stickOffsetX;
	public float stickOffsetY;

	[Header("Balanceable Variables:")]
	public float roundTimerStart = 120f;

	[HideInInspector]
	public bool playing;

	//Phrase Variables.
	private string currentPhrase;

	//Gameplay Variables.
	private float roundTimer;
	private int score;

	//Game Objects.
	private List<Underscore> underscores;
	private List<Transform> blocks;

	//Judgemental Variables.
	private float blockWidth;

	//Tutorial Variables.\
	private int currentTutorial;

	void Start ()
	{
		//Init Variables.
		score = 0;
		roundTimer = roundTimerStart;
		currentPhrase = "";

		currentTutorial = 0;

		//Init Lists.
		underscores = new List<Underscore> ();
		blocks = new List<Transform> ();

		//I'M NOT FAT, I'M JUST BIG BONED.
		blockWidth = blockPrefab.GetComponentInChildren<SpriteRenderer> ().bounds.size.x + 1.2f;
	}

	void Update ()
	{
		if (playing) 
		{
			//Display the Timer.
			string minutes = Mathf.Floor(roundTimer / 60).ToString("00");
			string seconds = (roundTimer % 60).ToString("00");
			timerText.text = Mathf.Floor(roundTimer / 60).ToString("00") + ":" + Mathf.Floor(roundTimer % 60).ToString("00");

			//Oh no! The time is going down! 
			roundTimer -= Time.deltaTime;

			//Game Over! You suck!
			if (roundTimer < 0) 
			{
				playing = false;
				gameOverMenu.SetActive (true);
				finalScore.text = score.ToString();
			}
		}

		//This activates the current tutorial gameobject on the tutorial menu.
		if (tutorialMenu.activeSelf == true)
		{
			tutorials[currentTutorial].SetActive(true);
			for(int i = 0; i < tutorials.Count; i++)
			{
				if(tutorials[i].activeSelf == true)
				{
					if (i != currentTutorial)
					{
						tutorials[i].SetActive(false);
					}
				}
			}
		}

		//Bail! Abandon ship!
		if (Input.GetKey ("escape"))
		{
			if (menu.activeSelf == true)
			{
				//*rage quits*
				QuitApp ();
			} else
			{
				//The player has given up.
				playing = false;

				//Make the menu exist again.
				menu.SetActive (true);
			}
		}
	}

	//Yo! Start up the game!
	public void Play()
	{
		//Is the player playing? Hell yea they are.
		playing = true;

		//Nuke the playing field.
		Reset ();

		//Set the Variables.
		score = 0;
		roundTimer = roundTimerStart;
	}

	//OH BOI, WHAT PHRASE IS IT GOING TO THROW AT ME?!?
	void ChoosePhrase()
	{
		//Choose a phrase! Any phrase!
		int i = Random.Range(0, phrases.Count);

		//If it's the same, pick a new one stupid.
		while (currentPhrase == phrases[i])
		{
			i = Random.Range(0, phrases.Count);
		}

		//Set the phrase.
		currentPhrase = phrases[i];

		//Display the Hint Text.
		hintText.text = hintTextLabel + hints[i];

		//Debug this!
		Debug.Log(currentPhrase);
	}

	// ------------------ I DON'T UNDERSTAND -----------------------

	//Sort out the phrase.
	void SortPhrase()
	{
		float x = stickOffsetX + blockWidth;
		float y = stickOffsetY;

		//When is the next space?
		int nextSpace = 0;

		//Finds the next space.
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

			//Word Wrapping.
			if (currentPhrase[i] != ' ')
			{
				//Spawn the character we are currently on as a block.
				GenerateBlocks(currentPhrase[i]);

				//Creates the underscore.
				Transform t = Instantiate(underscorePrefab).transform;

				//Parents it.
				t.SetParent(stick);


				if (x - blockWidth * (nextSpace - i) <= stickOffsetX - blockWidth * 9 && y == stickOffsetY) 
				{
					//Adds empty blocks until the end of the line.
					while (x >= stickOffsetX - blockWidth * 10)
					{
						Transform empty = Instantiate(emptyBlockPrefab).transform;
						empty.SetParent(stick);
						empty.localPosition = new Vector2(x,y);
						x -= blockWidth;
					}

					y = stickOffsetY + 12;
					x = stickOffsetX;
				}
				else if (x - blockWidth * (nextSpace - i) <= stickOffsetX - blockWidth * 9 && y == stickOffsetY + 12) 
				{
					while (x >= stickOffsetX - blockWidth * 10)
					{
						Transform empty = Instantiate(emptyBlockPrefab).transform;
						empty.SetParent(stick);
						empty.localPosition = new Vector2(x,y);
						x -= blockWidth;
					}
					y = -2;
					x = 60;
				}

				//Move it to it's new home.
				t.localPosition = new Vector2(x,y);

				//Add it to the list.
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
					x = -width * offset + Random.Range(0, width * 2) * offset;
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
	// ------------------ END OF ME NOT UNDERSTANDING -----------------------

	void Reset()
	{
		//Destroy all the blocks.
		for (int i = 0; i < blocks.Count; i++) 
		{
			Destroy (blocks[i].gameObject);
		}

		//Destroy underscores.
		for (int i = 0; i < underscores.Count; i++) 
		{
			Destroy (underscores[i].gameObject);
		}

		//Destroy the empty blocks.
		foreach (GameObject g in GameObject.FindGameObjectsWithTag ("EmptyBlock"))
		{
			Destroy (g);
		}

		//Reset the game lists.
		underscores = new List<Underscore> ();
		blocks = new List<Transform> ();

		//New phrase.
		ChoosePhrase();
		SortPhrase();

		//Give the player a boost in time.
		roundTimer += 60f;
	}

	//Change the current tutorial.
	public void TutorialChange(bool postive)
	{
		//Advance in the tutorial.
		if (postive)
		{
			currentTutorial++;
		} else
		{
			currentTutorial--;
		}

		//Loop the tutorial.
		if (currentTutorial < 0)
		{
			currentTutorial = tutorials.Count - 1;
		} else if(currentTutorial > tutorials.Count - 1)
		{
			currentTutorial = 0;
		}
	}

	//Restart the game.
	public void Restart()
	{
		Play ();
	}

	//I've had enough...
	public void QuitApp()
	{
		Application.Quit();
	}
}
