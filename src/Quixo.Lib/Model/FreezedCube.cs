using Quixo.Lib.Enum;
using Quixo.Lib.Manager.Freeze;
using System;
using System.Runtime.Serialization;

namespace Quixo.Lib.Model
{
    [DataContract]
    public class FreezedCube : Cube
    {
        [DataMember]
        public new int X { get; set; }
        [DataMember]
        public new int Y { get; set; }

        public FreezedCube(Symbol symbol, int x, int y) : base(symbol)
        {
            X = x;
            Y = y;
        }

        public override bool IsFrozen => true;
    }
}