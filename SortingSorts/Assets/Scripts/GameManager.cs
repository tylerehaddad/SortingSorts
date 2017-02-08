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

	[Header("UI References:")]
	public Text hintText;
	public Text scoreText;
	public Text timerText;
	public GameObject gameOverMenu;
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
		blockWidth = blockPrefab.GetComponentInChildren<SpriteRenderer> ().bounds.size.x + 1.2f;
	}

	void Update ()
	{
		if (playing) 
		{
			//Display the Timer.
			timerText.text = roundTimer.ToString ("##.##");

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

		//Bail! Abandon ship!
		if (Input.GetKey ("escape"))
		{
			QuitApp ();
		}
	}

	//Yo! Start up the game!
	public void Play()
	{
		playing = true;

		//Do the things.
		ChoosePhrase ();
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
			
		}
	}

	//I've had enough...
	public void QuitApp()
	{
		Application.Quit();
	}
}
