#light
module Helpers

//Wrapper around Map.exists
let inMap key map = 
    Map.exists (fun k _ -> k = key) map
    
    
let listOfKeys map =
    Map.fold
        (fun k _ acc -> k::acc)
        map
        []
    |> List.rev
    
        
//Function to count occurrences of substring    
let rec uoloop sub (target:string) index count = 
    let new_index = target.IndexOf((sub:string), (index:int))
    if (new_index = -1) then 
        count 
    else 
        uoloop sub target (new_index + 1) (count + 1)

//Friendly wrapper around above
let unique subString targetString = 
    uoloop subString targetString 0 0;;   
    
    
let devnull x =
    () //Functions evaluated by do need to return ()