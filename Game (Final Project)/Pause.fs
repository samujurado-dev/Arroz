module App.Pause

open Types

let mostrar x y =
    GenericMenu.mostrar x y
        [|
            Continuar,"Continue"
            Guardar, "Save Game"
            PauseMenu.Salir, "Exit"
        |]
