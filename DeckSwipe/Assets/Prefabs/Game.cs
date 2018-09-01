﻿using UnityEngine;

public class Game : MonoBehaviour {
	
	public InputDispatcher InputDispatcher;
	public CardBehaviour CardPrefab;
	public Vector3 SpawnPosition;
	public Sprite DefaultCharacterSprite;
	
	private static readonly CharacterModel gameOverCharacter = new CharacterModel("", null);
	
	private CardStorage cardStorage;
	
	private void Awake() {
		// Listen for Escape key ('Back' on Android) to quit the game
		InputDispatcher.AddKeyDownHandler(KeyCode.Escape, keyCode => Application.Quit());
		cardStorage = new CardStorage(DefaultCharacterSprite);
	}
	
	private void Start() {
		cardStorage.CallbackWhenCardsAvailable(StartGame);
	}
	
	public void DrawNextCard() {
		if (Stats.Heat == 0) {
			SpawnCard(new CardModel("The city runs out of heat and freezes over.", "", "",
					gameOverCharacter,
					new GameOverCardOutcome(),
					new GameOverCardOutcome()));
		}
		else if (Stats.Food == 0) {
			SpawnCard(new CardModel("Hunger consumes the city, as food reserves deplete.", "", "",
					gameOverCharacter,
					new GameOverCardOutcome(),
					new GameOverCardOutcome()));
		}
		else if (Stats.Hope == 0) {
			SpawnCard(new CardModel("All hope among the people is lost.", "", "",
					gameOverCharacter,
					new GameOverCardOutcome(),
					new GameOverCardOutcome()));
		}
		else if (Stats.Materials == 0) {
			SpawnCard(new CardModel("The city runs out of materials to sustain itself.", "", "",
					gameOverCharacter,
					new GameOverCardOutcome(),
					new GameOverCardOutcome()));
		}
		else {
			SpawnCard(cardStorage.Random());
		}
	}
	
	public void RestartGame() {
		Stats.ResetStats();
		GameStartOverlay.StartSequence();
	}
	
	private void StartGame() {
		GameStartOverlay.FadeOutCallback = DrawNextCard;
		GameStartOverlay.StartSequence(false);
	}
	
	private void SpawnCard(CardModel card) {
		CardBehaviour cardInstance = Instantiate(CardPrefab, SpawnPosition, Quaternion.Euler(0.0f, -180.0f, 0.0f));
		cardInstance.Card = card;
		cardInstance.SnapPosition.y = SpawnPosition.y;
		cardInstance.Controller = this;
	}
	
}
