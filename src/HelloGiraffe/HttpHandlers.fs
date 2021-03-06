namespace HelloGiraffe

module HttpHandlers =

    open Microsoft.AspNetCore.Http
    open Giraffe
    open HelloGiraffe.Models

    let handleGetHello =
        fun (next : HttpFunc) (ctx : HttpContext) ->
            task {
                let response = {
                    Text = "Hello world, from HelloGiraffe!"
                }
                return! json response next ctx
            }