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
        let userProgress = query {
            for up in Retriever.db.UserProgress do
            where (up.UserName = userName)
            select up
        }
        userProgress
    // @"[{""SiteName"":""test1"",""myCompletion"":""80%"",""averageCompletion"":""30%""},{""SiteName"":""test2"",""myCompletion"":""10%"",""averageCompletion"":""70%""}]"
    // GET /api/values/5
    member x.Get (userName:string, parentSiteId:int) =
        let newPageCount = query {
            for unvisitedSite in Retriever.db.UnvisitedSitesForUser do
            where (unvisitedSite.UserName = userName)
            select unvisitedSite
            count
        } 
        let newPage = query {
            for unvisitedSite in Retriever.db.UnvisitedSitesForUser do
            where (unvisitedSite.UserName = userName)
            skip (System.Random().Next(0, newPageCount))
            headOrDefault
        } 
        let newVisit = new dbSchema.ServiceTypes.UserVisits(UserId = newPage.UserId, SiteId = newPage.SiteId)
        Retriever.db.UserVisits.InsertOnSubmit(newVisit);
        Retriever.db.DataContext.SubmitChanges();
        newPage.SiteName
