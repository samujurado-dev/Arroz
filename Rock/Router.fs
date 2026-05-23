module Generic.Router

open Generic.Types

type RouterState =
| ShowingMenu
| ShowingRock
| ShowingMonster
| ShowingSaludo
| Terminated

let initialState = ShowingMenu

let rec mainLoop state =
    match state with 
    | ShowingMenu -> 
        // ¡IMPORTANTE!: Debe ser MenuGame.mostrar()
        let comando = MenuGame.mostrar() 
        match comando with 
        | NewRockSim -> mainLoop ShowingRock
        | NewMonsterSim -> mainLoop ShowingMonster
        | NewSaludo -> mainLoop ShowingSaludo
        | Exit -> mainLoop Terminated
        
    | ShowingRock -> 
        Rock.mostrar()
        mainLoop ShowingMenu

    | ShowingMonster ->
        Monster.mostrar()
        mainLoop ShowingMenu

    | ShowingSaludo ->
        Saludo.mostrar()
        mainLoop ShowingMenu

    | Terminated -> 
        () 

let mostrar() =
    initialState |> mainLoop