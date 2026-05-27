module App.MenuGame

open Types

let mostrar x y =
    GenericMenu.mostrar x y
        [|
            MainMenu.NuevoJuego,"New Game"
            CargarJuego, "Load Game"
            MainMenu.Salir, "Exit"
        |]
