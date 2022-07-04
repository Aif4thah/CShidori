using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

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
                    int rng = RandomNumberGenerator.GetInt32(10);
                    
                    switch (rng)
                    {
                        case 0:
                            this.Output.Add(Mutation.BitFlip(p));
                            break;

                        case 1:
                            this.Output.Add(Mutation.AddRandBc(p));
                            break;

                        case 2:
                            this.Output.Add(Mutation.DelChar(p));
                            break;

                        case 3:
                            this.Output.Add(Mutation.RepThreeBytes(p));
                            break;

                        case 4:
                            this.Output.Add(Mutation.RepLine(p));
                            break;

                        case 5:
                            this.Output.Add(Mutation.RepeatStr(p));
                            break;

                        default:
                            this.Output.Add(Mutation.RepRandBc(p));
                            break;
                    }

                }
            }
        }
    }
}
