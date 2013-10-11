namespace DataGathering

open System.Text
open System.Net
open FSharp.Data


type msdnSchema = JsonProvider<"./SampleJson/TechNet.json">
 

module Retriever =
    let rec CollectTechNetSites(baseURI: string, relativeRootPage: string, title: string, hasParent: bool) = 
            let downloader = new WebClient()
            let toc = downloader.DownloadString(baseURI + relativeRootPage)
            let technetData = msdnSchema.Parse(toc)
            for datum in technetData do
                //store
                if datum.ExtendedAttributes.DataTochassubtree then
                    CollectTechNetSites(baseURI, datum.Href, datum.Title, true)
//        |> Seq.filter (fun node -> node.ExtendedAttributes.DataTochassubtree)
    
    