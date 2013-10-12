namespace FsWeb.Controllers

open System.Web
open System.Web.Mvc
open DataGathering.Retriever

[<HandleError>]
type HomeController() =
    inherit Controller()
    member this.Index () =
        this.View() :> ActionResult

    member this.CallTechNet() =
        let sw = new System.Diagnostics.Stopwatch()
        sw.Start()
        CollectTechNetSite("http://technet.microsoft.com", "/en-us/library/bb510741(v=sql.105).aspx?toc=1", "SQL Server 2008 R2 Transact SQL")
        sw.Stop()
        this.ViewData.Add("duration", sw.ElapsedMilliseconds)
        this.View() :> ActionResult