module App.Router

open Types

open Utils

open App.AlienGame

open System

open System.Threading

open System.IO
open System.Text.Json
open System.Text.Json.Serialization
let opcionesJson = 
    let options = JsonSerializerOptions()
    options.Converters.Add(JsonFSharpConverter())
    options

type GameState =
| ShowingMainMenu
| ShowingGame
| ShowingPause
| ShowingGameOver
| End

type State = {
    MainState: GameState
    EstadoAlien: App.AlienGame.State option
}

let estadoInicial = {
        MainState = ShowingMainMenu
        EstadoAlien = None
}

let rutaGuardado = @"C:\Users\Samu\Documents\alien_save.json"

let mostrarMenuPrincipal state =
    mostrarMensaje 10 15 ConsoleColor.DarkRed "Alien Attack! 👽"
    match MenuGame.mostrar 10 15 with
    | MainMenu.NuevoJuego -> 
        { state with MainState = ShowingGame; EstadoAlien = None }
    | CargarJuego ->
        if File.Exists(rutaGuardado) then
            let json = File.ReadAllText(rutaGuardado)
            let LoadGame = JsonSerializer.Deserialize<App.AlienGame.State>(json, opcionesJson)
            { state with MainState = ShowingGame; EstadoAlien = Some LoadGame }
        else
            { state with MainState = ShowingMainMenu }
    | MainMenu.Salir -> { state with MainState = End }

let mostrarPausa state =
    mostrarMensaje 10 15 ConsoleColor.DarkRed $"Pause"
    match Pause.mostrar 10 15 with
    | Continuar -> { state with MainState = ShowingGame }
    | Guardar ->
        match state.EstadoAlien with
        | Some estadoAlien ->
            let json = JsonSerializer.Serialize(estadoAlien, opcionesJson)
            File.WriteAllText(rutaGuardado, json)
        | None -> ()
        { state with MainState = ShowingGame }
    | PauseMenu.Salir -> { state with MainState = ShowingMainMenu }

let rec mostrarJuego state =
    let FinalState = App.AlienGame.mostrar state.EstadoAlien    
    if FinalState.ProgramState = Terminated then 
        { state with MainState = ShowingGameOver; EstadoAlien = None }
    else
        { state with MainState = ShowingPause; EstadoAlien = Some FinalState }

let mostrarPerdida state =
    mostrarMensaje 10 13 ConsoleColor.DarkRed $"Game Over! ☠️"
    match GameOver.mostrar 10 15 with
    | NuevoJuego -> {state with MainState = ShowingGame}
    | Salir -> {state with MainState = End}

let mainFunctions state =
    match state.MainState with
    | ShowingGame -> mostrarJuego state
    | ShowingMainMenu -> mostrarMenuPrincipal state
    | ShowingPause -> mostrarPausa state
    | ShowingGameOver -> mostrarPerdida state
    | _ -> state

let rec mainLoop state =
    state
    |> mainFunctions
    |> fun nuevoEstado ->
        if nuevoEstado.MainState <> End then
            Thread.Sleep 25
            nuevoEstado |> mainLoop

let mostrar() =
        Console.Clear()
        Console.CursorVisible <- false
        
        estadoInicial
        |> mainLoop

        Console.Clear()
        Console.CursorVisible <- true