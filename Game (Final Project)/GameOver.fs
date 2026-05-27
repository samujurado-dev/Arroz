module App.GameOver

open Types

open Utils

open System

let mostrar x y =
    GenericMenu.mostrar x y
        [|
            NuevoJuego,"New Game"
            Salir, "Exit"
        |]
