using System;

class Program
{
    static void Main()
    {
        PySharp pySharp = new PySharp("test_session");
        pySharp.ClearSession();
        pySharp.RunScript("pysharp.py", false);

        while (true)
        {
            pySharp.WriteMessage(Console.ReadLine());
        }
    }
}