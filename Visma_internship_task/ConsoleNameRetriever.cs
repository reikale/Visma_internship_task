using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Visma_internship_task
{
    public class ConsoleNameRetriever
    {
        public virtual string GetNextName()
        {
            return Console.ReadLine();
        }
    }
}
