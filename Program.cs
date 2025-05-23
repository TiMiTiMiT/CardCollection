using CardCollection.Classes;
using static CardCollection.Classes.GlobalVariables;
using CardCollection.Classes.Models.Magic;
using CardCollection.Classes.Data;
using System.IO;
using System.Xml.Linq;

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
            HandleSettingsCollection();
            break;
        case "1":
            await HandleCardCollection();
            break;
        case "2":
            HandleDeckCollection();
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
static void HandleSettingsCollection()
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
        bool CardExists = await GlobalVariables.MTGService.DoesCardExistInCollection(CardName);

        if (CardExists)
        {
            await GlobalVariables.MTGService.IncreaseQuantityByName(CardName, Int32.Parse(Amount));
        }
        else
        {
            await GlobalVariables.MTGService.AddMTGCard(CardName, Int32.Parse(Amount));
        }
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
    waitForInputMessage("Make sure a txt file to import your collection from is in the collection folder. For more information check the documentation.");
    await GlobalVariables.MTGService.ImportCardCollection();
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
static void HandleDeckCollection()
{
    Console.WriteLine("Managing your deck is currently Work in progress");

    Console.WriteLine("> Type 'exit' if you want to go back");
    string? input = Console.ReadLine();

    switch (input)
    {
        case "exit":
            goto EndOfLoop;
        default:
            waitForInputMessage("Comand not found!");
            break;
    }

    EndOfLoop:;
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