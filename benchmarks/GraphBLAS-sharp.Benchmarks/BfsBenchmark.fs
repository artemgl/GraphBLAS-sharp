namespace GraphBLAS.FSharp.Benchmarks

open GraphBLAS.FSharp
open GraphBLAS.FSharp.Algorithms
open BenchmarkDotNet.Attributes
open BenchmarkDotNet.Configs
open BenchmarkDotNet.Columns
open System.IO
open System

[<Config(typeof<Config>)>]
[<SimpleJob(targetCount=10)>]
type BfsBenchmark() =
    let random = Random()

    let mutable matrix = Unchecked.defaultof<Matrix<bool>>
    let mutable source = 0

    [<ParamsSource("GraphPaths")>]
    member val PathToGraph = "" with get, set

    [<GlobalSetup>]
    member this.BuildMatrix () =
        matrix <- Matrix.Build<bool> this.PathToGraph
        source <- random.Next matrix.RowCount

    [<Benchmark>]
    member this.LevelBFS () =
        levelBFS matrix source

    static member GraphPaths = seq {
        yield! Directory.EnumerateFiles(Path.Join [|"Datasets"; "1"|], "*.mtx")
    }
