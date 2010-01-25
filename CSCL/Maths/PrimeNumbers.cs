using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace CSCL.Maths
{
    public static class PrimeNumbers
    {
        /// <summary>
        /// Generates a list containing all the dividers of an integer specified by its prime factorization
        /// </summary>
        /// <param name="PrimeFactorization">The prime factorization of the integer</param>
        /// <returns>A List containing the dividers</returns>
        public static List<long> GetDividers(SortedDictionary<long, long> PrimeFactorization)
        {
            SortedDictionary<long, long> DividerMask = new SortedDictionary<long, long>();
            long DividerCount = 1;
            foreach(KeyValuePair<long, long> pfactor in PrimeFactorization)
            {
                DividerCount *= pfactor.Value + 1;
                long divMask = 1;
                foreach(KeyValuePair<long, long> dmask in DividerMask)
                {
                    divMask *= PrimeFactorization[dmask.Key] + 1;
                }
                DividerMask.Add(pfactor.Key, divMask);
            }
            List<long> Dividers = new List<long>(Convert.ToInt32(DividerCount));
            for(long i = 0; i < DividerCount; i++)
            {
                long Divider = 1;
                foreach(KeyValuePair<long, long> pfactor in PrimeFactorization)
                {
                    long pow = (i / DividerMask[pfactor.Key]) % (pfactor.Value + 1);
                    for(long j = 1; j <= pow; j++)
                    {
                        Divider *= pfactor.Key;
                    }
                }
                Dividers.Add(Divider);
            }
            Dividers.Sort();
            return Dividers;
        }

        /// <summary>
        /// test if an integer is prime
        /// </summary>
        /// <param name="Number">the integer</param>
        /// <returns>true if prime else false</returns>
        public static bool IsPrime(long Number)
        {
            bool value;
            PrimeNumberReader pnreader = new PrimeNumberReader();
            pnreader.BeginGetNextPrime();
            if(pnreader.GetNextPrime(Number - 1) == Number)
            {
                value = true;
            }
            else
            {
                value = false;
            }
            pnreader.EndGetNextPrime();
            return value;
        }

        public static bool CheckPrime(int Number)
        {
            for(int i = 2; i <= Number - 1; i++)
            {
                if(Number % i == 0)
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Calculates the prime number factorization of a given integer
        /// </summary>
        /// <param name="Number">The integer to be factorized</param>
        /// <returns>A SortedDictionary where the keys are the prime factors and the corresponding values their exponent</returns>
        public static SortedDictionary<long, long> GetPrimeFactorization(long Number)
        {
            if(Number == 0)
            {
                throw new ArgumentOutOfRangeException("Number", "parameter must be greater than zero");
            }
            SortedDictionary<long, long> factors = new SortedDictionary<long, long>();
            if(Number == 1)
            {
                factors.Add(1, 1);
                return factors;
            }
            long MaxFactor = Convert.ToInt64(System.Math.Ceiling(System.Math.Sqrt(Number)));
            long CurrentPrime;
            PrimeNumberReader pnr = new PrimeNumberReader();
            pnr.BeginGetNextPrime();
            do
            {
                CurrentPrime = pnr.GetNextPrime();
                while((Number % CurrentPrime) == 0)
                {
                    Number /= CurrentPrime;
                    if(factors.ContainsKey(CurrentPrime))
                    {
                        factors[CurrentPrime]++;
                    }
                    else
                    {
                        factors.Add(CurrentPrime, 1);
                    }
                }
                if(Number == 1)
                {
                    break;
                }
            }
            while(MaxFactor > CurrentPrime);
            if(Number != 1)
            {
                if(factors.ContainsKey(Number))
                {
                    factors[Number]++;
                }
                else
                {
                    factors.Add(Number, 1);
                }
            }
            pnr.EndGetNextPrime();
            return factors;
        }

        /// <summary>
        /// A static class for generating prime numbers
        /// Calculated prime numbers are stored in a binary file
        /// </summary>
        /// <remarks>
        /// Usage: 
        /// 
        /// static void Main(string[] args)
        /// {
        ///    PrimeNumberGenerator.PrimeDatabaseFileName = @"C:\myprimes.bin";
        ///    PrimeNumberGenerator.StartBackgroundPrimeNumberGenerator();
        ///    ...
        ///    run your program as usual
        ///    ...
        ///    PrimeNumberGenerator.StopBackgroundPrimeNumberGenerator();
        /// }
        /// </remarks>
        public static class PrimeNumberGenerator
        {
            /// <summary>
            /// Returns the greatest prime number found in the file
            /// </summary>
            public static long GreatestKnownPrimeNumber
            {
                get
                {
                    if(File.Exists(PrimeDatabaseFileName))
                    {
                        long buf;
                        // open it for reading
                        using(BinaryReader reader = new BinaryReader(new FileStream(PrimeDatabaseFileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite)))
                        {
                            // seek to the last long
                            reader.BaseStream.Seek(-sizeof(long), SeekOrigin.End);
                            // read the last long, it's the last found prime number
                            buf = reader.ReadInt64();
                            // close
                            reader.Close();
                        }
                        return buf;
                    }
                    else
                    {
                        return 1;
                    }
                }
            }
            // how many numbers are tested in each run of Eratosthenes()
            private static long _NumbersToTest = 100000;
            /// <summary>
            /// Minimum value for NumbersToTest
            /// </summary>
            public const long MinNumbersToTest = 1000;
            /// <summary>
            /// Maximum value for NumbersToTest
            /// </summary>
            public const long MaxNumbersToTest = 1000000000;
            /// <summary>
            /// Get/set the count of numbers to test in a run of Eratosthenes()
            /// <para>The value is automatically corrected to be in the bounds given by MinNimbersToTest and MaxNumbersToTest</para>
            /// </summary>
            public static long NumbersToTest
            {
                get
                {
                    return _NumbersToTest;
                }
                set
                {
                    // TODO: enter code setting bounds to this value
                    if(value < MinNumbersToTest)
                    {
                        _NumbersToTest = MinNumbersToTest;
                    }
                    else
                    {
                        if(value > MaxNumbersToTest)
                        {
                            _NumbersToTest = MaxNumbersToTest;
                        }
                        else
                        {
                            _NumbersToTest = value;
                        }
                    }
                }
            }
            // true if Eratosthenes() is running as background thread
            private static bool RunThreaded = false;
            // the background thread itself
            private static System.Threading.Thread thread;
            private static string _FileName;
            /// <summary>
            /// The name of the file in which the prime numbers are stored
            /// </summary>
            public static string PrimeDatabaseFileName
            {
                get
                {
                    return _FileName;
                }
                set
                {
                    if(!RunThreaded)
                    {
                        _FileName = value;
                    }
                    else
                    {
                        throw new InvalidOperationException("Stop generator using StopBackgoundPNG() first");
                    }
                }
            }
            /// <summary>
            /// Initializes the prime number generator with a default filename
            /// <para>FileName = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + "\\PrimeNumberDatabase.bin";</para>
            /// </summary>
            static PrimeNumberGenerator()
            {
                _FileName = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + "\\PrimeNumberDatabase.bin";
            }
            /// <summary>
            /// Tests NumberToTest numbers for prime and saves found primes to PrimeDatabaseFileName
            /// </summary>
            public static void Eratosthenes()
            {
                // do at least once
                do
                {
                    // file access variables
                    BinaryWriter writer = null;
                    BinaryReader reader = null;
                    // we do not need any exception here
                    try
                    {
                        // every number to test is an element in this array
                        bool[] Zahlen = new bool[NumbersToTest];
                        // initializes the array to false meaning the number is prime
                        Zahlen.Initialize();
                        // initialize last found prime number with 1
                        // although 1 is not a prime number it is necessary here
                        // anyway, algorithm starts by successor
                        long LastFoundPrimeNumber = 1;
                        // is prime number file existing?
                        if(File.Exists(PrimeDatabaseFileName))
                        {
                            // if yes:
                            // open it for reading
                            reader = new BinaryReader(new FileStream(PrimeDatabaseFileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite));
                            // seek to the last long
                            reader.BaseStream.Seek(-sizeof(long), SeekOrigin.End);
                            // read the last long, it's the last found prime number
                            LastFoundPrimeNumber = reader.ReadInt64();
                            // seek back to beginning
                            reader.BaseStream.Seek(0, SeekOrigin.Begin);
                        }

                        // first number to be test
                        // this is the number represented by Zahlen[0]
                        long ZIB = LastFoundPrimeNumber + 1;
                        // calc last number which multiples must be tested
                        long MaxSieb = Convert.ToInt64(System.Math.Ceiling(System.Math.Sqrt(LastFoundPrimeNumber + NumbersToTest)));
                        // the number of which multiples are beeing eliminated
                        long AktSieb = 0;
                        // if we have to look up prime numbers in Zahlen
                        // start at this index
                        long StartIndex = 0;
                        // repeat until all multiples of all primes lower than MaxSieb are eliminated
                        do
                        {
                            // current index
                            long SuchIndex = StartIndex;
                            // test, where the next prime number can be retrieved
                            // it has to be taken from file if
                            // -ZIB is greater than 2 (meaning that are already prime numbers in the file)
                            // -and current prime (AktSieb) is lower than the greatest prime in file
                            // -and there is actually a file opened
                            if((ZIB > 2) && (AktSieb < LastFoundPrimeNumber) && (reader != null))
                            {
                                // load from file
                                do
                                {
                                    // read a number
                                    SuchIndex = reader.ReadInt64();
                                }
                                // until the number is greater than the current prime number
                                // or end of file
                                while((SuchIndex <= AktSieb) && (reader.BaseStream.Position < reader.BaseStream.Length));
                                // current prime number is the loaded
                                AktSieb = SuchIndex;

                            }
                            else
                            {
                                // find in Zahlen
                                // find first element that is not true (meaning it is not a multiple of any prime number)
                                while(Zahlen[SuchIndex] == true)
                                {
                                    SuchIndex++;
                                }
                                // current prime number is the number represented bei Zahlen[SuchIndex]
                                // (remember: ZIB is the number represented by Zahlen[0])

                            }

                            // find next multiple of the current prime
                            long Sieb = LastFoundPrimeNumber + AktSieb - (LastFoundPrimeNumber % AktSieb);
                            // repeat if the multiple is in the current block
                            while(Sieb < (LastFoundPrimeNumber + NumbersToTest + 1))
                            {
                                // do not "unprime" the prime itself
                                if(Sieb != AktSieb)
                                {
                                    // "unprime" the multiple
                                    Zahlen[Sieb - ZIB] = true;
                                }
                                // elevate to next mutliple
                                Sieb += AktSieb;
                            }

                            // elevate look up start point for next loop
                            StartIndex++;
                        }
                        // loop until we have checked all possible primes
                        while(AktSieb <= MaxSieb);

                        // if there is input file
                        if(reader != null)
                        {
                            // close it
                            reader.Close();
                            // and null it for next loop
                            reader = null;
                        }

                        // write data to file (append)
                        writer = new BinaryWriter(new FileStream(PrimeDatabaseFileName, FileMode.Append, FileAccess.Write, FileShare.ReadWrite));
                        // loop all elements of Zahlen
                        for(long i = 0; i < Convert.ToInt64(Zahlen.LongLength); i++)
                        {
                            // false is meaning that the number has not been multiple of anything
                            // so it is prime
                            if(Zahlen[i] == false)
                            {
                                // write it to file
                                // remember: ZIB is the number represented by Zahlen[0]
                                writer.Write(((long)(ZIB + i)));
                            }
                        }
                    }
                    // if we encounter any exception above
                    // we have to make sure, all files are closed before the next loop
                    finally
                    {
                        // is input file existing
                        if(reader != null)
                        {
                            // close it
                            reader.Close();
                            // null it for next loop
                            reader = null;
                        }
                        // same for output file
                        if(writer != null)
                        {
                            writer.Flush();
                            writer.Close();
                            writer = null;
                        }
                    }
                }
                // repeat this if running as background thread
                while(RunThreaded);

            }

            /// <summary>
            /// Signals the generator to stop after next loop
            /// </summary>
            public static void EndBackgroundPNG()
            {
                RunThreaded = false;
            }

            /// <summary>
            /// Immediately stops a running background prime number generator
            /// </summary>
            public static void StopBackgroundPNG()
            {
                // if there is no thread return
                if(thread == null)
                {
                    return;
                }
                // if this is false Eratosthenes() will not loop
                RunThreaded = false;
                // raise exception in Eratosthenes()
                thread.Abort();
                // wait until Eratosthenes() closes all files
                thread.Join(5000);
                // remove thread
                thread = null;
            }

            /// <summary>
            /// starts the prime number generator to run in the background
            /// </summary>
            public static void StartBackgroundPNG()
            {
                // if there is already a thread return
                if(thread != null)
                {
                    RunThreaded = true;
                    return;
                }
                // create thread for Eratosthenes()
                thread = new System.Threading.Thread(new System.Threading.ThreadStart(Eratosthenes));
                // inform Eratosthenes() about running continuosly
                RunThreaded = true;
                // it's only a background thread (will be aborted if main threads end)
                thread.IsBackground = true;
                // run with lowest priority (it shall be real background)
                thread.Priority = System.Threading.ThreadPriority.Lowest;
                // start
                thread.Start();
            }
        }

        /// <summary>
        /// A class for sequentially reading prime numbers from the file specified in PrimeNumberGenerator.PrimeDatabaseFileName
        /// </summary>
        /// <remarks>
        /// Usage: 
        /// 
        /// static void Main(string[] args)
        /// {
        ///    PrimeNumberReader pnr = new PrimeNumberReader()
        ///    pnr.BeginGetNextPrime();
        ///    ...
        ///    pnr.GetNextPrime();
        ///    ...
        ///    pnr.EndGetNextPrime();
        /// }
        /// </remarks>
        public class PrimeNumberReader
        {
            private BinaryReader file = null;
            /// <summary>
            /// Prepare for reading prime numbers from file
            /// </summary>
            public void BeginGetNextPrime()
            {
                if(File.Exists(PrimeNumberGenerator.PrimeDatabaseFileName))
                {
                    file = new BinaryReader(new FileStream(PrimeNumberGenerator.PrimeDatabaseFileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite));
                }
                else
                {
                    throw new PrimeNumbersNotCalculatedException();
                }
            }
            /// <summary>
            /// End reading prime numbers from file
            /// </summary>
            public void EndGetNextPrime()
            {
                file.Close();
                file = null;
            }
            /// <summary>
            /// Get the next prime number in file
            /// </summary>
            /// <returns>The next prime number found in file at current position</returns>
            public long GetNextPrime()
            {
                if(file == null)
                {
                    throw new InvalidOperationException("Call BeginGetNextPrime() first");
                }
                if(file.BaseStream.Position < file.BaseStream.Length)
                {
                    return file.ReadInt64();
                }
                else
                {
                    throw new PrimeNumbersNotCalculatedException();
                }
            }
            /// <summary>
            /// Get the next prime number in file greater than a specified number
            /// </summary>
            /// <param name="Number">The prime has to be greater than this number</param>
            /// <returns>The next prime greater than the specified number</returns>
            public long GetNextPrime(long Number)
            {
                if(file == null)
                {
                    throw new InvalidOperationException("Call BeginGetNextPrime() first");
                }
                long intern = 0;
                while((file.BaseStream.Position < file.BaseStream.Length) && (intern < Number))
                {
                    intern = file.ReadInt64();
                }
                if(intern > Number)
                {
                    return intern;
                }
                else
                {
                    throw new PrimeNumbersNotCalculatedException();
                }
            }
        }
        /// <summary>
        /// This exception is thrown if a prime number is needed that is greater
        /// than any by now calculated prime numbers => calculate more
        /// </summary>
        public class PrimeNumbersNotCalculatedException: System.Exception
        {
            public long GreatestPrimeByNow
            {
                get
                {
                    return PrimeNumberGenerator.GreatestKnownPrimeNumber;
                }
            }
            public PrimeNumbersNotCalculatedException() : base()
            {
            }
        }
    }
}
