# TPSorting

     A=>                            F=>C=>B=>E
     B=>C                           A=>D
     C=>F   =================>      A=>
     D=>A                           F=>
     E=>B
     F=> 
     
     
     1: Get all jobs which have no dependency (list<string> A)
     2: Get all jobs which have dependency (list<Item> B) (class Item has 2 properties: Main and Dependency) 
     2: Fore each job 'j' in list A - foreach(var j in listA), then use method : "CheckDependency(listB, j)"
     
     3: Method : CheckDependency(listB, j)                                               | ListB: B=>C             ListA: A=>
        Loop list B in a foreach, => foreach(Item jd in listB)                           |        C=>F                    F=>
        if (j == jd.Dependency)                                                          |        D=>A 
        => output += jd.Dependency                                                       |        E=>B 
        => assign: j = jd.Main                                                           
        => Remove current 'jd' in listB, and continue check until (j != jd.Dependency) => break loop and continue to next value of listA;
        
     4: After accomplishing foreach listA, if listB.Count > 0
        _ loop list B - foreach(var job in listB)
        _ and check CircularDependency(job,listB);
        foreach(Item jd in listB)
          if(job.Dependency== jd.Main)                                                   | ListB: B=>C          param job:  B=>C
                 temOutput += jd.Dependency                                              |        C=>F
                 temOutput += job.Dependency + job.Main                                  |        F=>B
                  assign:  job = current 'jd', continue Loop                             |
                      if(tempOut.Contain(jd.Dependency))                                 
                         throw : "jobs can’t have circular dependencies.";

