namespace DataGathering

open System.Text
open System.Net
open FSharp.Data


type msdnSchema = JsonProvider<"./SampleJson/TechNet.json">
 

module Retriever =
    let rec private CollectTechNetSites(baseURI: string, relativeRootPage: string, title: string, parentID: int) = 
            let downloader = new WebClient()
            let toc = downloader.DownloadString(baseURI + relativeRootPage)
            let technetData = msdnSchema.Parse(toc)
            for datum in technetData do
                //store
                let myParent = -1
                if datum.ExtendedAttributes.DataTochassubtree then
                    CollectTechNetSites(baseURI, datum.Href + "?toc=1", datum.Title, myParent)

                    
    let CollectTechNetSite(baseURI: string, relativeRootPage: string, title: string) = 
        //store
        let myParent = -1
        CollectTechNetSites(baseURI, relativeRootPage, title, myParent)
    
    