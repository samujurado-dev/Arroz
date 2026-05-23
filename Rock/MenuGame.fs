module Generic.MenuGame

open Generic.Types

let mostrar() =
    Menu.mostrar
        10
        15
        [|
            NewRockSim, "a"
            NewMonsterSim, "b"
            NewSaludo, "c"
            Exit, "d"
        |]