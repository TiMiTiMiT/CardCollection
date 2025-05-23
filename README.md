# CardCollection

## Description
Card and Deck collektion (currently for Magic the Gathering) to keep an overview of cards in ones collection and the currently build decks

My goal is to build a working prototype first and then go over the code again and clean up the code. 

## Todos
- add interaction with deck collection
- add Errorhandling
- Fix File paths to relativce paths
- Add Unit Tests when first real prototype works

## Featuer list for after cleanup
- Get Collection details 
	- like amount of cards
- Import decklist into collection for faster import
	- like write a txt file similar to the decklist and import them all into the collection
- When exporting/ importiung the collection, think about how to handle the "InUse" property

## Savefiles

### Collection
- Exporting your collection will create a .txt file here with the data from your collection
- You can also import a collection using a savefile in this directory

### Decklist
- Create a decklist.txt to import it into your deck collection
- The file will consist of (example below):
	- Deck
	- Amount Cardname
	- Sideboard (Optional)
	- Amount Cardname

```
Deck
1 Duress
3 Mountains
...

Sideboard
4 Thoughtsize
```

### Settings
- File containing settings
	- Current game
		- if multiple games are supported, save the game your currently managing	