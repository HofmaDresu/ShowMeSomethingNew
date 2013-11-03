namespace FsWeb.Controllers

open System.Web
open System.Web.Mvc
open DataGathering.Retriever

[<HandleError>]
type HomeController() =
    inherit Controller()
    member this.Index () =
        this.View() :> ActionResult