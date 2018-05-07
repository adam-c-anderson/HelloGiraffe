#r "paket:
nuget Fake.Core.Target
nuget Fake.DotNet.Cli
nuget Fake.IO.FileSystem
 //"
#load "./.fake/build.fsx/intellisense.fsx"
// include Fake modules, see Fake modules section

open Fake.Core
open Fake.DotNet
open Fake.IO.Globbing.Operators

let allProjects =
  !! "src/*/"
  ++ "tests/*/"

// *** Define Targets ***
Target.create "Clean" (fun _ ->
  Trace.log " --- Cleaning stuff --- "
  for proj in allProjects do
    DotNet.exec id "clean" proj |> ignore
)

Target.create "Build" (fun _ ->
  Trace.log " --- Building the app --- "
  for proj in allProjects do
    DotNet.exec id "build" proj |> ignore
)

Target.create "Test" (fun _ ->
  Trace.log " --- Running tests --- "
  DotNet.exec id "test" "tests/HelloGiraffe.Tests" |> ignore
)

Target.create "Publish" (fun _ ->
  Trace.log " --- Publishing app --- "
  DotNet.exec id "publish" "src/HelloGiraffe" |> ignore
)

open Fake.Core.TargetOperators

// *** Define Dependencies ***
"Clean"
  ==> "Build"
  ==> "Test"
  ==> "Publish"

// *** Start Build ***
Target.runOrDefault "Publish"
