#light
module TreeBuilder

type node = 
    {
        morse : string;
        children : node list
    }
    

//Memoization functions  
let memoCache = ref Map.empty

let genKey t (l:int) = 
    t ^ l.ToString()
    
let saveToMemo key data =
    memoCache := Map.add key data !memoCache
    data
    
let inMemo key = 
    Map.exists (fun k _ -> k = key) !memoCache

let retrieve key =
    Map.find key !memoCache
    
                
let rec builder (patterns:string list) (target:string) level =
    match level with
    | 10 -> [];
    | _  -> patterns
            |> List.filter (fun morse -> target.StartsWith(morse))
            |> List.map
                (fun morse ->
                    let newTarget = target.Substring(morse.Length)
                    let key = genKey newTarget (level + 1) in
                    {morse = morse;
                     children = if inMemo key then
                                    retrieve key
                                else
                                    builder
                                        patterns
                                        newTarget
                                        (level + 1)
                                    |> saveToMemo key})    