using System;
using System.Collections.Generic;
using System.IO;
using Excel;
using Microsoft.VisualBasic.FileIO;

namespace Branch_Benchmark
{//21/4/20 turned this into a maximisation solver
    static class Global
    {//houses variables that should apply globally to this program
        public static double upper_bound = int.MinValue;
        public static double[] best_solution;//the current best integer solution to the problem
        public static int count_depth = 0;//a counter for depth which can be used to access the current added constraint
        public static int depth_limit = 40;//the maximum depth allowance per decision variable. Very rule of thumb right now. *research this topic
        public static bool nonnegative_decisions = true;//if there is the assumption that all decisions variables must be non-negative
        public static double bigM = 1000000;
        public static string test_location = "C:/Users/n9951911/Documents/Code_Backup/Branch_Benchmark/Branch_Benchmark/TestProblems/ExcelInput.xlsx";
        public static int check = 0;
        public static int lbranch_depth = 0;//how deep the local branching is
        //public static int lbranch_limit = 5;//maximum sequential depth of local branching
        public static int lbranch_active = 0;//1 or 0 T/F. Is used in multiplication with an int
        public static bool incum_exist = false;//a check for whether an incumbent solution has been found
        public static bool lbranch_local_active = false;//a check for whether the local section of the local branching method is currently being explored
        public static int best_depth = int.MaxValue;
        public static double tolerance = 0.000001;//tolerance for what counts as an int
        public static bool cost_heuristic_enabled = false;
        public static double cost_improv_percent = 10;
        public static bool lbranch_incumb = false;

        public static int k_multi=2;

        public static int instance_count = 0;
        public static System.Diagnostics.Stopwatch watch2 = new System.Diagnostics.Stopwatch();
    }
    class Program
    {//a branch and bound algorithm for solving an IP problem
        static void Main(string[] args)
        {
            //Console.WriteLine("Hello World!");
            //want to input number of decision variables, then initialise objective function and constraints from that

            //consider double[][]
            //double[,] constraints = Init_Constraint(num_decisions,num_constraints);//new double [num_decisions+2,num_constraints+depth_limit*num_decisions];
            //current data structure is just a 2D array with an allowance for depth. Could use list but that is potentially quite inefficient. Naive approach for now
            //consider dynamic arrays. Could be useful and is a much better alternative to lists
            //I could also consider initialising each branch a new local version that was the previous local with +1 constraint or something
            //list is reasonable here, consider tree (insertion into tree is log(n))
            //double[] decisions = Init_Decisions(num_decisions);//should be of size num_decisions +1 for the constant. Might revise even including a constant in objective\

            //run_base();
            //run_excel();
            //run_text();

            //run this again with run_random edited for different constraint numbers and 
            Global.k_multi = 2;
            //local branching and cost heuristic disabled
            //run_random(@"C:/Users/n9951911/Documents/Code_cleaned/Branch_Benchmark/Branch_Benchmark/TestProblems/BaseMIP_Record.txt", @"C:/Users/n9951911/Documents/Code_cleaned/Branch_Benchmark/Branch_Benchmark/TestProblems/BaseMIP_Record2.txt");
            
            run_random(@"C:/Users/Aidan/Desktop/QUT/MXN404/Sem2_CodeUpdates/Code_cleaned/Branch_Benchmark/Branch_Benchmark/TestProblems/BaseMIP_Record.txt", @"C:/Users/Aidan/Desktop/QUT/MXN404/Sem2_CodeUpdates/Code_cleaned/Branch_Benchmark/Branch_Benchmark/TestProblems/BaseMIP_Record2.txt");

            Global.lbranch_active = 1;//turn on local branching
            run_random(@"C:/Users/Aidan/Desktop/QUT/MXN404/Sem2_CodeUpdates/Code_cleaned/Branch_Benchmark/Branch_Benchmark/TestProblems/lbranchMIP_Record.txt", @"C:/Users/Aidan/Desktop/QUT/MXN404/Sem2_CodeUpdates/Code_cleaned/Branch_Benchmark/Branch_Benchmark/TestProblems/lbranchMIP_Record2.txt");
            //heuristic on 10% improvement, local off
            Global.lbranch_active = 0;
            Global.cost_heuristic_enabled = true;
            run_random(@"C:/Users/Aidan/Desktop/QUT/MXN404/Sem2_CodeUpdates/Code_cleaned/Branch_Benchmark/Branch_Benchmark/TestProblems/CostMIP10_Record.txt", @"C:/Users/Aidan/Desktop/QUT/MXN404/Sem2_CodeUpdates/Code_cleaned/Branch_Benchmark/Branch_Benchmark/TestProblems/CostMIP10_Record2.txt");
            //heuristic on 1% improvement, local off
            Global.lbranch_active = 0;
            Global.cost_heuristic_enabled = true;
            Global.cost_improv_percent = 1;
            run_random(@"C:/Users/Aidan/Desktop/QUT/MXN404/Sem2_CodeUpdates/Code_cleaned/Branch_Benchmark/Branch_Benchmark/TestProblems/CostMIP1_Record.txt", @"C:/Users/Aidan/Desktop/QUT/MXN404/Sem2_CodeUpdates/Code_cleaned/Branch_Benchmark/Branch_Benchmark/TestProblems/CostMIP1_Record2.txt");
            //heuristic on 1% improvement, local on
            Global.lbranch_active = 1;
            Global.cost_heuristic_enabled = true;
            run_random(@"C:/Users/Aidan/Desktop/QUT/MXN404/Sem2_CodeUpdates/Code_cleaned/Branch_Benchmark/Branch_Benchmark/TestProblems/HybridMIP1_Record.txt", @"C:/Users/Aidan/Desktop/QUT/MXN404/Sem2_CodeUpdates/Code_cleaned/Branch_Benchmark/Branch_Benchmark/TestProblems/HybridMIP1_Record2.txt");
            //heuristic on 10% improvement, local on
            Global.cost_heuristic_enabled = true;
            Global.cost_improv_percent = 10;
            run_random(@"C:/Users/Aidan/Desktop/QUT/MXN404/Sem2_CodeUpdates/Code_cleaned/Branch_Benchmark/Branch_Benchmark/TestProblems/HybridMIP10_Record.txt", @"C:/Users/Aidan/Desktop/QUT/MXN404/Sem2_CodeUpdates/Code_cleaned/Branch_Benchmark/Branch_Benchmark/TestProblems/HybridMIP10_Record2.txt");

            Global.k_multi = 4;
            Global.cost_heuristic_enabled = false;
            Global.lbranch_active = 1;//turn on local branching
            run_random(@"C:/Users/Aidan/Desktop/QUT/MXN404/Sem2_CodeUpdates/Code_cleaned/Branch_Benchmark/Branch_Benchmark/TestProblems/lbranchMIPk4_Record.txt", @"C:/Users/Aidan/Desktop/QUT/MXN404/Sem2_CodeUpdates/Code_cleaned/Branch_Benchmark/Branch_Benchmark/TestProblems/lbranchMIPk4_Record2.txt");
            
            Global.cost_heuristic_enabled = true;
            Global.lbranch_active = 1;//turn on local branching
            Global.cost_improv_percent = 1;
            run_random(@"C:/Users/Aidan/Desktop/QUT/MXN404/Sem2_CodeUpdates/Code_cleaned/Branch_Benchmark/Branch_Benchmark/TestProblems/CostMIP1k4_Record.txt", @"C:/Users/Aidan/Desktop/QUT/MXN404/Sem2_CodeUpdates/Code_cleaned/Branch_Benchmark/Branch_Benchmark/TestProblems/CostMIP1k4_Record2.txt");
            
            Global.cost_heuristic_enabled = true;
            Global.lbranch_active = 1;//turn on local branching
            Global.cost_improv_percent = 10;
            run_random(@"C:/Users/Aidan/Desktop/QUT/MXN404/Sem2_CodeUpdates/Code_cleaned/Branch_Benchmark/Branch_Benchmark/TestProblems/CostMIP10k4_Record.txt", @"C:/Users/Aidan/Desktop/QUT/MXN404/Sem2_CodeUpdates/Code_cleaned/Branch_Benchmark/Branch_Benchmark/TestProblems/CostMIP10k4_Record2.txt");


            //will need to also try different local branching k parameters

            //run again with global variables to turn off/on local branching and relax/tighten heuristic
        }
        static void run_random(string text_location, string text_location_solution)
        {
            Random rng = new Random(42);//42 seed for regular testing, 43 for larger problems
            string[,] data_record = new string[10,6];
            string[,] data_solution = new string[10, 6];
            double[] decision_range = { 0, 50 };
            int[] constraint_range = { 0, 10, 100, 500 };

            int num_real = 5;
            int num_int = 5;
            int num_bin = 10;
            int num_decisions = num_real + num_int + num_bin;
            int num_constraints = 20;
            //run_sim(10, num_decisions, num_constraints, num_real, num_int, num_bin, rng, decision_range, constraint_range, data_record, data_solution, 0);

            num_real = 7;
            num_int = 8;
            num_bin = 15;
            num_decisions = num_real + num_int + num_bin;
            num_constraints = 30;
            //run_sim(10, num_decisions, num_constraints, num_real, num_int, num_bin, rng, decision_range, constraint_range, data_record, data_solution,1);

            num_real = 10;
            num_int = 10;
            num_bin = 20;
            num_decisions = num_real + num_int + num_bin;
            num_constraints = 40;
            //run_sim(10, num_decisions, num_constraints, num_real, num_int, num_bin, rng, decision_range, constraint_range, data_record, data_solution, 2);

            num_real = 12;
            num_int = 13;
            num_bin = 25;
            num_decisions = num_real + num_int + num_bin;
            num_constraints = 50;
            //run_sim(10, num_decisions, num_constraints, num_real, num_int, num_bin, rng, decision_range, constraint_range, data_record, data_solution, 3);

            num_real = 15;
            num_int = 15;
            num_bin = 30;
            num_decisions = num_real + num_int + num_bin;
            num_constraints = 60;
            //run_sim(10, num_decisions, num_constraints, num_real, num_int, num_bin, rng, decision_range, constraint_range, data_record, data_solution, 0);

            num_real = 0;
            num_int = 0;
            num_bin = 20;
            num_decisions = num_real + num_int + num_bin;
            num_constraints = 20;
            //run_sim(10, num_decisions, num_constraints, num_real, num_int, num_bin, rng, decision_range, constraint_range, data_record, data_solution, 1);

            num_real = 0;
            num_int = 0;
            num_bin = 30;
            num_decisions = num_real + num_int + num_bin;
            num_constraints = 30;
            //run_sim(10, num_decisions, num_constraints, num_real, num_int, num_bin, rng, decision_range, constraint_range, data_record, data_solution, 2);

            num_real = 0;
            num_int = 0;
            num_bin = 40;
            num_decisions = num_real + num_int + num_bin;
            num_constraints = 40;
            //run_sim(10, num_decisions, num_constraints, num_real, num_int, num_bin, rng, decision_range, constraint_range, data_record, data_solution, 3);

            num_real = 0;
            num_int = 0;
            num_bin = 50;
            num_decisions = num_real + num_int + num_bin;
            num_constraints = 50;
            //run_sim(10, num_decisions, num_constraints, num_real, num_int, num_bin, rng, decision_range, constraint_range, data_record, data_solution, 4);


            num_real = 50;
            num_int = 50;
            num_bin = 100;
            num_decisions = num_real + num_int + num_bin;
            num_constraints = 200;
            run_sim(10, num_decisions, num_constraints, num_real, num_int, num_bin, rng, decision_range, constraint_range, data_record, data_solution,0);

            //double[] decisions=Generate_Decision_Instance(num_decisions,rng,decision_range);
            //double[,] constraints = Generate_Constraint_Instance(num_decisions,num_real,num_int,num_bin,num_constraints,rng,constraint_range,decisions);
            //Init_Bin_Constraints(num_decisions, num_constraints, num_bin, constraints);
            //Branch(constraints, decisions, num_decisions, num_constraints, num_real, num_int, num_bin);
            //Console.WriteLine(constraints[1,1]);

            using (System.IO.StreamWriter file = new System.IO.StreamWriter(text_location, true))
                for (int i = 0; i < 2; i++)
                {
                    for (int j = 0; j < 10; j++)
                    {
                        file.Write(data_record[j, i]);
                    }
                }
            using (System.IO.StreamWriter file2 = new System.IO.StreamWriter(text_location_solution, true))
                for (int i = 0; i < 2; i++)
                {
                    for (int j = 0; j < 10; j++)
                    {
                        file2.Write(data_solution[j, i]);
                    }
                }
            //write data_record to text. i, j with each i loop initially inputing a label so that we can tell the data apart. or all different files
        }
        static void run_sim(int i_max,int num_decisions, int num_constraints, int num_real, int num_int, int num_bin,Random rng, double[] decision_range,int[] constraint_range,string[,] data_record,string[,] data_solution,int data_row)
        {
            for(int i = 0; i < i_max; i++)
            {
                Reset_Global(num_decisions);
                double[] decisions = Generate_Decision_Instance(num_decisions, rng, decision_range);
                double[,] constraints = Generate_Constraint_Instance(num_decisions, num_real, num_int, num_bin, num_constraints, rng, constraint_range, decisions);
                Init_Bin_Constraints(num_decisions, num_constraints, num_bin, constraints);
                Global.watch2.Start();
                //if (Global.instance_count++ == 236)//just the problematic instance
                Branch(constraints, decisions, num_decisions, num_constraints, num_real, num_int, num_bin);
                data_record[i, data_row] = $"{ Global.watch2.ElapsedMilliseconds}\n";
                data_solution[i, data_row] = $"{Global.best_solution[num_decisions]}\n";
                Global.watch2.Reset();
            }
        }
        static void Reset_Global(int num_decisions)
        {
            Global.best_solution = new double[num_decisions + 1];
            Global.best_depth = int.MaxValue;
            Global.count_depth = 0;
            Global.lbranch_depth = 0;
            Global.incum_exist = false;
            Global.check = 0;
            Global.upper_bound = int.MinValue;
            Global.lbranch_active = 0;
            Global.lbranch_local_active = false;
            Global.lbranch_incumb = false;
        }
        static void run_excel()
        {//the read from excel version
            int num_real = 0;
            int num_int = 10;
            int num_bin = 0;
            int num_decisions = num_real + num_int + num_bin;
            int num_constraints = 20;
            Global.best_solution = new double[num_decisions + 1];
            double[] alpha = ReadExcel1(Global.test_location);
            double[,] beta = ReadExcel2(Global.test_location);
            var watch = new System.Diagnostics.Stopwatch();
            watch.Start();

            Branch(beta, alpha, num_decisions, num_constraints, num_real, num_int, num_bin);
            //int kappa = 1;
            for (int dvar = 0; dvar < num_decisions + 1; dvar++)
            {
                Console.WriteLine(Global.best_solution[dvar]);
            }
            watch.Stop();
            Console.WriteLine($"Execution Time: {watch.ElapsedMilliseconds} ms");
        }
        static void run_text()
        {//the read from excel version
            int num_real = 5;
            int num_int = 5;
            int num_bin = 10;
            int num_decisions = num_real + num_int + num_bin;
            int num_constraints = 40;
            Global.best_solution = new double[num_decisions + 1];
            double[] decisions = new double[num_decisions+1];
            double[,] constraints = new double[num_constraints + num_bin + Global.depth_limit * num_decisions, num_decisions + 2];
            Init_Bin_Constraints(num_decisions, num_constraints, num_bin, constraints);
            Read_Text(decisions,constraints,num_decisions,num_constraints, "C:/Users/n9951911/Documents/Code_cleaned/Branch_Benchmark/Branch_Benchmark/TestProblems/Data_test2.txt");
            var watch = new System.Diagnostics.Stopwatch();
            Global.watch2.Start();
            watch.Start();

            Branch(constraints, decisions, num_decisions, num_constraints, num_real, num_int, num_bin);
            //int kappa = 1;
            for (int dvar = 0; dvar < num_decisions + 1; dvar++)
            {
                Console.WriteLine(Global.best_solution[dvar]);
            }
            watch.Stop();
            Global.watch2.Stop();
            Console.WriteLine($"Execution Time: {watch.ElapsedMilliseconds} ms");
        }
        static void run_base()
        {//in code test version
            int num_real = 0;
            int num_int = 20;
            int num_bin = 0;
            int num_decisions = num_real + num_int + num_bin;
            int num_constraints = 40;
            //alterations for
            //int m = 2; //num of machines
            //int n = 8; //num of jobs
            //num_constraints = m * n + n + m - 1;//change num contraints for flowshop
            //num_decisions = n * n + 2 * m * n;
            //num_real = 2 * m * n;
            //num_int = 0;
            //num_bin = n * n;
            Global.best_solution = new double[num_decisions + 1];
            //double[] decisions = Init_Decisions(num_decisions);
            //double[,] constraints = Init_Constraint(num_decisions, num_constraints, num_bin);
            double[,] constraints = Init_Constraint(num_decisions, num_constraints, num_bin);//Init_FlowConstraints(num_decisions, num_constraints,num_real,num_int, num_bin);
            double[] decisions = Init_Decisions(num_decisions);//Init_FlowDecisions(num_decisions,m,n);
            var watch = new System.Diagnostics.Stopwatch();
            watch.Start();

            Branch(constraints, decisions, num_decisions, num_constraints, num_real, num_int, num_bin);
            //int kappa = 1;
            for (int dvar = 0; dvar < num_decisions + 1; dvar++)
            {
                Console.WriteLine(Global.best_solution[dvar]);
            }
            watch.Stop();
            Console.WriteLine($"Execution Time: {watch.ElapsedMilliseconds} ms");
        }
        static double[] Generate_Decision_Instance(int num_decisions,Random rng, double[] decision_range)
        {//decision_range is a vector of size 2 containing the maximum and minimum
            double[] decisions = new double[num_decisions + 1];

            for(int i = 0; i < num_decisions; i++)
            {
                decisions[i] = rng.NextDouble() * (decision_range[1] - decision_range[0]) + decision_range[0];
            }
            return decisions;
        }
        static double[,] Generate_Constraint_Instance(int num_decisions, int num_real, int num_int, int num_bin, int num_constraints,Random rng, int[] constraint_range, double[] decisions)
        {
            double[,] constraints = new double[num_constraints + num_bin + Global.depth_limit * num_decisions, num_decisions + 2];
            double real_sum = 0;
            double int_sum = 0;
            double bin_sum = 0;
            double[] int_array = new double[num_int];
            double[] bin_array = new double[num_decisions];
            bool feasible = false;
            bool simplex = false;
            int intk = 0;
            int bink = 0;
            while (!simplex)
            {
                for (int row = 0; row < num_constraints; row++)
                {
                    feasible = false;
                    //now to do check for feasible constraints
                    while (!feasible)
                    {
                        Roll_Constraint(num_decisions, rng, constraint_range, constraints, row, num_int, num_bin, int_array, bin_array);
                        real_sum = 0;
                        int_sum = 0;
                        bin_sum = 0;
                        intk = 0;
                        bink = 0;
                        feasible = true;
                        for (int i = 0; i < num_real; i++)
                        {
                            real_sum += constraints[row,i];
                        }
                        for (int i = 0; i < num_int; i++)
                        {
                            int_sum += constraints[row,i + num_real];
                        }
                        while (intk < num_int && !(int_array[intk] == 0))
                        {
                            intk++;
                        }
                        while (bink < num_bin && !(bin_array[bink] == 0))
                        {
                            bink++;
                        }
                        double[] int_check = new double[intk];
                        double[] bin_check = new double[bink];
                        //binary greater than constraint feasible
                        if (real_sum == 0 && int_sum == 0 && constraints[row,num_decisions + 1] == 1)
                        {
                            for (int i = 0; i < num_bin; i++)
                            {
                                bin_sum += constraints[row,i + num_real + num_int];
                            }
                            if (bin_sum < constraints[row, num_decisions])
                            {
                                feasible = false;
                            }
                        }
                        else if (real_sum == 0 && int_sum == 0 && constraints[row, num_decisions + 1] == 0 && false)
                        {//binary equality feasibility test
                            bin_sum = constraints[row,num_decisions];

                            for (int i = 0; i <= bink; i++)
                            {//the purpose of this loop is to check all
                                for (int j = 0; j <= bink; j++)
                                {
                                    if (i == 0)
                                    {
                                        bin_check[j] = constraints[row,num_decisions];
                                    }
                                    bin_check[j] += -bin_array[(j + i) % bink];
                                }


                            }
                            //
                            if (!(bin_sum == 0))
                            {
                                feasible = false;
                            }
                        }
                        //if its an equality without real variables, is the RHS reachable?
                        else if (constraints[row, num_decisions + 1] == 0 && num_real == 0 && false)
                        {

                        }

                    }
                }
                double[] solution_local = MySimplex(decisions, constraints, num_constraints + num_bin + Global.count_depth, num_decisions + num_constraints + num_bin + Global.count_depth);
                if (solution_local != null && solution_local[num_decisions]>0)
                {
                    simplex = true;
                }
                else
                {
                    simplex = false;
                }
                //run simplex to check that its feasible. can use as the current solution if it works
            }
            return constraints;
        }
        static void Roll_Constraint(int num_decisions, Random rng, int[] constraint_range, double[,] constraints, int row,int num_int,int num_bin,double[] int_array, double[] bin_array)
        {//rolls the random values for a given contraint
            int alpha = 0;
            double[] inequality_list = { -1, 1 };
            for(int j = 0; j < num_decisions-num_int-num_bin; j++)
            {//real
                alpha = rng.Next(constraint_range[0], constraint_range[1]);
                constraints[row,j] = alpha;
                //order_array[j] = Math.Abs(alpha);
            }
            for (int j = 0; j < num_int; j++)
            {//int
                alpha = rng.Next(constraint_range[0], constraint_range[1]);
                constraints[row,j+num_decisions-num_int-num_bin] = alpha;
                int_array[j] = Math.Abs(alpha);
            }
            for (int j = 0; j < num_bin; j++)
            {//int
                alpha = rng.Next(constraint_range[0], constraint_range[1]);
                constraints[row,j + num_decisions - num_bin] = alpha;
                bin_array[j] = Math.Abs(alpha);
            }
            constraints[row,num_decisions] = rng.Next(constraint_range[2],constraint_range[3]);
            constraints[row,num_decisions + 1] = inequality_list[rng.Next(0, 1)];//convert to double?
            Array.Sort(int_array);
            Array.Sort(bin_array);
        }
        static void Init_Bin_Constraints(int num_decisions, int num_constraints, int num_bin, double[,] local_constraints)
        {//adds the binary constraints to the constraint list
            for (int i = 0; i < num_bin; i++)
            {
                local_constraints[num_constraints + i, num_decisions - num_bin + i] = 1;
                local_constraints[num_constraints + i, num_decisions] = 1;
                local_constraints[num_constraints + i, num_decisions + 1] = -1;
            }
            //return local_constraints;
        }
        static double[,] Init_Constraint(int num_decisions, int num_constraints, int num_bin)
        { double[,] local_constraints = new double[num_constraints + num_bin + Global.depth_limit * num_decisions, num_decisions + 2];
            //-1 is <=, 0 is =, 1 is >=
            //double[,] constraints_input = { {8,  1,   7,   5,   1,   9,   1,   7,   5,   5,   231, -1 },
            //{1,  2,   4,   6 ,  3 ,  3   ,3   ,0 ,  2  , 6  , 437, -1 },
            //{9,  7,   7,   4,   1,   0,   1,   3,   1,   4,   400, -1 },
            //{6,  2,   9,   5,   2,   2,   0,   6,   4,   7,   389, -1 },
            //{9,  6,   6,   9,   9,   4,   6,   2,   9,   3,   502, -1 },
            //{4,  7,   0,   4,   8,   6,   4,   1,   2,   9,   293, -1 },
            //{8,  5,   3,   4,   7,   8,   2,   6,   1,   1,   337, -1 },
            //{8,  8,   6,   2,   8,   1,   4,   7,   4,   0,   377, -1 },
            //{1,  4,   9,   0,   8,   9,   4,   2,   8,   1,   154, -1 },
            //{9,  2,   2,   1,   3,   6,   5,   2,   5,   9,   283, -1 },
            //{0,  7,   9,   9,   4,   2,   2,   2,   2,   8,   554, -1 },
            //{6,  3,   6,   0,   0,   4,   0,   2,   4,   9,   223, -1 },
            //{2,  5,   9,   8,   7,   7,   8,   2,   9,   4,   383, -1 },
            //{7,  8,   2,   5,   1,   9,   2,   4,   9,   4,   449, -1 },
            //{0,  0,   1,   0,   3,   0,   5,   4,   8,   1,   231, -1 },
            //{4,  7,   2,   9,   2,   2,   3,   6,   3,   3,   511, -1 },
            //{5,  6,   8,   8,   5,   0,   5,   3,   4,   9,   463, -1 },
            //{9,  2,   8,   0,   5,   2,   4,   2,   0,   0,   383, -1 },
            //{2,  5,   1,   2,   8,   4,   6,   4,   4,   6,   415, -1 },
            //{4,  0,   8,   1,   2,   2,   6,   9,   6,   4,   251, -1 } };

            //{ {1,1,1,1,1,2,0 },{0,0,0,1,1,1,0 }, { 0, 0, 1, 0, 1, 1, 0 }, { 0, 1, 0, 0, 1, 1, 0 }, 
            //{ 0, 0, 1, 1, 0, 1, 0 } };//{ { 4, 9, 26, -1 }, { 8, 5, 17, -1 } };
            double[,] constraints_input = { {8,  1,   7,   5,   1,   9,   1,   7,   5,   5,   3,   8,   1,   9,   4,   0,   2,   4,   0,   8,   231, -1
            },{1,   2,   4,   6,   3,   3,   3,   0,   2,   6,   0,   2,   9,   6,   2,   4,   3,   1,   0,   7,   437, -1
            },{9,   7,   7,   4,   1,   0,   1,   3,   1,   4,   7,   4,   3,   4,   7,   0,   6,   9,   6,   3,   400, -1
            },{6,   2,   9,   5,   2,   2,   0,   6,   4,   7,   9,   1,   8,   6,   6,   1,   4,   2,   8,   6,   389, -1
            },{9,   6,   6,   9,   9,   4,   6,   2,   9,   3,   7,   8,   6,   3,   6,   7,   7,   2,   1,   5,   502, -1
            },{4,   7,   0,   4,   8 ,  6 ,  4,   1,   2,   9,   5,   0,   9,   9,   5,   0 ,  1,   4,   4,   1,   293, -1
            },{8,   5 ,  3 ,  4 ,  7,   8 ,  2,   6 ,  1  , 1 ,  2 ,  7,   4,   9,   1,   9 ,  1,   2,   7 ,  7,   337, -1
            },{8,   8,   6 ,  2 ,  8,   1,   4,   7,   4,   0,   0,   3,   9,   4,   1,   4,   0,   1,   4,   8,   377, -1
            },{1,   4  , 9 ,  0  , 8 ,  9 ,  4 ,  2 ,  8 ,  1 ,  9,   6,   5,   6 ,  8,   8,   7 ,  6 ,  7 ,  9,   154, -1
            },{9 ,  2   ,2 ,  1 ,  3  , 6  , 5 , 2 ,  5 ,  9 ,  1 ,  3 ,  0,   6,   6  , 9 ,  6 ,  8 ,  4  , 3 ,  283, -1
            },{0 ,  7 ,  9   ,9  , 4 ,  2 ,  2  , 2   ,2  , 8,   6 ,  3  , 1  , 3 ,  2,   9 ,  3  , 6 ,  8  , 6 ,  554, -1
            },{6, 3, 6, 0, 0, 4, 0, 2, 4, 9, 1, 1, 2, 7, 2, 8, 9, 2, 6, 1, 223, -1
            },{2, 5, 9, 8, 7, 7, 8, 2, 9, 4, 0, 9, 5, 1, 7, 5, 3, 6, 5, 2, 383, -1
            },{7, 8, 2, 5, 1, 9, 2, 4, 9, 4, 6, 6, 4, 4, 1, 6, 1, 7, 9, 9, 449, -1
            },{0, 0, 1, 0, 3, 0, 5, 4, 8, 1, 3, 5, 5, 4, 8, 4, 9, 4, 5, 1, 231, -1
            },{4, 7, 2, 9, 2, 2, 3, 6, 3, 3, 3, 4, 4, 0, 4, 0, 0, 7, 1, 3, 511, -1
            },{5, 6, 8, 8, 5, 0, 5, 3, 4, 9, 1, 6, 7, 5, 8, 1, 6, 5, 6, 1, 463, -1
            },{9, 2, 8, 0, 5, 2, 4, 2, 0, 0, 3, 2, 3, 2, 0, 8, 1, 3, 8, 8, 383, -1
            },{2, 5, 1, 2, 8, 4, 6, 4, 4, 6, 1, 9, 3, 4, 6, 8, 4, 9, 1, 2, 415, -1
            },{4, 0, 8, 1, 2, 2, 6, 9, 6, 4, 7, 5, 4, 7, 4, 4, 2, 3, 8, 1, 251, -1
            },{8, 4, 4, 0, 0, 5, 2, 5, 6, 2, 4, 4, 5, 5, 8, 4, 3, 8, 5, 3, 226, -1
            },{2, 5, 9, 3, 8, 5, 7, 0, 7, 8, 5, 8, 7, 7, 5, 0, 0, 7, 2, 4, 543, -1
            },{3, 9, 5, 1, 7, 1, 8, 8, 2, 4, 6, 6, 3, 0, 9, 8, 9, 3, 5, 8, 320, -1
            },{1, 5, 7, 2, 5, 2, 6, 7, 5, 6, 8, 7, 4, 2, 0, 2, 0, 9, 6, 2, 238, -1
            },{3, 5, 5, 0, 2, 3, 5, 7, 5, 9, 8, 0, 6, 1, 9, 6, 6, 5, 9, 9, 103, -1
            },{6, 8, 6, 7, 5, 1, 2, 7, 4, 8, 5, 7, 4, 0, 8, 0, 1, 5, 6, 4, 487, -1
            },{7, 0, 1, 5, 4, 6, 0, 3, 9, 4, 4, 3, 0, 8, 5, 5, 3, 6, 7, 1, 123, -1
            },{3, 6, 9, 0, 7, 6, 5, 3, 6, 3, 1, 4, 6, 6, 7, 2, 5, 2, 1, 4, 280, -1
            },{1, 0, 5, 6, 0, 7, 5, 5, 4, 6, 4, 7, 9, 6, 4, 0, 6, 1, 4, 8, 285, -1
            },{0, 4, 5, 2, 4, 2, 4, 0, 5, 1, 9, 4, 5, 8, 3, 3, 8, 4, 9, 0, 496, -1
            },{0, 7, 6, 3, 7, 8, 7, 2, 8, 0, 0, 6, 1, 1, 6, 5, 2, 2, 7, 3, 509, -1
            },{7, 0, 0, 1, 9, 3, 3, 1, 5, 3, 5, 2, 6, 9, 5, 2, 3, 1, 9, 2, 496, -1
            },{1, 4, 7, 2, 8, 4, 1, 3, 5, 0, 0, 3, 4, 2, 7, 5, 9, 9, 2, 2, 530, -1
            },{1, 5, 2, 0, 7, 3, 9, 6, 9, 1, 4, 8, 9, 5, 4, 3, 7, 8, 9, 2, 312, -1
            },{0, 1, 4, 5, 2, 9, 3, 5, 1, 1, 6, 2, 3, 9, 9, 8, 2, 4, 8, 3, 512, -1
            },{4, 6, 2, 6, 2, 2, 6, 9, 2, 1, 0, 5, 6, 9, 5, 5, 0, 4, 4, 0, 540, -1
            },{7, 3, 9, 0, 0, 7, 3, 0, 3, 2, 2, 4, 5, 2, 9, 7, 7, 8, 0, 1, 498, -1
            },{4, 4, 8, 5, 8, 5, 0, 3, 2, 3, 1, 1, 1, 8, 7, 2, 4, 0, 7, 4, 129, -1
            },{4, 2, 8, 2, 0, 2, 4, 3, 8, 2, 7, 4, 2, 8, 0, 1, 5, 8, 5, 1, 148, -1
            },{1, 2, 3, 1, 4, 7, 0, 5, 1, 0, 9, 2, 0, 7, 7, 3, 0, 7, 6, 0, 119, -1
            } };//{1, 1, 5, -1 }, {3, 2, 7, 1 } };//{ 2, 1, 1, 14, -1 }, { 4, 2, 3, 28,-1 }, { 1, 5, 5, 30,-1 }, {2,-1,0,5,1 } };//,{ 1,0,0,5,1}
            for (int i = 0; i < num_constraints; i++)
            {
                for (int j = 0; j < num_decisions + 2; j++)
                {
                    local_constraints[i, j] = constraints_input[i, j];
                }
            }
            for (int i = 0; i < num_bin; i++)
            {
                local_constraints[num_constraints + i, num_decisions - num_bin + i] = 1;
                local_constraints[num_constraints + i, num_decisions] = 1;
                local_constraints[num_constraints + i, num_decisions + 1] = -1;
            }
            return local_constraints;
        }
        static double[] Init_Decisions(int num_decisions)
        {//all objectives are assumed to be maximisation
            double[] decisions_out = { 7,    6,   5,   9,   4,   7,   1,   6,   9,   5,   10,  10,  3,   9,   3,   6,   8,   3,   9,   8, 0 };
            return decisions_out;
            //{ 7, 6, 5, 9, 4, 7, 1, 6, 9, 5, 0 };//{ 1.01,1.02,1.03,1.04,1.05,0 };//{4,3,0 };
        }
        static double[] Init_FlowDecisions(int num_decisions, int m, int n)
        {
            double[] decisions_out = new double[num_decisions];
            for(int k = 0; k < n; k++)
            {
                decisions_out[(m - 1) * n + k] = 1;//I
            }

            return decisions_out;
        }
        static double[,] Init_FlowConstraints(int num_decisions, int num_constraints, int num_real,int num_int, int num_bin)
        {//IWX
            double[,] process = { {2.5,1.25,3.25,3.5,3,2.5,0.5,3.75 },{1.25,1.75,0.75,2.5,2.25,3,1,3.5 } };
            int m = 2; //num of machines
            int n = 8; //num of jobs
            int count = 0;//which constraint we are up to
            num_constraints = m * n + n+m-1;//change num contraints for flowshop
            num_decisions =n*n+2*m*n;
            num_real = 2 * m * n;
            num_int = 0;
            num_bin =n*n;
            double[,] local_constraints = new double[num_constraints + num_bin + Global.depth_limit * num_decisions, num_decisions + 2];
            for (int k=0;k<n;k++)
            {
                for (int i=0;i<n;i++)
                {//all of the X's for job k in this constraint have a 1 coeficient
                    local_constraints[count, 2*n*m+n * i + k] = 1;
                }
                local_constraints[count++, num_decisions] = 1;
            }
            for (int i = 0; i < n; i++)
            {
                for (int k = 0; k < n; k++)
                {//all of the X's for machine i in this constraint have a 1 coeficient
                    local_constraints[count, 2 * n * m + n * i + k] = 1;
                }
                local_constraints[count++, num_decisions] = 1;
            }
            //balance equation
            for (int j=0;j<m-1;j++)
            {
                for (int k=1;k<n;k++)
                {
                    local_constraints[count, j * n + k] = 1;//I
                    local_constraints[count, (j+1) * n + k] = -1;//I
                    for (int i=0;i<n;i++)
                    {
                        local_constraints[count, 2 * n * m + n * i + k] = process[j,i];//X
                        local_constraints[count, 2 * n * m + n * i + k-1] = -process[j+1,i];//X
                    }
                    local_constraints[count, n * m + j * m + k] = 1;//W
                    local_constraints[count, n * m + j * m + k-1] = -1;//W
                    count++;
                }
                
            }
            //idle before 1st job
            for (int j = 1; j < m; j++) 
            {
                for (int i = 0; i < n; i++)
                {
                    for (int r = 0; r < j - 1; r++)
                    {
                        local_constraints[count, 2 * n * m + i * n] = -process[r,i];//X
                    }
                }
                local_constraints[count++, j] = 1;//I
            }
            //first job 0 transfer delay
            for (int j = 0; j < m-1; j++)
            {
                local_constraints[count++, n * m + j * m] = 1;//W
            }
            Console.WriteLine(count);
            //add binary variable constraints
            for(int i = 0; i < num_bin; i++)
            {
                local_constraints[num_constraints + i, num_decisions - num_bin + i] = 1;
                local_constraints[num_constraints + i, num_decisions] = 1;
                local_constraints[num_constraints + i, num_decisions + 1] = -1;
            }
            return local_constraints;
        }
        double[] slope_heuristic(int m, int n, int num_decisions)
        {
            double[,] process = { { 2.5, 1.25, 3.25, 3.5, 3, 2.5, 0.5, 3.75 }, { 1.25, 1.75, 0.75, 2.5, 2.25, 3, 1, 3.5 } };
            double[] init_solution = new double[num_decisions+1];
            double[] G = new double[n];
            int[] index = new int[n];
            for(int i = 0; i < n; i++)
            {
                index[i] = i;
                for(int k = 0; k < m; k++)
                {
                    G[i] += (m - 2 * k + 1)*process[k,i];
                }
            }

            Array.Sort(G, index, 1, n);//G is the sorting criteria, index is the reference to the resulting order of jobs
            //calculate the decision variables and objective value from this ordering
            double[] mach_times = new double[m];
            for(int i = 0; i < n; i++)
            {
                init_solution[n*i+index[i]]= 1;//set the X for this ordering
                for(int j = 0; j < m; j++)
                {
                    if (i == 0)
                    {
                        if (j == 0)
                        {
                            mach_times[j] = process[j,index[i]];
                        }
                        else
                        {
                            mach_times[j] = mach_times[j-1]+process[j, index[i]];
                        }

                    }
                    else
                    {
                        if (j == 0)
                        {
                            mach_times[j] = mach_times[j] + process[j, index[i]];
                        }
                        else
                        {//edit I or W depending on where the unproductive time is
                            if(mach_times[j - 1]> mach_times[j])
                            {//W
                                mach_times[j] = mach_times[j - 1] + process[j, index[i]];
                                init_solution[n * n + n * m + n * j + index[i]] = mach_times[j - 1] - mach_times[j];
                            }
                            else
                            {//I
                                mach_times[j] = mach_times[j] + process[j, index[i]];
                                init_solution[n * n + n * j + index[i]] = -mach_times[j - 1] + mach_times[j];
                            }
                            //mach_times[j] = Math.Max(mach_times[j - 1], mach_times[j]) + process[j, index[i]];
                        }
                    }
                    
                }
                init_solution[num_decisions] = mach_times[m - 1];
            }
            return init_solution;
        }
        static void Branch(double[,] constraint_input,double[] decisions,int num_decisions,int num_constraints,int num_real, int num_int, int num_bin)
        {//the recursive function responsible for the branching portion of the algorithm
            
            double[] solution_local = MySimplex(decisions,constraint_input,num_constraints + num_bin + Global.count_depth, num_decisions+num_constraints + num_bin + Global.count_depth);//the values of each decision variable, with the objective as the last value in the array
            double[,] constraint_local = constraint_input;//is this line necessary, or can I just edit the input directly?
            bool[] nonint_decisions = new bool[num_decisions-num_real];//determines whether a decision variable is at an integer solution or not. True is integer
            bool isint = true;//used to determine if the solution is an integer solution
            int branch_choice=0;//determines which variable should be chosen in the even of multiple non-ints. base is first
            int branch_count = 0;//the counter for the default variable choice functionality
            //int[] inequality = { -1, 1 };//determine whether to be rounddown and less than or roundup and greater than. Used when branching on nonint
            //return;
            
            if (Global.instance_count++ > 310)
            {
                int kappa = 1;
            }
            if (Global.watch2.ElapsedMilliseconds > 300000)//time limit 5 minutes
            {
                return;
            }

            if (solution_local != null)
            {//if the solution is infeasible, fathom
                //if (solution_local[num_decisions] > 1098.5)
                //{
                //    Global.instance_count++;
                //}
                if (solution_local[num_decisions] >= Global.upper_bound)
                {//only continue if its actually possible to get a better answer

                    for (int i = 0; i < num_decisions-num_real; i++)
                    {
                        nonint_decisions[i] = (solution_local[i+num_real] % 1 <= Global.tolerance)|| (solution_local[i + num_real] % 1 >= 1-Global.tolerance);//some tolerance for int
                        if (!nonint_decisions[i])
                        {
                            isint = false;//check to see if its an integer solution
                        }
                    }
                    if (isint)
                    {
                        if (solution_local[num_decisions] > Global.upper_bound)//if the integer solution is an improvement, this is the new best solution. Only care about getting 1 optimal solution
                        {
                            Global.incum_exist = true;
                            Global.upper_bound = solution_local[num_decisions];
                            Global.best_depth = Global.count_depth;
                            for (int dvar = 0; dvar <= num_decisions; dvar++)
                            {
                                Global.best_solution[dvar] = solution_local[dvar];//the casting might go weird, check here for errors
                            }
                            if (!Global.lbranch_local_active)
                            {//ensures that an incumbent solution must be found outside of a local branch subproblem to enable local branching again
                                //if this isn't done then it is possible for a lbranch constraint to be added that directly conflicts with
                                Global.lbranch_incumb = true;
                            }
                        }
                    }
                    else//if its not an integer solution, branch on each nonint variable
                    {
                        Global.count_depth++;
                        if (Global.count_depth > Global.depth_limit * num_decisions)
                        {
                            throw new ArgumentException("Depth is greater than the maximum allowed depth");
                        }
                        if(!Global.incum_exist || !Global.cost_heuristic_enabled|| cost_heuristic(solution_local,num_decisions,num_constraints))// new code
                        if (num_bin>=1 && Global.lbranch_active==1 && !Global.lbranch_local_active && Global.lbranch_incumb)
                        {//if local branching is enabled, do that. otherwise, don't
                            Local_Branch(num_bin/Global.k_multi, constraint_local, decisions, num_decisions, num_constraints, num_real, num_int, num_bin);//k is a quarter of the binary variabled, rounded down
                        }
                        else
                        {
                            for (int i = 0; i < num_decisions - num_real; i++)
                            {
                                if (!nonint_decisions[i] && (branch_choice == branch_count++))
                                {//if the decision is nonint, branch on it. else don't
                                    for (int j = 0; j < 2; j++)
                                    {//first, add the constraint to lock the selected variable to int
                                        if (j == 1)
                                        {//if less than, round down
                                            constraint_local[num_constraints + num_bin + Global.count_depth - 1, num_decisions] = Math.Floor(solution_local[i + num_real]);
                                            constraint_local[num_constraints + num_bin + Global.count_depth - 1, i + num_real] = 1;
                                            constraint_local[num_constraints + num_bin + Global.count_depth - 1, num_decisions + 1] = -1;
                                        }
                                        else
                                        {//if greater than, round up
                                            constraint_local[num_constraints + num_bin + Global.count_depth - 1, num_decisions] = Math.Ceiling(solution_local[i + num_real]);
                                            constraint_local[num_constraints + num_bin + Global.count_depth - 1, i + num_real] = 1;//*-1 to make the >= flip to <=
                                            constraint_local[num_constraints + num_bin + Global.count_depth - 1, num_decisions + 1] = 1;
                                        }


                                        //constraint_local[num_decisions+1, num_decisions + Global.count_depth] = inequality[j]; not needed now that we assume <=

                                        Branch(constraint_local, decisions, num_decisions, num_constraints, num_real, num_int, num_bin);//branch with the new constraints. The constraints will overwrite each time
                                                                                                                                        //undo changes
                                        constraint_local[num_constraints + num_bin + Global.count_depth - 1, num_decisions] = 0;
                                        constraint_local[num_constraints + num_bin + Global.count_depth - 1, i + num_real] = 0;
                                        constraint_local[num_constraints + num_bin + Global.count_depth - 1, num_decisions + 1] = 0;
                                    }

                                }
                            }
                        }
                        Global.count_depth--;//because I used a local constraint, simply exiting the branch will have its predecessor have none of these changes.
                    }
                }
            }
        }
        static double[] Choice_Node()
        {//a method used to choose which node to explore
            return null;
        }
        static bool cost_heuristic(double[] solution_local, int num_decisions, int num_constraints)
            //fathoms branches where the work to progress outways the potential improvement
        {
            double pot_improv = 100*(solution_local[num_decisions]-Global.upper_bound) / Global.upper_bound;//% maximum improvement from this branch
            double upper_cost = 10*Global.best_depth;//some function for cost
            double local_cost = 10*Global.count_depth;//some function for cost
            double improv_coefficient = Global.cost_improv_percent;//10% better solution is worth repeating the depth of the previous solution if this is =10. 1% for 1 and so on
            bool worth_equation = (local_cost<upper_cost*pot_improv/improv_coefficient);//
            return worth_equation;
        }
        static void Local_Branch(double k,double[,] constraint_local,double[] decisions,int num_decisions, int num_constraints,int num_real,int num_int,int num_bin)
            //adds a constraint to the current list to restrict searching to a local area around the binary variables
            //k is the definition of local. It can be static or change. It should be int, but typing is double
        {
            double sum_incumb = 0;
            double current_bin = 0;
            Global.lbranch_depth++;
            for (int i=0;i<num_bin;i++)
            {//first add constraint for the local
                current_bin = Global.best_solution[num_decisions-num_bin+i];
                sum_incumb += current_bin;
                if (current_bin == 1)
                {
                    //add to constraint
                    constraint_local[num_constraints + num_bin + Global.count_depth-1, num_decisions - num_bin + i] =-1;
                }
                else
                {
                    constraint_local[num_constraints + num_bin + Global.count_depth -1, num_decisions - num_bin + i] = 1;
                }
            }
            constraint_local[num_constraints + num_bin + Global.count_depth -1, num_decisions] = k-sum_incumb;//could be a prob: negative rhs
            constraint_local[num_constraints + num_bin + Global.count_depth -1, num_decisions+1] = -1;
            Global.lbranch_local_active = true;
            Global.lbranch_incumb = false;//require a new incumbent solution to do more local branching
            Branch(constraint_local, decisions, num_decisions, num_constraints, num_real, num_int, num_bin);
            Global.lbranch_local_active = false;
            //edit constraint for non-local
            constraint_local[num_constraints + num_bin + Global.count_depth -1, num_decisions] = k - sum_incumb+1;
            constraint_local[num_constraints + num_bin + Global.count_depth -1, num_decisions + 1] = 1;
            Branch(constraint_local, decisions, num_decisions, num_constraints, num_real, num_int, num_bin);
            //remove constraint: cleanup
            constraint_local[num_constraints + num_bin + Global.count_depth -1, num_decisions] = 0;
            constraint_local[num_constraints + num_bin + Global.count_depth -1, num_decisions + 1] = 0;
            for (int i = 0; i < num_bin; i++)
            {
                constraint_local[num_constraints + num_bin + Global.count_depth -1, num_decisions - num_bin + i] = 0;
            }
            Global.lbranch_depth--;
        }
             
        static double[] MySimplex(double[] objective_in,double[,] constraints_in,int ai_m, int ai_n)
        {//constraints is my constraints, length ai_n+2
            //ai_n is num_decisions
            //ai_m is num_constraints
            //var watch2 = new System.Diagnostics.Stopwatch();
            //watch2.Start();
            double gd_lp_tol = 0.000005;
            //d_t
            
            int[] i_basis= new int[ai_m];

            int i_b;
            int i_iteration=0;
            int i_pivot_j=-1;//so that I get an index error if it never gets assigned
            int i_pivot_i;
            int ai_decis = ai_n - ai_m;
            

            double d_max;
            double d_min;
            double d_tmp;
            double ad_z;//something to do with output

            bool b_done=false;
            bool b_first;
            bool b_isbasis;

            //create identity matrix in constraints for slack variables?
            
            int greaterthan = 0;//the number of greater than contstraints
            //determine the size of the constraints and the objective by counting when -slack+dummy happens
            for(int i=0;i<ai_m;i++)
            {
                if (constraints_in[i, ai_decis + 1]==1)
                {
                    greaterthan++;
                }
            }
            double[,] constraints = new double[ai_m, ai_n + 1+greaterthan];
            double[] objective = new double[ai_n+greaterthan];
            int varcount = 0;
            for (int i = 0; i < ai_m; i++)
            {
                for(int j = 0; j < ai_decis; j++)
                {//copy the start
                    constraints[i, j] = constraints_in[i, j];
                    objective[j] = objective_in[j];
                }
                

                constraints[i, ai_n+greaterthan] = constraints_in[i, ai_n - ai_m];
                constraints[i, i + ai_decis+greaterthan] = 1;//for the identity; the initial basis
                //if its a less than constraint, slack only, so this is already enough
                if (constraints_in[i, ai_n - ai_m + 1] == 0)
                {//if its an equal to constraint, dummy
                    objective[i + ai_decis+greaterthan] = -Global.bigM;
                    //varcount++;
                }
                if (constraints_in[i, ai_n - ai_m + 1] == 1)
                {//if its a greater than constraint, dummy and -slack
                    objective[i + ai_decis+greaterthan] = -Global.bigM;
                    constraints[i, ai_decis + varcount] = -1;
                    varcount++;
                }

            }
            int ai_width = ai_n + greaterthan;
            double[,] simplex = new double[ai_m, ai_width+1];
            double[] d_zj = new double[ai_width + 1];
            double[] d_zjcj = new double[ai_width + 1];
            double[] ad_x = new double[ai_width];//solution output
            //objective[ai_n] = objective_in[ai_n - ai_m];
            //-------------------------------------------------------------------------------------------------------
            //identify the initial basis
            for (int j=0;j<ai_width;j++)//*
            {
                b_isbasis = true;
                i_b = 0;
                int i = 0;

                while(b_isbasis && (i < ai_m))
                {
                    if ((Math.Abs(constraints[i, j]) > gd_lp_tol) && (Math.Abs(1 - constraints[i, j]) > gd_lp_tol))
                    {
                        b_isbasis = false;
                        
                    }else if((Math.Abs(1-constraints[i, j]) <= gd_lp_tol) && (i_b > 0))
                    {
                        b_isbasis = false;
                    }else if((Math.Abs(1-constraints[i, j]) <= gd_lp_tol))
                    {
                        i_b = i;
                    }
                    i++;
                }
                if (b_isbasis)
                {
                    i_basis[i_b] = j;//note: pauls j is one higher than mine
                }
            }
            //initialise tableau
            for(int i = 0; i < ai_m; i++)
            {
                for(int j = 0; j < ai_width; j++)//*
                {
                    simplex[i, j] = constraints[i, j];
                }
                simplex[i, ai_width] = constraints[i,ai_width];//*
            }
            //simplex main loop

            while (!b_done)
            {
                b_done = true;
                for (int j=0;j<(ai_width+1);j++)//*
                {//calculate zj row and cj-zj
                    d_zj[j] = 0;
                    for(int i = 0; i < ai_m; i++)
                    {
                        d_zj[j] = d_zj[j]+objective[i_basis[i]] * simplex[i, j];
                    }
                    if (j <= ai_width - 1)//*
                    {
                        d_zjcj[j] = objective[j] - d_zj[j];
                        b_done = b_done && (d_zjcj[j] <= gd_lp_tol);//optimality test
                        //if (b_done)
                        //{
                        //    int k = 2;
                        //}
                    }
                }

                if (!b_done)
                {
                    i_iteration++;//these are the same rn. Should this start from -1?
                    //select vector to enter the basis
                    d_max = 0;
                    i_pivot_i = -1;
                    for(int j = 0; j < ai_width; j++)
                    {
                        if (d_zjcj[j] > d_max)
                        {
                            d_max = d_zjcj[j];
                            i_pivot_j = j;
                        }
                    }
                    //select vector to leave the basis
                    d_min = 0;
                    i_pivot_i = -1;
                    b_first = true;//this guarantees that i_pivot_i is not -1 after you pass the next if statement
                    for(int i = 0; i < ai_m; i++)
                    {
                        if (simplex[i, i_pivot_j] > 0)//ensures that the denominator is postive
                        {
                            d_tmp = simplex[i, ai_width] / simplex[i, i_pivot_j];//*
                            if(b_first || d_tmp < d_min)
                            {
                                b_first = false;
                                d_min = d_tmp;
                                i_pivot_i = i;
                            }
                        }
                    }
                    if (i_pivot_i == -1)//was 0 for 1-based case
                    {
                        return null;//read null as the lp is infeasible in the main program with isnull
                    }
                    //transform the non-pivot elements of the tableau
                    for(int i = 0; i < ai_m; i++)
                    {
                        if (i != i_pivot_i)
                        {
                            for(int j = 0; j < ai_width + 1; j++)//*
                            {
                                if (j != i_pivot_j)
                                {
                                    simplex[i, j] = simplex[i, j] - simplex[i_pivot_i, j] * simplex[i, i_pivot_j] / simplex[i_pivot_i, i_pivot_j];
                                }
                            }
                        }//random else here that does nothing?
                    }
                    //transform pivot row
                    for(int j = 0; j < ai_width+1; j++)//*
                    {
                        if (j != i_pivot_j)
                        {
                            simplex[i_pivot_i, j] = simplex[i_pivot_i, j] / simplex[i_pivot_i, i_pivot_j];
                        }
                    }
                    //transform pivot column
                    for(int i = 0; i < ai_m; i++)
                    {
                        if (i == i_pivot_i)
                        {
                            simplex[i, i_pivot_j] = 1;
                        }
                        else
                        {
                            simplex[i, i_pivot_j] =0;
                        }
                    }
                    i_basis[i_pivot_i] = i_pivot_j;//update basis
                }
            }
            //copy solution
            for(int j = 0; j < ai_width; j++)//*
            {
                ad_x[j] = 0;//could I just use .clear?
            }
            for(int i = 0; i < ai_m; i++)
            {
                ad_x[i_basis[i]] = simplex[i, ai_width];//*
            }
            ad_z = d_zj[ai_width];//*
            //my edits to fit the rest of my code
            double[] obj_out = new double[objective_in.Length];
            obj_out[obj_out.Length - 1] = 0;
            for (int i = 0; i < obj_out.Length-1; i++)
            {
                //obj_out[objective_in.Length - 1] += objective_in[i] * ad_x[i];
                   obj_out[i] = ad_x[i];
            }
            obj_out[objective_in.Length - 1] = ad_z;
            if (Global.check++ == 100)
            {
                Global.check = 0;
            }
            for(int i = 0;i < ai_n - ai_decis;i++)
            {//if a dummy is in basis its infeasible
                if ((ad_x[ai_decis+greaterthan+i]>0)&&(objective[i + ai_decis + greaterthan] == -Global.bigM))
                {
                    return null;
                }
            }
            //watch2.Stop();
            //Console.WriteLine($"Execution Time: {watch2.ElapsedMilliseconds} ms");
            
            return obj_out;
        }
        static double[] ReadExcel1(string file_location)
        {
           

            // Excel Configuration settings
            //private const int NUM_WORKSHEETS = 1;
            // Array
            const int ARRAY_START_ROW = 0;
            const int ARRAY_START_COL = 0;
            const int ARRAY_NUM_COLS = 20;

            // Matrix
            const int MATRIX_START_ROW = ARRAY_START_ROW + 1;
            const int MATRIX_START_COL = ARRAY_START_COL;
            const int MATRIX_NUM_ROWS = 40;
            const int MATRIX_NUM_COLS = 22;

            double[] array = new double[61];
            double[,] matrix = new double[MATRIX_NUM_ROWS + Global.depth_limit * ARRAY_NUM_COLS, ARRAY_NUM_COLS + 2];//new double[62, 200]; // [row][col]

            //file_location will look something like
            //"C:\users\user\ExcelFile.xlsx"
            //foreach (var worksheet in Workbook.Worksheets(@"C:/Users/Aidan/Desktop/QUT/MXN404/Code/Branch_Benchmark/Branch_Benchmark/TestProblems/CSV_test1.xlsx"))
            foreach (var worksheet in Workbook.Worksheets(@"C:/Users/Aidan/Desktop/QUT/MXN404/Sem2_CodeUpdates/Code_cleaned/Branch_Benchmark/Branch_Benchmark/TestProblems/ExcelTest1.xlsx"))
            //foreach (var worksheet in Workbook.Worksheets(@"C:/Users/n9951911/Documents/Code_Backup/Branch_Benchmark/Branch_Benchmark/TestProblems/ExcelInput.xlsx"))
            {
                int i = 0;
                for (int x = ARRAY_START_COL; x < ARRAY_NUM_COLS + ARRAY_START_COL; x++)
                {
                    // Cell has three functions
                    // CellReference
                    // tType
                    // Value

                    array[i] = Convert.ToDouble(worksheet.Rows[ARRAY_START_ROW].Cells[x].Value);
                    i++;
                }

                
                int j = 0;
                for (int y = MATRIX_START_ROW; y < MATRIX_NUM_ROWS + MATRIX_START_ROW; y++)
                {
                    i = 0;
                    for (int x = MATRIX_START_COL; x < MATRIX_START_COL + MATRIX_NUM_COLS; x++)
                    {
                        matrix[j,i] = Convert.ToDouble(worksheet.Rows[y].Cells[x].Value);
                        i++;
                    }
                    j++;
                }
            }
            return array;
            // Create a return type later
        }
        static double[,] ReadExcel2(string file_location)
        {
            // Excel Configuration settings
            //private const int NUM_WORKSHEETS = 1;
            // Array
            const int ARRAY_START_ROW = 0;
            const int ARRAY_START_COL = 0;
            const int ARRAY_NUM_COLS = 20;

            // Matrix
            const int MATRIX_START_ROW = ARRAY_START_ROW + 1;
            const int MATRIX_START_COL = ARRAY_START_COL;
            const int MATRIX_NUM_ROWS = 40;
            const int MATRIX_NUM_COLS = 22;

            double[] array = new double[61];
            double[,] matrix = new double[MATRIX_NUM_ROWS + Global.depth_limit * ARRAY_NUM_COLS, ARRAY_NUM_COLS + 2];

            //file_location will look something like
            //"C:\users\user\ExcelFile.xlsx"
            foreach (var worksheet in Workbook.Worksheets(@"C:/Users/Aidan/Desktop/QUT/MXN404/Sem2_CodeUpdates/Code_cleaned/Branch_Benchmark/Branch_Benchmark/TestProblems/ExcelTest1.xlsx"))
            //foreach (var worksheet in Workbook.Worksheets(@"C:/Users/n9951911/Documents/Code_Backup/Branch_Benchmark/Branch_Benchmark/TestProblems/ExcelInput.xlsx"))
            {
                int i = 0;
                for (int x = ARRAY_START_COL; x < ARRAY_NUM_COLS + ARRAY_START_COL; x++)
                {
                    // Cell has three functions
                    // CellReference
                    // tType
                    // Value

                    array[i] = Convert.ToDouble(worksheet.Rows[ARRAY_START_ROW].Cells[x].Value);
                    i++;
                }

                
                int j = 0;
                for (int y = MATRIX_START_ROW; y < MATRIX_NUM_ROWS + MATRIX_START_ROW; y++)
                {
                    i = 0;
                    for (int x = MATRIX_START_COL; x < MATRIX_START_COL + MATRIX_NUM_COLS; x++)
                    {
                        matrix[j,i] = Convert.ToDouble(worksheet.Rows[y].Cells[x].Value);
                        i++;
                    }
                    j++;
                }
            }
            return matrix;
            // Create a return type later
        }
        static void Testing_Comparison1()
            //wrap multiple run_base together, changing some global variables and problem size
            //ideally figure out how to read from a text file
        {

        }
        static void Read_Text(double[] decisions, double[,] constraints,int num_decisions,int num_constraints, string filelocation)//  
        {
            var reader = new StreamReader(File.OpenRead(@filelocation));
            List<string> listA = new List<string>();
            int i = 0;
            while (!reader.EndOfStream && (i<=num_constraints))
            {
                var line = reader.ReadLine();
                var values = line.Split('\t');

                //listA.Add(values[0]);
                if (i++ == 0)
                {
                    for (int col=0;col<num_decisions;col++)
                    {
                        decisions[col] = Double.Parse(values[col]);//check if this works
                    }
                }
                else
                {
                    for (int col = 0; col < num_decisions+2; col++)
                    {
                        constraints[i-2,col] = Double.Parse(values[col]);//check here too
                    }
                }
                //foreach(var column1 in listA)
                //{
                //    Console.WriteLine(column1);
                //}
            }
        }
    }
}
