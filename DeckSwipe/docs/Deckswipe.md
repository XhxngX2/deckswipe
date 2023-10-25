# Deckswipe



```mermaid
sequenceDiagram

participant G as Game
participant CS as CardStorage
participant PS as progress storage
participant SO as GameStartOverlay


note over G,SO: Awake

activate G

G->>CS: new
activate CS



G->>PS: new with card storage instance
deactivate CS
activate PS


G->>SO: set Class static FadeOutCallback = StartGameplayLoop
note over G : Start

activate G

G-->>PS:await progress storage Task ProgressStorageInit
PS->>PS:await Task Load
activate PS


PS->>PS:await Task LoadLocally
activate PS
deactivate PS # task LoadLocally

PS->>PS:await Task cardStorage.CardCollectionImport
activate PS
deactivate PS # task cardStorage.CardCollectionImport


deactivate PS # task Load


alt Progress == null

PS->>PS: progress = new GameProgress

else

PS->>PS: attachReference(cardStorage)
PS->>PS: cardStorage.ResolvePrerequisites()

end

PS->>G : with initialized GameProgress
G->>SO: StartSequence(progressStorage.Progress.daysPassed, daysLastRun);

SO->>G:call back (StartGameplayLoop) 
G->>G:StartGameplayLoop

deactivate G
deactivate PS # new with card storage instance

deactivate G
```

