using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]
public class GameManager : MonoBehaviour
{
	[Header("Phrases:")]
	public List<string> phrases;
	public List<string> hints;
	public List<string> clues;
	[Range(0, 3)]
	public List<int> difficulty;

	[Header("Game UI:")]
	public Text hintText;

	public Text scoreText;
	public GameObject scoreUI;
	public Text scoreAdditionText;
	public GameObject scoreAdditionUI;

	public Text difficultyText;
	public GameObject difficultyUI;
	public GameObject difficultyAdditionUI;

	public Text timerText;
	public GameObject timerUI;
	public Text timerAdditionText;
	public GameObject timerAdditionUI;

	public Text clueText;
	public GameObject clueUI;
	public Text clueUnlockText;
	bool clueUnlocked = false;
	public string clueUnlockDefault;
	float clueUnlockTime = 0;
	public GameObject clueUnlockedUI;
	public GameObject clueLockedUI;

	public GameObject flippedTutorialUI;

	public Text finalScore;
	public Text finalPhrase;

	[Header("Drawers:")]
	public DrawerBehavior sponsorDrawer;
	public DrawerBehavior menuDrawer;
	public DrawerBehavior overDrawer;

	[Header("UI Labels:")]
	public string hintTextLabel;

	[Header("Tutorial UI References:")]
	public GameObject grabArrow;
	public Text grabText;
	public GameObject placeArrow;
	public Text placeText;
	public GameObject printArrow;
	public Text printText;

	[Header("Tutorial UI Variables:")]
	public float tutorialArrowLerpSpeed = 0.125f;

	[Header("Prefabs:")]
	public GameObject blockPrefab;
	public GameObject emptyBlockPrefab;
	public GameObject underscorePrefab;

	[Header("Stick:")]
	public Transform stick;
	public float stickOffsetX;
	public float stickOffsetY;
	public float lineMax = 3;
	public float lineAmount = 10;

	[Header("Drawer:")]
	public Transform drawer;

	[Header("Balanceable Variables:")]
	public float roundTimerStart = 120f;

	[Header("Sound Effects:")]
	public AudioClip clickClip;
	public AudioClip printClip;
	public AudioClip endClip;

	[HideInInspector]
	public bool playing;

	[HideInInspector]
	public bool grabTutorial = true;
	[HideInInspector]
	public bool placeTutorial = false;
	[HideInInspector]
	public bool printTutorial = true;
	private bool printTutorialDrawn = false;
	[HideInInspector]
	private bool tutorial = false;

	[HideInInspector]
	public int currentDifficulty = 0;
	[HideInInspector]
	public int currentDifficultyGoal = 1;
	[HideInInspector]
	public int currentLevel = 0;

	private bool difficultyUp = false;

	//Phrase Variables.
	private string currentPhrase;

	//Gameplay Variables.
	private float roundTimer;
	private int score;
	[HideInInspector]
	public bool paused = false;

	//Game Objects.
	private List<Underscore> underscores;
	private List<Transform> blocks;

	//Judgemental Variables.
	private float blockWidth;
	private float blockHeight = 11;

	//Tutorial UI Images.
	private Image grabImage;
	private Image placeImage;
	private Image printImage;

	//Tutorial UI Alphas.
	private float grabAlpha = 0f;
	private float placeAlpha = 0f;
	private float printAlpha = 0f;

	private AudioSource source;

	void Start ()
	{
		//Init Variables.
		score = 0;
		roundTimer = roundTimerStart;
		currentPhrase = "";

		//Init Lists.
		underscores = new List<Underscore> ();
		blocks = new List<Transform> ();

		//I'M NOT FAT, I'M JUST BIG BONED.
		blockWidth = blockPrefab.GetComponentInChildren<SpriteRenderer> ().bounds.size.x;

		source = GetComponent<AudioSource>();

		printTutorial = true;

		Invoke ("CloseSponsor", 2f);
	}

	void Update ()
	{
		if (playing)
		{
			//Game Over! You suck!
			if (roundTimer < 0)
			{
				roundTimer = 0;
				playing = false;
				overDrawer.Open();
				drawer.GetComponent<GameDrawerBehavior> ().Close ();
				stick.GetComponent<GameDrawerBehavior> ().Open ();

				scoreUI.GetComponent<UIBehavior> ().Hide();
				difficultyUI.GetComponent<UIBehavior> ().Hide();
				timerUI.GetComponent<UIBehavior> ().Hide();
				clueUI.GetComponent<UIBehavior> ().Hide();

				hintText.text = "";
				finalScore.text = score.ToString ();
				source.clip = endClip;
				source.Play();
			}

			if (!paused)
			{
				//Display the Timer.
				timerText.text = Mathf.Floor (roundTimer / 60).ToString ("00") + ":" + Mathf.Floor (roundTimer % 60).ToString ("00");

				//Oh no! The time is going down!
				roundTimer -= Time.deltaTime;
			}

			if (clueUnlocked)
			{
				if (clueUI.GetComponent<UIBehavior> ().hidden)
				{
					clueUI.GetComponent<UIBehavior> ().Show ();

					clueUnlockedUI.SetActive(true);
					clueLockedUI.SetActive(false);
				}
			} else
			{
				clueUnlockText.text = clueUnlockDefault + (Mathf.Floor (clueUnlockTime / 60).ToString ("00") + ":" + Mathf.Floor (clueUnlockTime % 60).ToString ("00")) + "?"; 

				if (clueUI.GetComponent<UIBehavior> ().hidden)
				{
					clueUnlockedUI.SetActive (false);
					clueLockedUI.SetActive (true);
				}
			}

			printTutorialDrawn = true;
		} else
		{
			printTutorialDrawn = false;
		}

		if (tutorial)
		{
			//Arrow Alphas.
			if (grabTutorial == true)
			{
				grabAlpha = Mathf.Lerp (grabAlpha, 1, tutorialArrowLerpSpeed);
			} else
			{
				grabAlpha = Mathf.Lerp (grabAlpha, 0, tutorialArrowLerpSpeed);
			}

			if (placeTutorial == true)
			{
				placeAlpha = Mathf.Lerp (placeAlpha, 1, tutorialArrowLerpSpeed);
				flippedTutorialUI.GetComponent<UIBehavior> ().Show ();
			} else
			{
				placeAlpha = Mathf.Lerp (placeAlpha, 0, tutorialArrowLerpSpeed);
				flippedTutorialUI.GetComponent<UIBehavior> ().Hide ();
			}



			if (printTutorial == true)
			{
				for (int i = 0; i < underscores.Count; i++)
				{
					if (underscores [i].transform.childCount <= 1)
					{
						printTutorialDrawn = false;
					}
				}
			} else
			{
				printTutorialDrawn = false;
			}

			if (printTutorialDrawn == true)
			{
				printAlpha = Mathf.Lerp (printAlpha, 1, tutorialArrowLerpSpeed);
				placeTutorial = false;
			} else
			{
				printAlpha = Mathf.Lerp (printAlpha, 0, tutorialArrowLerpSpeed);
				if (printTutorial == true && grabTutorial == false)
				{
					placeTutorial = true;
				}
			}
		} else
		{
			flippedTutorialUI.GetComponent<UIBehavior> ().Hide();
		}

		//Set the alphas.
		grabImage = grabArrow.GetComponent<Image>();
		Color grabArrowColor = grabImage.color;
		if (tutorial)
		{
			grabArrowColor.a = grabAlpha;
		} else
		{
			grabArrowColor.a = 0;
		}
		grabImage.color = grabArrowColor;

		Color grabTextColor = grabText.color;

		if (tutorial)
		{
			grabTextColor.a = grabAlpha;
		} else
		{
			grabTextColor.a = 0;
		}
		grabText.color = grabTextColor;


		placeImage = placeArrow.GetComponent<Image>();
		Color placeArrowColor = placeImage.color;
		placeArrowColor.a = placeAlpha;
		placeImage.color = placeArrowColor;

		Color placeTextColor = placeText.color;
		placeTextColor.a = placeAlpha;
		placeText.color = placeTextColor;


		printImage = printArrow.GetComponent<Image>();
		Color printArrowColor = printImage.color;
		printArrowColor.a = printAlpha;
		printImage.color = printArrowColor;

		Color printTextColor = printText.color;
		printTextColor.a = printAlpha;
		printText.color = printTextColor;

		//Bail! Abandon ship!
		if (Input.GetKeyDown ("escape"))
		{
			if (paused == false)
			{
				if (menuDrawer.closed == false)
				{
					//*rage quits*
					QuitApp ();
				} else
				{
					//The player has given up.
					playing = false;

					drawer.GetComponent<GameDrawerBehavior> ().Close ();
					stick.GetComponent<GameDrawerBehavior> ().Open ();

					scoreUI.GetComponent<UIBehavior> ().Hide ();
					difficultyUI.GetComponent<UIBehavior> ().Hide ();
					timerUI.GetComponent<UIBehavior> ().Hide ();
					clueUI.GetComponent<UIBehavior> ().Hide();
					HideAdditions ();
					hintText.text = "";


					//Make the menu exist again.
					menuDrawer.Open ();
				}
			}
		}
	}

	//Yo! Start up the game!
	public void Play()
	{
		//if (menuDrawer.moving == false)
		//{
			//Is the player playing? Hell yea they are.
			playing = true;

			//Nuke the playing field.
			Invoke("Reset", 0.75f);

			//Set the Variables.
			score = 0;
			roundTimer = roundTimerStart;
			currentDifficulty = 0;
			currentDifficultyGoal = 1;
			currentLevel = 0;
			difficultyText.text = "Level " + (currentDifficulty + 1).ToString ();
			clueUnlocked = false;
		//}
	}

	//OH BOI, WHAT PHRASE IS IT GOING TO THROW AT ME?!?
	void ChoosePhrase()
	{
		//Choose a phrase! Any phrase!
		int i = Random.Range(0, phrases.Count);

		//If it's the same or not the right difficulty, pick a new one stupid.
		while (difficulty [i] > currentDifficulty || currentPhrase == phrases[i])
		{
			i = Random.Range(0, phrases.Count);
		}

		//Set the phrase.
		currentPhrase = phrases[i];

		//Display the Hint Text.
		hintText.text = hintTextLabel + hints[i];

		clueText.text = clues [i];
		finalPhrase.text = currentPhrase;

		//Debug this!
		Debug.Log(currentPhrase);
	}

	void OnDrawGizmos()
	{
		Gizmos.color = Color.red;
		float size = 0.3f;

		Vector3 stickOffset = new Vector3 (stick.position.x + stickOffsetX, stick.position.y + stickOffsetY, 0f);
		Gizmos.DrawLine (stickOffset - Vector3.up * size, stickOffset + Vector3.up * size);
		Gizmos.DrawLine (stickOffset - Vector3.left * size, stickOffset + Vector3.left * size);
	}

	//Sort out the phrase.
	void SortPhrase()
	{
		float x = stick.position.x + stickOffsetX;
		float y = stick.position.y + stickOffsetY;

		int position = 0;
		int positionPhrase = 0;
		int nextSpace = 0;

		//Finds the first space.
		for (int n = 0; n < currentPhrase.Length; n++) 
		{
			if (currentPhrase[n] == ' ') 
			{
				nextSpace = n - 1;
				//Debug.Log ("The next space is at " + nextSpace);
				break;
			}
		}

		//Loop between every tile.
		while(position < lineMax * lineAmount)
		{
			//Set the x & y of the new tile.
			x = stick.position.x + stickOffsetX + blockWidth * (position - (Mathf.Floor(position/lineAmount) * lineAmount));
			y = stick.position.y + stickOffsetY + blockHeight * (Mathf.Floor(position/lineAmount));

			//Lets spawn something in the phrase if we can.
			if (positionPhrase < currentPhrase.Length)
			{
				if (currentPhrase [positionPhrase] != ' ' && nextSpace < ((Mathf.Floor(position/lineAmount) + 1) * lineAmount))
				{
					//Spawn the character we are currently on as a block.
					GenerateBlocks (currentPhrase [positionPhrase]);

					//Creates the underscore.
					Transform t = Instantiate (underscorePrefab).transform;

					//Parents it.
					t.SetParent (stick);

					//Move it.
					t.position = new Vector2 (x, y);

					//Add it.
					underscores.Add (t.GetComponent<Underscore>());

					//Go forwards.
					positionPhrase++;
				} else
				{
					//If space, look for the next one.
					if(currentPhrase [positionPhrase] == ' ')
					{
						//Look for the length of the next word.
						for (int j = positionPhrase + 1; j < currentPhrase.Length; j++) 
						{
							if (currentPhrase [j] == ' ') 
							{
								nextSpace = j - 1;
								break;
							}
						}

						//Make sure there is a space of some sort, because if not everything breaks at the end.
						if (nextSpace <= positionPhrase)
						{
							nextSpace = currentPhrase.Length + 1;
						}

						//Move the phrase forwards.
						positionPhrase++;
					}

					//Kill off blocks when new lining, spawn them if not.
					if (position - (Mathf.Floor (position / lineAmount) * lineAmount) != 0 && currentPhrase [positionPhrase] != ' ')
					{
						Transform empty = Instantiate (emptyBlockPrefab).transform;
						empty.SetParent (stick);
						empty.position = new Vector2 (x, y);
					} else
					{
						//Go back to compensate for the lack of an empty block.
						position--;
					}
				}
			} else //Nah fam let's spawn just a bunch of empty shit to fill in the rest.
			{
				Transform empty = Instantiate(emptyBlockPrefab).transform;
				empty.SetParent(stick);
				empty.position = new Vector2(x, y);
			}

			//Next position.
			position++;
		}

		scoreText.text = score.ToString ();

		drawer.GetComponent<GameDrawerBehavior> ().Open ();
		stick.GetComponent<GameDrawerBehavior> ().Close ();

		clueUnlockTime = (currentDifficulty + 1) * 10;

		scoreUI.GetComponent<UIBehavior> ().Show();
		difficultyUI.GetComponent<UIBehavior> ().Show();
		timerUI.GetComponent<UIBehavior> ().Show();
		clueUI.GetComponent<UIBehavior> ().Show();

		if(score > 0)
		{
			scoreAdditionUI.GetComponent<UIBehavior> ().Show();
			timerAdditionUI.GetComponent<UIBehavior> ().Show();
			Invoke ("HideAdditions", 3f);

			if (difficultyUp)
			{
				difficultyAdditionUI.GetComponent<UIBehavior> ().Show();
				difficultyUp = false;
			}
		}
		paused = false;
	}

	// ------------------ I DON'T UNDERSTAND -----------------------

	void GenerateBlocks(char c)
	{
		bool same;
		float x;
		float offset = blockWidth;
		float offset_y = -12;
		float y = offset_y;
		int tries = 0;
		int width = Mathf.Min(currentPhrase.Length /3, 7);
		width = Mathf.Max (width, 5);
		do
		{
			x = -width * offset + Random.Range (0, width * 2) * offset;
			same = false;

			for (int i = 0; i < blocks.Count; i++) 
			{
				if (x == blocks[i].GetComponent<Letter> ().startPos.x && y == blocks[i].GetComponent<Letter> ().startPos.y) 
				{
					same = true;
					y = offset_y;
				}
			}
			if(same || x > width * offset + offset || x < -width * offset - offset)
			{
				if(y < offset_y)
				{
					same = true;
					y = offset_y;
					continue;
				}
				else
				{
					y = offset_y - offset;
					x = -width * offset + Random.Range(0, width * 2) * offset;
				}
			}
			same = false;
			for (int i = 0; i < blocks.Count; i++) 
			{
				if (x == blocks[i].GetComponent<Letter> ().startPos.x && y == blocks[i].GetComponent<Letter> ().startPos.y) 
				{
					same = true;
					y = offset_y;
				}
			}

			tries++;

			if(tries > 100)
			{
				same = false;
			}
		} while(same == true);
			
		Letter l = Instantiate (blockPrefab).GetComponent<Letter> ();
		l.transform.SetParent (drawer);
		l.transform.position = new Vector2 (x, y + drawer.position.y);
		l.startPos.x = x;
		l.startPos.y = y;
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
		printTutorial = false;
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
			scoreAdditionText.text = "+" + currentPhraseNoSpaces.Length.ToString();

			//Give the player a boost in time.
			roundTimer += 30f;
			timerAdditionText.text = "+" + (Mathf.Floor (30f / 60).ToString ("00") + ":" + Mathf.Floor (30f % 60).ToString ("00")).ToString();

			source.clip = printClip;
			source.Play();

			currentLevel++;

			if (currentLevel >= currentDifficultyGoal)
			{
				if (currentDifficulty < 3)
				{
					currentDifficulty++;
					difficultyUp = true;
				} else
				{
					currentDifficulty = 3;
				}

				currentDifficultyGoal += 3 * currentDifficulty;
			}
				
			drawer.GetComponent<GameDrawerBehavior> ().Close ();
			stick.GetComponent<GameDrawerBehavior> ().Open ();

			scoreUI.GetComponent<UIBehavior> ().Hide();
			difficultyUI.GetComponent<UIBehavior> ().Hide();
			timerUI.GetComponent<UIBehavior> ().Hide();

			clueUnlocked = false;
			clueUI.GetComponent<UIBehavior> ().Hide();
			paused = true;

			hintText.text = "Success! Printing...";

			Invoke ("Reset", 2f);

			grabTutorial = false;
			placeTutorial = false;
			printTutorial = false;
			printTutorialDrawn = false;
		} 
		else 
		{
			for (int i = 0; i < blocks.Count; i++) 
			{
				blocks [i].transform.SetParent (drawer);

				blocks[i].transform.position = new Vector2 (blocks[i].GetComponent<Letter>().startPos.x, blocks[i].GetComponent<Letter>().startPos.y + drawer.position.y);
				blocks[i].GetComponent<SpriteRenderer> ().sortingOrder = 1;
				blocks[i].GetComponentsInChildren<SpriteRenderer> () [1].sortingOrder = -1;
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
		tutorial = true;
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

		if (currentDifficulty < 3)
		{
			difficultyText.text = "Level " + (currentDifficulty + 1).ToString ();
		} else
		{
			difficultyText.text = "Level MAX";
		}

		clueUnlocked = false;

		//New phrase.
		ChoosePhrase();
		SortPhrase();
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

	public void Click()
	{
		source.clip = clickClip;
		source.Play();
	}

	public void CloseSponsor()
	{
		sponsorDrawer.Move ();
	}

	public void HideAdditions()
	{
		scoreAdditionUI.GetComponent<UIBehavior> ().Hide();
		difficultyAdditionUI.GetComponent<UIBehavior> ().Hide();
		timerAdditionUI.GetComponent<UIBehavior> ().Hide();
	}

	public void UnlockClue()
	{
		if (!clueUnlocked)
		{
			clueUnlocked = true;
			roundTimer -= clueUnlockTime;
			clueUI.GetComponent<UIBehavior> ().Hide ();
		}
	}
}
