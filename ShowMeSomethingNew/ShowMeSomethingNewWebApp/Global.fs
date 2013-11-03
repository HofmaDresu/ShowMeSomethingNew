namespace FsWeb

open System
open System.Web
open System.Web.Mvc
open System.Web.Routing
open System.Web.Http
open System.Data.Entity
open System.Web.Optimization


type Route = { controller : string
               action : string
               id : UrlParameter }

type ApiRoute = { userName : String
                  parentSiteId : RouteParameter }

type Global() =
    inherit System.Web.HttpApplication() 

    static member RegisterRoutes(routes:RouteCollection) =
        routes.IgnoreRoute("{resource}.axd/{*pathInfo}")
        routes.MapHttpRoute( "DefaultApi", "api/{controller}/{userName}/{parentSiteId}", 
            { userName = "thisisnotarealusername"
              parentSiteId = RouteParameter.Optional } ) |> ignore
        routes.MapRoute("Default", 
                        "{controller}/{action}/{id}", 
                        { controller = "Home"; action = "Index"
                          id = UrlParameter.Optional } )

    member this.Start() =
        AreaRegistration.RegisterAllAreas()
        Global.RegisterRoutes(RouteTable.Routes)
