using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class IslandController : MonoBehaviour
{
	[Header("References")]
    public TitleScreen TitleScreen;
	public ExtinctionScreen EndScreen;
	public CarrotsManager Carrots;
	public RabbitsManager Rabbits;
	public GameObject Hand;

	public List<GameObject> IslandsPool;

	private void Start()
	{
		foreach(GameObject island in IslandsPool)
		{
			island.SetActive(false);
		}

		int islandIndex = UnityEngine.Random.Range(0, IslandsPool.Count);
		IslandsPool[islandIndex].SetActive(true);

		TitleScreen.onStart += OnGameState;
		EndScreen.onEnd += OnGameEnd;
	}

	private void OnGameEnd(object sender, EventArgs e)
	{
		Hand.SetActive(false);
	}

	private void OnGameState(object sender, EventArgs e)
	{
		Rabbits.Init(Rabbits.InitialRabbitsCount, Rabbits.InitialRabbitsRadius);
		Carrots.Init(Carrots.InitialCarrotsCount, Carrots.InitialCarrotsRadius);
		Hand.SetActive(true);
	}
}
