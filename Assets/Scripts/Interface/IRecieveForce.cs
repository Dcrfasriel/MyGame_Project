using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Interface
{
    public interface IRecieveForce
    {
        public void SetForce(Vector3 Gravity);
    }
}
