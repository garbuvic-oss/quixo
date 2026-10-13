using System;
using System.Collections.Generic;
using System.Text;
using Quixo.Lib.Model;
namespace Quixo.Lib.Manager.Freeze
{
    public class FreezedCubeManager : IFreezedCubeManager
    {
        private readonly Random random;

        public FreezedCubeManager(Random? random = null)
        {
            this.random = random ?? Random.Shared;
        }

        public FreezedCube SelectRandomCube()
        {
            FreezedCube f = new(Enum.Symbol.NONE, 0, 0);
            f.X = random.Next(0, 5);
            f.Y = random.Next(0, 5);
            return f;

        }
    }
}