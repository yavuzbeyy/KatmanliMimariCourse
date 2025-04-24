using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KutuphaneServis.Response
{
    public class SuccessResponse<T> : ResponseGeneric<T>
    {
        public SuccessResponse(T data) : base(data, true, "Success")
        {
        }


    }
   
}
