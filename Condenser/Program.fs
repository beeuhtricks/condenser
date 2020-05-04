// Learn more about F# at http://fsharp.org

open System
open Microsoft.CodeAnalysis;
open Microsoft.CodeAnalysis.CSharp

let programText =
    @"using System;
    using System.Collections.Generic;
    using System.Text;

    namespace HelloWorld
    {
        class Program
        {
            static void Main(string[] args)
            {
                Console.WriteLine(""Hello, World!"");
            }
        }
    }"

let tree = CSharpSyntaxTree.ParseText programText
let root = tree.GetCompilationUnitRoot ()
let compilation =
    CSharpCompilation
        .Create("HelloWorld")
        .AddReferences(MetadataReference.CreateFromFile typeof<string>.Assembly.Location)
        .AddSyntaxTrees tree
let model = compilation.GetSemanticModel (tree, false)
let usingSystem = root.Usings.[0]
let systemName = usingSystem.Name
let nameInfo = model.GetSymbolInfo systemName

[<EntryPoint>]
let main argv =
    let systemSymbol = nameInfo.Symbol :?> INamespaceSymbol
    for ns in systemSymbol.GetNamespaceMembers () do
        Console.WriteLine ns
    0
