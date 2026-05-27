module App.Types

type MainMenu =
| NuevoJuego
| CargarJuego
| Salir

type PauseMenu =
| Continuar
| Guardar
| Salir

type GameOverMenu =
| NuevoJuego
| Salir