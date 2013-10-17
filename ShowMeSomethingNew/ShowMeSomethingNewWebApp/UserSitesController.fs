namespace FsWeb.Controllers

open System.Web
open System.Web.Mvc
open System.Net.Http
open System.Web.Http
open DataGathering


type UserSitesController() =
    inherit ApiController()

    // GET /api/values
    member x.Get(userName:string) = 
        let foo = query {
            for row in Retriever.db.TableOfContent do
            select (row.SiteName, row.SiteId)
            } 
        foo 
    // @"[{""SiteName"":""test1"",""myCompletion"":""80%"",""averageCompletion"":""30%""},{""SiteName"":""test2"",""myCompletion"":""10%"",""averageCompletion"":""70%""}]"
    // GET /api/values/5
    member x.Get (userName:string, parentSiteId:int) = "http://www.google.com"
