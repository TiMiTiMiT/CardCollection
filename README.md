# CardCollection

## Description
Card and Deck collektion (currently for Magic the Gathering) to keep an overview of cards in ones collection and the currently build decks

My goal is to build a working prototype first and then go over the code again and clean up the code. 

## Todos
- add Errorhandling
- Add Unit Tests when first real prototype works

## Featuer list for after cleanup
- Get Collection details 
	- like amount of cards
- Import decklist into collection for faster import
	- like write a txt file similar to the decklist and import them all into the collection
- When exporting/ importiung the collection, think about how to handle the "InUse" property
- Finish Settings when a bit more functionality
	- Set a save path and create the SaveFiel structure there
- Add Format support
	- Add Format to Decks
	- Add Format legality to cards and check when adding a deck
	- Validify the format when adding a deck
- When removing a card check that the quantity is now still >= InUse

## Bugs
- Adventure cards ar not added to collection