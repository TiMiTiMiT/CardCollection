while (true)
{
    clearConsole();
    Console.WriteLine("> Type '1' to interact with your card collection");
    Console.WriteLine("> Type '2' to interact with your deck collection");
    Console.WriteLine("> Type 'quit' to stop");

    string? input = Console.ReadLine();

    switch (input)
    {
        case "1":
            HandleCardCollection();
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
CARD COLLECTION
--------------------------------------------------------------------------------------------------------------------------
*/
static void HandleCardCollection()
{
    Console.WriteLine("Managing your card collection is currently Work in progress");

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

// i added the function because my code crashed in the debugger if i clear the console
static void clearConsole()
{
    if (!System.Diagnostics.Debugger.IsAttached)
    {
        Console.Clear();
    }
}

static void waitForInputMessage(string message)
{
    Console.WriteLine(message);
    Console.WriteLine("Press any key to continue");
    string? _ = Console.ReadLine();
}