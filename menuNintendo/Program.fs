open System
open System.Threading

type ProgramState =
| Running
| Terminated

type State = {
    ProgramState: ProgramState
    redrawScreen: bool
    ContinueX: int
    ContinueY: int
    newGameX: int
    newGameY: int
    ExitX: int
    ExitY: int
    CursorX: int
    CursorY: int
}

let initialState = {
    ProgramState = Running
    redrawScreen = true
    ContinueX = Console.BufferWidth/2 - 1
    ContinueY = Console.BufferHeight/2 - 1
    newGameX = Console.BufferWidth/2 - 1
    newGameY = Console.BufferHeight/2 - 2
    ExitX = Console.BufferWidth/2 - 1
    ExitY = Console.BufferHeight/2
    CursorX = Console.BufferWidth/2 - 3
    CursorY = Console.BufferHeight/2 - 2
}

let lookForEscape key state:State =
    if key = ConsoleKey.Enter && state.CursorY = (Console.BufferHeight/2) then
        {state with ProgramState = Terminated}
    else
        state

let processCursorKeys key state =
    match key with
    | ConsoleKey.UpArrow -> Some {state with CursorY = max (Console.BufferHeight/2 - 2) (state.CursorY - 1)}
    | ConsoleKey.DownArrow -> Some {state with CursorY = min (Console.BufferHeight/2) (state.CursorY + 1)}
    | _ -> None
    |> Option.map ( fun s -> {s with redrawScreen = true})
    |> Option.defaultValue state
let processKeyboard state =
    if Console.KeyAvailable then
        let k = Console.ReadKey true
        state
        |> lookForEscape k.Key
        |> processCursorKeys k.Key
    else
        state

let displayMessage x y color (msg:string) =
    Console.SetCursorPosition(x,y)
    Console.ForegroundColor <- color
    msg |> Console.Write

let displayMenu state =
    displayMessage state.ContinueX state.ContinueY ConsoleColor.White "Continue"
    displayMessage state.newGameX state.newGameY ConsoleColor.White "New Game"
    displayMessage state.ExitX state.ExitY ConsoleColor.White "Exit"
    displayMessage state.CursorX state.CursorY ConsoleColor.White "*"
    state

let redrawScreen state =
    if state.redrawScreen then
        Console.Clear()
        state
        |> displayMenu
        |> fun s -> {s with redrawScreen=false}
    else
        state

let oldBackground = Console.BackgroundColor
let oldForeground = Console.ForegroundColor

Console.CursorVisible <- false

Console.Clear()

let rec mainLoop state =
    let newState =
        state
        |> processKeyboard
        |> redrawScreen
    match newState.ProgramState with
    | Running ->
        Thread.Sleep 25
        mainLoop newState
    | Terminated -> ()

initialState
|> mainLoop

Console.CursorVisible <- true
Console.ForegroundColor <- oldForeground
Console.BackgroundColor <- oldBackground
Console.Clear()