using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Talabat.Core.Entites.Order_Aggregate
{
    public enum OrderStatus
    {
        [EnumMember(Value = "Pending")]         // EnumMember make the value of "Pending" store in db "pending" instead of 0  
        Pending,

        [EnumMember(Value = "Payment Successed")]
        PaymentSuccessed,

        [EnumMember(Value = "Payment Failed")]
        PaymentFailed
    }
}
