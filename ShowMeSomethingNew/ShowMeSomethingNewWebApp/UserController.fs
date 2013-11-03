namespace FsWeb.Controllers

open System.Web
open System.Web.Mvc
open System.Net.Http
open System.Web.Http
open DataGathering

type UserController() =
    inherit ApiController()

    member x.Get() =
        if (HttpContext.Current.User.Identity = null) then ""
        else HttpContext.Current.User.Identity.Name

    member x.Put(userName:string) =
        let newUser = new dbSchema.ServiceTypes.User(UserName = userName)
        Retriever.db.User.InsertOnSubmit(newUser)
        Retriever.db.DataContext.SubmitChanges()