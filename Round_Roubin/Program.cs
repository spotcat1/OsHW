using System;
using System.Collections.Generic;

class RoundRobin
{
    #region reading the Input
    static void Main(string[] args)
    {
        Console.WriteLine("Enter Number of Processes:");
        int numProcesses = int.Parse(Console.ReadLine());

        List<string> processes = new List<string>();
        List<int> arrivalTime = new List<int>();
        List<int> serviceTime = new List<int>();

        for (int i = 0; i < numProcesses; i++)
        {
            processes.Add($"P#{i + 1}");
            Console.Write($"{processes[i]} Arrival Time: ");
            arrivalTime.Add(int.Parse(Console.ReadLine()));
            Console.Write($"{processes[i]} Service Time: ");
            serviceTime.Add(int.Parse(Console.ReadLine()));
        }

        Console.Write("Quantum Slice: ");
        int quantum = int.Parse(Console.ReadLine());
        Console.Write("Context Switch: ");
        int contextSwitch = int.Parse(Console.ReadLine());

        RoundRobinAlgorithm(processes, arrivalTime, serviceTime, quantum, contextSwitch);
    }
    #endregion

    static void RoundRobinAlgorithm(List<string> processes, List<int> arrivalTime, List<int> serviceTime, int quantum, int contextSwitch)
    {
        int NUmberOfProcesses = processes.Count;
        List<int> remainingTime = new List<int>(serviceTime);
        int time = 0;
        int currentProcess = -1;  // To keep track of the current process
        List<Tuple<int, int, string>> queue = new List<Tuple<int, int, string>>();

        while (remainingTime.Exists(t => t > 0))
        {
            for (int i = 0; i < NUmberOfProcesses; i++)
            {
                if (remainingTime[i] > 0 && arrivalTime[i] <= time)
                {
                    if (i != currentProcess)
                    {
                        // Change in process, add context switch
                        if (currentProcess != -1)
                        {
                            time += contextSwitch;
                            queue.Add(new Tuple<int, int, string>(time - contextSwitch, time, "C.S"));
                        }
                        currentProcess = i;
                    }

                    int startTime = time;
                    if (remainingTime[i] > quantum)
                    {
                        time += quantum;
                        remainingTime[i] -= quantum;
                    }
                    else
                    {
                        time += remainingTime[i];
                        remainingTime[i] = 0;
                    }

                    int endTime = time;
                    queue.Add(new Tuple<int, int, string>(startTime, endTime, processes[i]));
                }
            }
        }

        // Add context switch after the last process
        if (currentProcess != -1 && remainingTime.Exists(t => t > 0))
        {
            time += contextSwitch;
            queue.Add(new Tuple<int, int, string>(time - contextSwitch, time, "C.S"));
        }

        Console.WriteLine("Output:");
        foreach (var interval in queue)
        {
            Console.WriteLine($"{interval.Item1}-{interval.Item2} {interval.Item3}");
        }
    }
}
