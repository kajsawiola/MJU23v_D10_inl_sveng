namespace MJU23v_D10_inl_sveng
{
    internal class Program
    {
        static List<SweEngGloss> dictionary;
        class SweEngGloss
        {
            public string word_swe, word_eng;
            public SweEngGloss(string word_swe, string word_eng)
            {
                this.word_swe = word_swe; this.word_eng = word_eng;
            }
            public SweEngGloss(string line)
            {
                string[] words = line.Split('|');
                this.word_swe = words[0]; this.word_eng = words[1];
            }
        }
        static void Main(string[] args)
        {
            string defaultFile = "..\\..\\..\\dict\\sweeng.lis";
            Console.WriteLine("Welcome to the dictionary app!");
            do
            {
                Console.Write("> ");
                string[] argument = Console.ReadLine().Split();
                string command = argument[0];
                if (command == "quit")
                {
                    Console.WriteLine("Goodbye!");
                }
                else if (command == "help")
                {
                    Console.WriteLine($"Följande kommandon finns:\n" +
                    $"load:                      Ladda filen 'sweng.lis'.\n" +
                    $"load /file:                Ladda angien fil. \n" +
                    $"list:                      Lista alla ord som finns laddade i ordboken. \n" +
                    $"help:                      Listar alla funktioner i programmet.\n" +
                    $"new:                       Lägga till en ny översättning.\n" +
                    $"new /sve ord /eng ord:     Lägga till angiven översättning.\n" +
                    $"delete:                    Radera en översättning. \n" +
                    $"delete /sve ord /eng ord:  Radera angiven översättning.\n" +
                    $"translate /ord:            Översätter angivet ord. \n" +
                    $"translate:                 Översätter.\n" +
                    $"quit:                      Avlsutar programmet");
                }
                else if (command == "load")
                {
                    if (argument.Length == 2)
                    {
                        try
                        {
                            using (StreamReader sr = new StreamReader(argument[1]))
                            {
                                LaddaFil(sr);
                            }
                        }
                        catch (FileNotFoundException)
                        {
                            Console.WriteLine("Angiven fil kunde inte hittas, försök gärna med en ny");
                        }
                    }
                    else if (argument.Length == 1) //FixME: skriva ut att filen är laddad.
                    {
                        using (StreamReader sr = new StreamReader(defaultFile))
                        {
                            LaddaFil(sr);
                        }
                    }
                }
                else if (command == "list") //FIXME: Exception, om inget finns laddat att visa.
                {
                    foreach (SweEngGloss gloss in dictionary)
                    {
                        Console.WriteLine($"{gloss.word_swe,-10}  - {gloss.word_eng,-10}");
                    }
                }
                else if (command == "new") 
                {
                    if (argument.Length == 3) //FiXME: Skriva ut "testa igen" om bara ett ord anges. 
                        try
                        {
                            {
                                dictionary.Add(new SweEngGloss(argument[1], argument[2]));
                            }
                        }
                        catch (System.NullReferenceException)
                        {
                            Console.WriteLine("Du har inte laddat någon lista att lägga till i, gör det först genom att använda kommandot load.");
                        }

                    else if (argument.Length == 1)
                    try 
                    {
                            { string sveOrd, engOrd;
                                MataInOrd(out sveOrd, out engOrd);

                                dictionary.Add(new SweEngGloss(sveOrd, engOrd));
                            }
                            }
                        catch (NullReferenceException)
                        {
                            Console.WriteLine("Du har inte laddat någon lista att lägga till i, gör det först genom att använda kommandot load.");
                        }
                }
                else if (command == "delete") 
                {
                    try
                    {
                        if (argument.Length == 3)
                        {
                            string sveOrd = argument[1];
                            string engOrd = argument[2];
                            Delete(sveOrd, engOrd); int index = -1;
                        }
                        else if (argument.Length == 1)
                        {
                            string sveOrd, engOrd;
                            MataInOrd(out sveOrd, out engOrd);

                            Delete(sveOrd, engOrd);
                        }
                    }
                    catch (NullReferenceException)
                    {
                        Console.WriteLine("Du har inte laddat någon lista att radera ifrån, gör det först genom att använda kommandot load.");
                    }
                    catch (ArgumentOutOfRangeException)
                    {
                        Console.WriteLine("Det angivna ordet finns inte i listan, prova gärna med ett nytt");
                    }
                }
                else if (command == "translate") //FIXME: skriva ut info om angivet ord inte finns i ordlistan. 
                {
                    if (argument.Length == 2)
                    {
                        string ord = argument[1];
                        Translate(ord);
                    }
                    else if (argument.Length == 1)
                    {
                        Console.WriteLine("Write word to be translated: ");
                        string inmatOrd = Console.ReadLine();
                        Translate(inmatOrd);
                    }
                }
                else
                {
                    Console.WriteLine($"Unknown command: '{command}'");
                }
            }
            while (true);
        }

        private static void LaddaFil(StreamReader sr)
        {
            dictionary = new List<SweEngGloss>(); // Empty it!
            string line = sr.ReadLine();
            while (line != null)
            {
                SweEngGloss gloss = new SweEngGloss(line);
                dictionary.Add(gloss);
                line = sr.ReadLine();
            }
        }

        private static void MataInOrd(out string sveOrd, out string engOrd)
        {
            Console.WriteLine("Write word in Swedish: ");
            sveOrd = Console.ReadLine();
            Console.Write("Write word in English: ");
            engOrd = Console.ReadLine();
        }

        private static void Delete(string sveOrd, string engOrd)
        {
            int index = -1;
            for (int i = 0; i < dictionary.Count; i++)
            {
                SweEngGloss gloss = dictionary[i];
                if (gloss.word_swe == sveOrd && gloss.word_eng == engOrd)
                    index = i;
            }
            dictionary.RemoveAt(index);
        }

        private static void Translate(string ord)
        {
            foreach (SweEngGloss gloss in dictionary)
            {
                if (gloss.word_swe == ord)
                    Console.WriteLine($"English for {gloss.word_swe} is {gloss.word_eng}");
                if (gloss.word_eng == ord)
                    Console.WriteLine($"Swedish for {gloss.word_eng} is {gloss.word_swe}");
            }
        }
    }
}