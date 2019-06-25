using System;
using System.Collections.Generic;
using System.IO;

namespace TopoSort
{
    internal class Program
    {
        public class Item
        {
            public string Main { get; set; }
            public string Dependency { get; set; }
        }

        private static string sortedJobs;

        private static void Main(string[] args)
        {
            #region Tasks Summary
            /*L -> 
                 * 1: Get all jobs which have no dependency (list<string> A)
                 * 2: Get all jobs which have dependency (list<Item> B) (class Item has 2 properties: Main and Dependency) 
                 * 2: Fore each job 'j' in list A - foreach(var j in listA), then use method : "CheckDependency(listB, j)"
                 * 
                 * 3: Method : CheckDependency(listB, j)                                               | ListB: B=>C             ListA: A=>
                 *    Loop list B in a foreach, => foreach(Item jd in listB)                           |        C=>F                    F=>
                 *    if (j == jd.Dependency)                                                          |        D=>A 
                 *    => output += jd.Dependency                                                       |        E=>B 
                 *    => assign: j = jd.Main                                                           
                 *    => Remove current 'jd' in listB, and continue check until (j != jd.Dependency) => break loop and continue to next value of listA;
                 *    
                 * 4: After accomplishing foreach listA, if listB.Count > 0
                 *    _ loop list B - foreach(var job in listB)
                 *    _ and check CircularDependency(job,listB);
                 *    foreach(Item jd in listB)
                 *      if(job.Dependency== jd.Main)                                                   | ListB: B=>C          param job:  B=>C
                 *             temOutput += jd.Dependency                                              |        C=>F
                 *             temOutput += job.Dependency + job.Main                                  |        F=>B
                 *              assign:  job = current 'jd', continue Loop                             |
                 *                  if(tempOut.Contain(jd.Dependency))                                 
                 *                     throw : "jobs can’t have circular dependencies.";
                 * 
                 *
                 *  Case 5: F=>C=>B=>E
                 *          A=>D
                 *          
                 *  Case 7: B=>C=>F=>B
                 *          D=>A
                 *          E=>
                 * **/

            #endregion

            //string[] content = File.ReadAllLines(Directory.GetCurrentDirectory() + "\\case1.txt");
            //string[] content = File.ReadAllLines(Directory.GetCurrentDirectory() + "\\case2.txt");
            //string[] content = File.ReadAllLines(Directory.GetCurrentDirectory() + "\\case3.txt");
            //string[] content = File.ReadAllLines(Directory.GetCurrentDirectory() + "\\case4.txt");
            //string[] content = File.ReadAllLines(Directory.GetCurrentDirectory() + "\\case5.txt");
            //string[] content = File.ReadAllLines(Directory.GetCurrentDirectory() + "\\case6.txt");
            string[] content = File.ReadAllLines(Directory.GetCurrentDirectory() + "\\case7.txt");

            List<string> JobsNoDependency = new List<string>();
            List<Item> JobsWithDependency = new List<Item>();
            sortedJobs = string.Empty;

            #region Read data from file and sort it to separate lists
            foreach (var line in content)
            {

                var jobsArrays = line.Split(new[] { "=>" }, StringSplitOptions.RemoveEmptyEntries);
                if (jobsArrays.Length == 2)
                {
                    JobsWithDependency.Add(new Item()// Add job with its dependency to a list B
                    {
                        Main = jobsArrays[0],
                        Dependency = jobsArrays[1]
                    });
                }
                else
                {
                    JobsNoDependency.Add(jobsArrays[0]);// Add single job to a list A
                }

            }
            #endregion

            #region Check dependency
            foreach (var j in JobsNoDependency)
            {
                CheckDependency(j, JobsWithDependency);//Method check dependency
            }
            #endregion

            #region Check CircularDependency
            if (JobsWithDependency.Count > 0)
            {
                foreach (var jd in JobsWithDependency)
                {
                    if (jd.Main == jd.Dependency)// if current job itself is dependency (C=>C) => throw error
                    {
                        sortedJobs = " jobs can’t depend on themselves.";
                        break;
                    }
                    else
                    {
                        var isCurcularDependency = CheckCircularDependency(jd, JobsWithDependency); // Check circular dependency
                        if (isCurcularDependency)
                        {
                            sortedJobs = "jobs can’t have circular dependencies.";
                            break;
                        }
                    }
                }
            } 
            #endregion

            Console.WriteLine(sortedJobs);// Write ouput
            Console.ReadLine();
        }

        private static bool CheckCircularDependency(Item job, List<Item> jobsWithDependency)
        {
            var tempOutput = string.Empty; // tempOutput contains jobs                     Ex: job: B=>C           jd:C=>F 
            foreach (var jd in jobsWithDependency)
            {
                if (job.Dependency == jd.Main)
                {
                    if (tempOutput.Contains(jd.Dependency))// if(tempOuput contains a dependency and if this one is a dependency of another) => throw error
                    {
                        return true;
                    }
                    tempOutput += jd.Dependency;
                    tempOutput = job.Dependency + job.Main; // temOuput like : F=>C=>B
                    job = jd;                               // if dependency in 'job' == jd.Main => assign : job = jd
                }
            }
            return false;
        }

        private static void CheckDependency(string j, List<Item> jobsWithDependency)
        {
            var lineToRemove = new Item(); // remove job which is added to sorted job list
            var tempValue = string.Empty; // assign this variable to singjob 'j'


            bool flag = false; // true if job is dependency
            foreach (var jd in jobsWithDependency)
            {
                if (j == jd.Dependency)// => if 'j' is a dependency of another job
                {
                    lineToRemove = jd; // assign value to remove this item from listB, because this job is added to sortedJob, no needed to check second time
                    j = jd.Main;// assign value of param 'j' to the current item.Main in listB
                    sortedJobs += jd.Dependency;
                    flag = true;
                    break;
                }
            }

            if (flag)
            {
                jobsWithDependency.Remove(lineToRemove);// remove line which is added to list job Sorted
                CheckDependency(j, jobsWithDependency);// check parent of this job
            }

            if (!sortedJobs.Contains(j))// added to sortedJob if this job isn't a dependency in listB
            {
                sortedJobs += j;
            }
        }
    }
}