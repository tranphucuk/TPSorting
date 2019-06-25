    A=>                 F=>C=>B=>E
    B=>C                A=>D
    C=>F   =========>   A=>
    D=>A                F=>
    E=>B
    F=>
    
    
    1: Get all jobs which have no dependency (list<string> A)
    2: Get all jobs which have dependency (list<Item> B) (class Item has 2 properties: Main and Dependency) 
    2: Fore each job 'j' in list A - foreach(var j in listA), then use method : "CheckDependency(listB, j)"
    4: After accomplishing foreach listA, if listB.Count > 0 
        => check CircularDependency(job,listB);
        
        
![alt text](https://imgur.com/YssynWK.png)
