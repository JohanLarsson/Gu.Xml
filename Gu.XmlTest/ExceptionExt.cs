namespace Gu.XmlTest
{
    using System;

    public static class ExceptionExt
    {

        public static void DumpToConsole(this Exception e, int indent = 0)
        {
            Console.WriteLine(e.GetType().Name);
            Console.Write(e.Message);
            Console.WriteLine();
            Console.WriteLine();
            if (e.InnerException != null)
            {
                DumpToConsole(e.InnerException, indent + 1);
            }
        }
    }
}
