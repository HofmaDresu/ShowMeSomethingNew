namespace DataGathering

open System.Net
open FSharp.Data


type msdnSchema = JsonProvider<"./SampleJson/TechNet.json">
 


type TechNet() = 
    let downloader = new WebClient()
    let toc = downloader.DownloadString("http://technet.microsoft.com/en-us/library/bb510741.aspx?toc=1")
    let technet = 
        msdnSchema.Parse(toc)
        |> Seq.filter (fun node -> node.ExtendedAttributes.DataTochassubtree)
