using CardCollection.Classes;
using static CardCollection.Classes.GlobalVariables;
using CardCollection.Classes.Models.Magic;
using CardCollection.Classes.Data;
using System.IO;
using System.Xml.Linq;
using System.Text.RegularExpressions;

while (true)
{
    clearConsole();
    Console.WriteLine($"Current Game: {Settings["current_game"]}");
    Console.WriteLine("> Type '0' to change settings");
    Console.WriteLine("> Type '1' to interact with your card collection");
    Console.WriteLine("> Type '2' to interact with your deck collection");
    Console.WriteLine("> Type 'quit' to stop");

    string? input = Console.ReadLine();

    switch (input)
    {
        case "0":
            HandleSettings();
            break;
        case "1":
            await HandleCardCollection();
            break;
        case "2":
            await HandleDeckCollection();
            break;
        case "quit":
            return;
        default:
            waitForInputMessage("Comand not found!");
            break;
    }
}

/*
--------------------------------------------------------------------------------------------------------------------------
Settings
--------------------------------------------------------------------------------------------------------------------------
*/
static void HandleSettings()
{
    while (true)
    {
        clearConsole();
        Console.WriteLine("These are your current settings:");
        foreach (string SettingKeys in Settings.Keys)
        {
            Console.WriteLine($"- {SettingKeys}: {Settings[SettingKeys]}");
        }
        Console.WriteLine("\n");

        Console.WriteLine("> Type '1' if you want to change your settings");
        Console.WriteLine("> Type 'exit' to go back");
        string? input = Console.ReadLine();

        if (input == "1")
        {
            Console.WriteLine("Write the setting you want to change:");
            string? SettingName = Console.ReadLine();

            if (SettingName != null && Settings.ContainsKey(SettingName))
            {
                switch (SettingName)
                {
                    case "current_game":
                        ChangeCurrentGame();
                        break;
                    case "exit":
                        goto EndOfLoop;
                    default:
                        waitForInputMessage("Setting not found!");
                        break;
                }
            }
        }
        else if (input == "exit")
        {
            goto EndOfLoop;
        }
        else
        {
            waitForInputMessage("Setting not found!");
        }
    }
    EndOfLoop:;
}

static void ChangeCurrentGame()
{
    clearConsole();
    Console.WriteLine("These are the current supported games");
    foreach (string Game in GamesAvailiable)
    {
        Console.WriteLine($"- {Game}");
    }
    Console.WriteLine("\n");

    Console.WriteLine("Write the game you want to change to:");
    string? GameName = Console.ReadLine();

    if( GameName != null && GamesAvailiable.Contains(GameName))
    {
        Settings["current_game"] = GameName;
        UpdateSettings();
    }
}

/*
--------------------------------------------------------------------------------------------------------------------------
CARD COLLECTION
--------------------------------------------------------------------------------------------------------------------------
*/
static async Task HandleCardCollection()
{
    while (true)
    {
        clearConsole();
        Console.WriteLine("> Type '1' if you want to add a card to your collection");
        Console.WriteLine("> Type '2' if you want to remove a card from your collection");
        Console.WriteLine("> Type '3' if you want to change the amount of copies you own of a card");
        Console.WriteLine("> Type '4' if you want to import your collection from a txt file");
        Console.WriteLine("> Type '5' if you want to export yout collection into a txt file");
        Console.WriteLine("> Type 'exit' if you want to go back");
        string? input = Console.ReadLine();

        switch (input)
        {
            case "1":
                await AddCardToCollection();
                break;
            case "2":
                await RemoveCardFromCollection();
                break;
            case "3":
                await ChangeQuantity();
                break;
            case "4":
                await ImportCollection();
                break;
            case "5":
                await ExportCollection();
                break;
            case "exit":
                goto EndOfLoop;
            default:
                waitForInputMessage("Comand not found!");
                break;
        }
    }
EndOfLoop:;
}

static async Task AddCardToCollection()
{
    Console.WriteLine("> What is the name of the card you want to add?");
    string? CardName = Console.ReadLine();

    Console.WriteLine("> What is the amount of copys you own?");
    string? Amount = Console.ReadLine();

    if (CardName != null && Amount != null)
    {
        await GlobalVariables.MTGService.AddMTGCard(CardName, Int32.Parse(Amount));
    }
}

static async Task RemoveCardFromCollection()
{
    Console.WriteLine("> What is the name of the card you want to remove?");
    string? CardName = Console.ReadLine();

    if (CardName != null)
    {
        await GlobalVariables.MTGService.RemoveMTGCardByName(CardName);
    }
}

static async Task ChangeQuantity()
{
    Console.WriteLine("> What is the name of the card you want to change the quantity of?");
    string? CardName = Console.ReadLine();

    Console.WriteLine("> do you want to increase or decrease the amount? (i/d)");
    string? Mode = Console.ReadLine();

    Console.WriteLine("> By what value do you want to increase/decrease the amount?");
    string? Amount = Console.ReadLine();

    if (CardName != null && Mode != null && Amount != null)
    {
        if (Mode == "i")
        {
            await GlobalVariables.MTGService.IncreaseQuantityByName(CardName, Int32.Parse(Amount));
        }
        else if (Mode == "d")
        {
            await GlobalVariables.MTGService.DecreaseQuantityByName(CardName, Int32.Parse(Amount));
        }    
    }
}

static async Task ImportCollection() 
{
    Console.WriteLine("> Make sure a txt file to import your collection from is in the collection folder.\n> THIS WILL DELETE ALL CURRENT CARDS FROM YOUR COLLECTION!\n> For more information check the github wiki.");
    Console.WriteLine("> Do you want to continue? (y/n)");
    string? input = Console.ReadLine();

    if(input == "y")
    {
        Console.WriteLine("Importing deck, plese wait...");
        await GlobalVariables.MTGService.ImportCardCollection();
    }
}

static async Task ExportCollection() 
{
    waitForInputMessage("A txt file with the export will be created in the collections folder.");
    await GlobalVariables.MTGService.ExportCardCollection();
}


/*
--------------------------------------------------------------------------------------------------------------------------
DECK COLLECTION
--------------------------------------------------------------------------------------------------------------------------
*/
static async Task HandleDeckCollection()
{
    while (true)
    {
        clearConsole();
        Console.WriteLine("> Press '1' if you want to import a deck to your collection");
        Console.WriteLine("> Press '2' if you want to delete a deck from your collection");
        Console.WriteLine("> Type 'exit' if you want to go back");

        string? input = Console.ReadLine();

        switch (input)
        {
            case "1":
                await ImportDeck();
                break;
            case "2":
                await DeleteDeck();
                break;
            case "exit":
                goto EndOfLoop;
            default:
                waitForInputMessage("Comand not found!");
                break;
        }
    }
    EndOfLoop:;
}

static async Task ImportDeck() 
{
    Console.WriteLine("> Create a txt file in the Decklist folder and enter the name if the file. For more informationcheck the github wiki");
    Console.WriteLine("> Enter the Deck name");
    String? DeckName = Console.ReadLine();

    if(DeckName != null)
    {
        MTGDeck Deck = ReadDecklist(DeckName);

        Console.WriteLine("> What is the vaild format of the deck?");
        String? Format = Console.ReadLine();
        Deck.Format = Format;


        if (Format != null && Format.ToLower() == "commander")
        {
            Console.WriteLine("> Who is the commadner of the deck");
            String? Commander = Console.ReadLine();
            Deck.Commander = Commander;
        }

        await GlobalVariables.MTGService.AddDeck(Deck);
    }

    
}

static MTGDeck ReadDecklist(string DeckName)
{
    Dictionary<string, int> Decklist = new Dictionary<string, int>();
    Dictionary<string, int> Sideboard = new Dictionary<string, int>();
    bool isMainDeck = false;
    bool isSideboard = false;

    try
    {
        StreamReader StreamReader = new StreamReader($"C:\\Users\\Tim\\source\\repos\\CardCollection\\Savefiles\\Decklists\\{DeckName}.txt");
        String? line;

        // for more information about the format og the decklist check the github wiki
        while ((line = StreamReader.ReadLine()) != null)
        {
            if (line == "Deck")
            {
                isMainDeck = true;
                continue;
            }else if (line == "Sideboard")
            {
                isSideboard = true;
                continue;
            }else if (line == "")
            {
                isMainDeck = false;
                isSideboard = false;
                continue;
            } else if (isMainDeck)
            {
                (string? cardName, int amount) = FormatLineForDictionary(line);
                Decklist[cardName] = amount;
                continue;
            }
            else if (isSideboard)
            {
                (string? cardName, int amount) = FormatLineForDictionary(line);
                Sideboard[cardName] = amount;
            }
        }

        StreamReader.Close();
    }
    catch (Exception e)
    {
        Console.WriteLine(e.Message);
    }

    // create deck object and return
    MTGDeck deck = new(
        DeckName,
        Decklist,
        "",
        Sideboard.Keys.Count > 0 ? Sideboard : null
    );

    return deck;
}
static (string?, int) FormatLineForDictionary(string line)
{
    Match match = Regex.Match(line, @"^(\S+)\s+(.+)$");

    if (match.Success)
    {
        int amount = Int32.Parse(match.Groups[1].Value); // "1"
        string cardName = match.Groups[2].Value; // "bla bla"

        return (cardName, amount);
    }

    return (null, 0);
}

static async Task DeleteDeck()
{
    Console.WriteLine("> Enter the name of teh deck you want to delete");
    String? DeckName = Console.ReadLine();

    if (DeckName != null)
    {
        Console.WriteLine($"> Are you sure you want to delete {DeckName}? (y/n)");
        String? Confirmation = Console.ReadLine();

        if (Confirmation != null)
        {
            await GlobalVariables.MTGService.RemoveDeckByName(DeckName);
        }
    }

    
}

/*
--------------------------------------------------------------------------------------------------------------------------
Helper functions
--------------------------------------------------------------------------------------------------------------------------
*/

static void clearConsole()
{
    // This code is/was here because it was nececary to debug the code in vs code.
    // but it is not needed when coding in visual studio
    /*
    if (!System.Diagnostics.Debugger.IsAttached)
    {
        Console.Clear();
    }
    */
    Console.Clear();
}

static void waitForInputMessage(string message)
{
    Console.WriteLine(message);
    Console.WriteLine("Press any key to continue");
    string? _ = Console.ReadLine();
}