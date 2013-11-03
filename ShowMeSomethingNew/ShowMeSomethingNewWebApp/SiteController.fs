namespace FsWeb.Controllers

open System.Web
open System.Web.Mvc
open System.Net.Http
open System.Web.Http
open DataGathering.Retriever



type SiteController() =
    inherit ApiController()

    member x.Put(baseURI:string, relativeRoot:string, title:string) =
        CollectTechNetSite(baseURI, relativeRoot, title)