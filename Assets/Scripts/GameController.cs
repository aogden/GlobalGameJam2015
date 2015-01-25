﻿using UnityEngine;
using System.Collections;

public enum GameStage
{
	StageStart,
	StageIntro1,
	StageIntro2,
	StageIntro3,
	StageIntro4,
	StageIntro5,
	StageCoutdown,
	StageEating,
	StageTurn,
	StageTurn2,
	StageBathroomZoom,
	StageRacingSetup,
	StageRacingSetup2,
	StageCountdown2,
	StageRacing,
	StageRacingResolution,
	StageGameOver
}
public class GameController : MonoBehaviour {

	public Plate plate;
	public PlayerController player1;
	public PlayerController player2;
	public HostController host;

	public GameStage CurrentStage {get; private set;}

	public string EatingWinnerName {get; private set;}
	public string EatingLoserName {get; private set;}

	public string RaceLoserName {get; private set;}

	private float raceWithThreshold = 12.0f;

	// Use this for initialization
	void Start () {
		StartStage(GameStage.StageStart);
	}
	
	// Update is called once per frame
	void Update () {
		if(CurrentStage == GameStage.StageEating)
		{
			if(!plate.HasFood())
			{
				EatingWinnerName = player1.FoodCounter > player2.FoodCounter ? "player 1" : "player 2";
				EatingLoserName = player1.FoodCounter > player2.FoodCounter ? "Player 2" : "Player 1";
				StartStage(GameStage.StageEating + 1);
			}
		}
		else if(CurrentStage == GameStage.StageRacing)
		{
			//Resolve race
			float player1Dist = Vector3.Distance(player1.transform.position,player1.BodyTarget.transform.position);
			float player2Dist = Vector3.Distance(player2.transform.position,player2.BodyTarget.transform.position);

			if(player1Dist <= raceWithThreshold)
			{
				RaceLoserName = "Player 2";
				StartStage(CurrentStage+1);
			}
			else if(player2Dist <= raceWithThreshold)
			{
				RaceLoserName = "Player 1";
				StartStage(CurrentStage+1);
			}
		}
		//TODO other non space to continue stages
		else
		{
			if(Input.GetKeyDown("space"))
			{
				StartStage(CurrentStage+1);
			}
		}
	}

	void StartStage(GameStage newStage)
	{
		Debug.Log("Start stage "+newStage);
		CurrentStage = newStage;
		host.PerformActionForStage();
		switch(newStage)
		{
		case GameStage.StageIntro1:
		case GameStage.StageIntro2:
		case GameStage.StageIntro3:
		case GameStage.StageIntro4:
		case GameStage.StageIntro5:
		case GameStage.StageEating:
		case GameStage.StageTurn:
		case GameStage.StageTurn2:
		case GameStage.StageRacingSetup:
		case GameStage.StageRacingSetup2:
		case GameStage.StageRacingResolution:
			break;
		case GameStage.StageGameOver:
			//Show credits
			break;
		case GameStage.StageRacing:
			player1.UpdateSpeedForFatness();
			player2.UpdateSpeedForFatness();
			break;
		case GameStage.StageBathroomZoom:
			player1.PerformStageAction();
			player2.PerformStageAction();
//			StartStage(newStage+1);
			break;
		default:
			StartStage(newStage+1);
			return;
		}
	}
}
