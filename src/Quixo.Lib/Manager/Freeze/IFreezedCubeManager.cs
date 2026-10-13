using Quixo.Lib.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Quixo.Lib.Manager.Freeze
{
    internal interface IFreezedCubeManager
    {
        public FreezedCube SelectRandomCube();
    }
}
