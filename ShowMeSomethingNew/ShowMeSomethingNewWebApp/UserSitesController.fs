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
        (newPage.SiteName, newPage.URL, newPage.SiteId)

    member x.Put (userName:string, siteId:int64) =
        let userId = query {
            for u in Retriever.db.User do
            where (u.UserName = userName)
            select u.UserId
            exactlyOne
        }
        let newVisit = new dbSchema.ServiceTypes.UserVisits(UserId = userId, SiteId = siteId)
        Retriever.db.UserVisits.InsertOnSubmit(newVisit)
        Retriever.db.DataContext.SubmitChanges()
