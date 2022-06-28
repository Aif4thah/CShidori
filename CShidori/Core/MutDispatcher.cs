using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CShidori.Core
{
    public class MutDispatcher
    {
        public List<string> Output { set; get; }

        public MutDispatcher(int n, string p)
        {

            if (p != string.Empty)
            {
                this.Output = new List<string>();
                while (n > 0)
                {
                    n -= 1;
                    Random rand = new Random();
                    int r = rand.Next(0, 10);

                    switch (r)
                    {
                        case 0:
                            this.Output.Add(Mutation.BitFlip(rand, p));
                            break;

                        case 1:
                            this.Output.Add(Mutation.AddRandBc(rand,p));
                            break;

                        case 2:
                            this.Output.Add(Mutation.DelChar(rand,p));
                            break;

                        case 3:
                            this.Output.Add(Mutation.RepThreeBytes(rand,p));
                            break;

                        case 4:
                            this.Output.Add(Mutation.RepLine(rand,p));
                            break;

                        case 5:
                            this.Output.Add(Mutation.RepeatStr(rand,p));
                            break;

                        default:
                            this.Output.Add(Mutation.RepRandBc(rand,p));
                            break;
                    }

                }
            }
        }
    }
}
