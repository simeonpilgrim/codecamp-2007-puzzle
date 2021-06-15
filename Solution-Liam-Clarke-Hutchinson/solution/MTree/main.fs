#light
module Main

open System
open System.Collections.Generic
open System.Windows.Forms
open System.IO
open List
open PrepMorse
open Helpers
open TreeBuilder
   
                                   
//tree search functions                                                                    
let rec search node acc (results:List<string list>) level =  
    match node.children with
    | [] -> results.Add(List.rev acc);
    | _  -> node.children
            |> List.iter (fun node -> search node (node.morse::acc) results (level + 1))                    

let doSearch resultStore tree =
    List.iter (fun node -> search node [node.morse] resultStore 0) tree


//String permutation functions
//Note to self, do not generate cartesian product of list with 13 ^ 10 possible permutations ever again
let cartesian ls =
    let nth = List.nth ls
    [for a in (nth 0)
     for b in (nth 1)
     for c in (nth 2)
     for d in (nth 3)
     for e in (nth 4)
     for f in (nth 5)
     for g in (nth 6)
     for h in (nth 7)
     for i in (nth 8)
     for j in (nth 9) -> [a;b;c;d;e;f;g;h;i;j]]
     
    
let genPermutations patternMap result =
        result
        |> List.map (fun morse -> Map.find morse patternMap)
        |> cartesian //Some morse patterns match two/three words, so generate all possible strings matching morse pattern


//Actual 'meat'
let matchingBranches patternMap =
    let resultStore = new List<string list>()
    builder (listOfKeys patternMap) target 0 |> doSearch resultStore
    resultStore              
               
let mainFlow path =
    let patternMap = getPatterns path
    matchingBranches patternMap
        |> Seq.filter   (fun item -> (length item) = 10 && (String.concat "" item) = target) 
        |> Seq.map      (fun item -> genPermutations patternMap item)
        |> List.flatten
        |> List.map     (fun item -> String.concat " " item)
        |> List.filter  (fun item -> (unique "T" item = 5) && (unique "E" item = 4))
        |> String.concat "\n\n(or)\n\n"
        |> (fun (s:string) -> MessageBox.Show(s))


    
let main = 
    let args = Environment.GetCommandLineArgs()
    if Array.length args <> 2 then
        MessageBox.Show("Incorrect number of arguments") 
    else if not (File.Exists args.[1]) then
        MessageBox.Show("File " ^ args.[1] ^ " not found")
    else
        mainFlow args.[1]
        

do main |> devnull
    