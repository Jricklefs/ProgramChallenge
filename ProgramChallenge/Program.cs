using System;
using System.ComponentModel.Design;
using System.Net.Sockets;
using ProgramChallenge.Enums;
using ProgramChallenge.Models;

namespace ProgramChallenge
{
    class Program
    {
        static void Main(string[] args)
        {
            //Ac3pCr/g148OQoCCQSCy8dDjwH7QBwAAzLowAAARRGA= 
           // EmailConvoThread TI = new EmailConvoThread("Ac3pCr/g148OQoCCQSCy8dDjwH7QBwAAzLowAAARRGA=");
           Console.WriteLine("*** START PROCESSING THREAD INDEX");
           EmailConvoThread dc = new EmailConvoThread();
           ProcessThreadObject pto = dc.ProcessThreadObj("Ac3pCr/g148OQoCCQSCy8dDjwH7QBwAAzLowAAARRGA=",
               new ProcessThreadObject());

           if(pto.RawThreadStatus == ConvoResultEnum.Incomplete || pto.RawThreadStatus == ConvoResultEnum.ReplysAreIncomplete)
                Console.WriteLine("This Thread Is Junk");
           else
           {
               Console.WriteLine(" Header Date : " +  pto.ThreadDateTime.ToString("MM/dd/yyyy HH:mm:ss ") +" (UTC)");
               Console.WriteLine(" GUID : " + pto.ThreadId);
               Console.WriteLine("");

               if (pto.RawThreadStatus == ConvoResultEnum.ValidWithReplys)
               {
                   foreach (ChildReply cr in pto.Replys)
                   {
                        Console.WriteLine(" Message Date : " + cr.ReplyDate.ToString("MM/dd/yyyy HH:mm:ss tt")+" (UTC)") ;
                        Console.WriteLine("") ;
                   }
               }
           }

           Console.WriteLine("*** END PROCESSING THREAD INDEX");

           
        }
    }
}
