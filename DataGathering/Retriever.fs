namespace DataGathering

open System.Text
open System.Net
open System
open System.Data
open System.Data.Linq
open FSharp.Data
open Microsoft.FSharp.Data.TypeProviders
open Microsoft.FSharp.Linq


type msdnSchema = JsonProvider<"./SampleJson/TechNet.json">
type dbSchema = SqlDataConnection<"Data Source=HD-ULTRA\SQLEXPRESS2012;Initial Catalog=ShowMeSomethingNew;Integrated Security=SSPI;">


module Retriever =
    let db = dbSchema.GetDataContext() 

    let private StoreTOCSite(siteName: string, url: string, parentID: Nullable<int64>) =
        let newRecord = new dbSchema.ServiceTypes.TableOfContent(SiteName = siteName,
                                                                URL = url,
                                                                TopSiteId = parentID)
        db.TableOfContent.InsertOnSubmit(newRecord)
        db.DataContext.SubmitChanges()
        let getOne = query {
            for row in db.TableOfContent do
            where (row.SiteName.Equals(siteName))
            select row
            } 
        (Seq.exactlyOne getOne).SiteId

    let rec private CollectTechNetSites(baseURI: string, relativeRootPage: string, title: string, parentID: Nullable<int64>) = 
            let downloader = new WebClient()
            let toc = downloader.DownloadString(baseURI + relativeRootPage)
            let technetData = msdnSchema.Parse(toc)
            for datum in technetData do
                StoreTOCSite(datum.Title, baseURI + datum.Href, parentID) |> ignore
                if datum.ExtendedAttributes.DataTochassubtree then
                    CollectTechNetSites(baseURI, datum.Href + "?toc=1", datum.Title, parentID)

                    
    let CollectTechNetSite(baseURI: string, relativeRootPage: string, title: string) = 
        let myParent = StoreTOCSite(title, baseURI + relativeRootPage, System.Nullable())
        CollectTechNetSites(baseURI, relativeRootPage, title, System.Nullable(myParent))
    
    