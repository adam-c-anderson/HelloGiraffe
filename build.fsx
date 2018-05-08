#r "paket:
nuget Fake.Core.Target
nuget Fake.DotNet.Cli
nuget Fake.IO.FileSystem
 //"
#load "./.fake/build.fsx/intellisense.fsx"
// include Fake modules, see Fake modules section

open Fake.Core
open Fake.Core.TargetOperators
open Fake.DotNet
open Fake.IO.Globbing.Operators

let allProjs =
  !! "src/*/"
  ++ "tests/*/"

let pubProj = "src/HelloGiraffe"

// *** Define Targets ***
Target.create "Clean" (fun _ ->
  Trace.log " --- Cleaning stuff --- "
  for proj in allProjs do
    DotNet.exec id "clean" proj |> ignore
)

Target.create "Build" (fun _ ->
  Trace.log " --- Building the app --- "
  for proj in allProjs do
    DotNet.exec id "build" proj |> ignore
)

Target.create "Test" (fun _ ->
  Trace.log " --- Running tests --- "
  let testProjs = !! "tests/*/"
  for testProj in testProjs do
    DotNet.exec id "test" testProj |> ignore
)

Target.create "Publish" (fun _ ->
  Trace.log " --- Publishing app --- "
  let args = sprintf "%s --configuration Release" pubProj
  DotNet.exec id "publish" args |> ignore
)

// *** Define Dependencies ***
"Clean"
  ==> "Build"
  ==> "Test"
  ==> "Publish"

// *** Start Build ***
Target.runOrDefault "Publish"
