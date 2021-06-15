#light
module PrepMorse
open System.IO
open Helpers

let target = ".-.-...-..-.-.------.--.....-...--.---.-.------...--------.-..---.--...-.---.-..--.-.-.....-.---.-..-----.-.--.-....-..-........."
let amkey = "A .-  B -... C -.-.  D -..  E .  F ..-.  G --.  H ....  I ..  J .---  K -.-  L .-..  M -- N -.  O ---  P .--.  Q --.-  R .-.  S ... T -  U ..-  V ...-  W .--  X -..-  Y -.--  Z --.."


//Map alphabet to morse equivalent
let createMapping (amkey:string) = 
    let alphabet, morseCode = List.partition 
                                (fun s -> not (String.contains s '.' or String.contains s '-'))
                                [for s in amkey.Split [|' '|] when s <> "" -> s]
    List.fold_left2
        (fun map alpha morse -> Map.add alpha.[0] morse map) //String.map_concat requires a char, hence alpha.[0]
        Map.empty
        alphabet 
        morseCode
    
    
//Create Morse translation of word
let convert mapping word = 
    String.map_concat 
        (fun character -> mapping.[character])
        word

                                            
let storeRoman morse word map =
    if inMap morse map then 
        word::map.[morse] 
    else
        [word]         
                
                
let makePatternMap wordsPath (target:string) mapping =
    File.ReadAllLines(wordsPath)
    |> Seq.filter   (fun word -> target.IndexOf(convert mapping word) <> -1)
    |> Seq.map      (fun word -> (convert mapping word, word))
    |> Seq.fold     (fun map (morse, word) -> Map.add morse (storeRoman morse word map) map) Map.empty


let getPatterns path = 
    createMapping amkey
    |> makePatternMap path target 